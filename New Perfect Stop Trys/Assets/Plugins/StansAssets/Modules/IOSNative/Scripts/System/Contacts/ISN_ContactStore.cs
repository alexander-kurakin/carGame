//#define CONTACTS_API_ENABLED
////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if (UNITY_IPHONE && !UNITY_EDITOR && CONTACTS_API_ENABLED) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif


namespace SA.IOSNative.Contacts {


	public class ContactStore : SA.Common.Pattern.Singleton<ContactStore> {

		#if (UNITY_IPHONE && !UNITY_EDITOR && CONTACTS_API_ENABLED) || SA_DEBUG_MODE
		[DllImport ("__Internal")]
		private static extern void _ISN_RetrievePhoneContacts();

		#endif

		private event Action<ContactsLoadResult> ContactsLoadResult = delegate {};



		//--------------------------------------
		// Initialization
		//--------------------------------------

		void Awake() {
			DontDestroyOnLoad(gameObject);
		}


		//--------------------------------------
		// Public Methods
		//--------------------------------------


		public void RetrievePhoneContacts(Action<ContactsLoadResult> callback) {
			ContactsLoadResult += callback;

			#if (UNITY_IPHONE && !UNITY_EDITOR && CONTACTS_API_ENABLED) || SA_DEBUG_MODE
			 _ISN_RetrievePhoneContacts();
			#endif
		}



		//--------------------------------------
		// Private Methods
		//--------------------------------------


		private Contact ParseContactData(string data) {
			string[] DataArray = data.Split(SA.Common.Data.Converter.DATA_SPLITTER); 

			Contact contact =  new Contact();

			contact.GivenName 	= DataArray[0];
			contact.FamilyName 	= DataArray[1];

			string[] emails =  SA.Common.Data.Converter.ParseArray(DataArray[2]);
			contact.Emails.AddRange(emails);

			string[] countryCodes =  SA.Common.Data.Converter.ParseArray(DataArray[3]);
			string[] digits =  SA.Common.Data.Converter.ParseArray(DataArray[4]);

			for(int i = 0; i < countryCodes.Length; i++) {
				PhoneNumber number =  new PhoneNumber();
				number.CountryCode 	= countryCodes[i];
				number.Digits 		= digits[i];

				contact.PhoneNumbers.Add(number);

			}

			return contact;

		}


		//--------------------------------------
		// Native Events
		//--------------------------------------

		private void OnContactsRetrieveFailed(string errorData) {

			Debug.Log("OnContactsRetrieveFailed");

			var error =  new SA.Common.Models.Error(errorData);

			ContactsLoadResult result =  new ContactsLoadResult(error);
			ContactsLoadResult(result);
			ContactsLoadResult = delegate {};
			
		}


		private void OnContactsRetrieveFinished(string data) {

			Debug.Log("OnContactsRetrieveFinished");

			string[] DataArray = data.Split(new string[] { SA.Common.Data.Converter.DATA_SPLITTER2 }, StringSplitOptions.None);


			List<Contact> contacts =  new List<Contact>();
			for (int i = 0; i < DataArray.Length; i++) {
				if (DataArray[i] == SA.Common.Data.Converter.DATA_EOF) {
					break;
				}

				Contact contact = ParseContactData(DataArray[i]);
				contacts.Add(contact);
			}

			Debug.Log("Loaded " + contacts.Count + " contacts");

			ContactsLoadResult result =  new ContactsLoadResult(contacts);
			ContactsLoadResult(result);
			ContactsLoadResult = delegate {};
		}



	

	}

}


