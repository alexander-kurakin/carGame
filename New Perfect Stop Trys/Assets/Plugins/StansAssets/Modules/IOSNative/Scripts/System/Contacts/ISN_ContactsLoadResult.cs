////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SA.IOSNative.Contacts {


	public class ContactsLoadResult : SA.Common.Models.Result {

		private List<Contact> _Contacts = new List<Contact>();


		//--------------------------------------
		// Initialization
		//--------------------------------------

		public ContactsLoadResult(List<Contact> contacts):base() {
			_Contacts = contacts;
		}

		public ContactsLoadResult(SA.Common.Models.Error error):base(error) { 
		
		}


		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public List<Contact> Contacts {
			get {
				return _Contacts;
			}
		}
	}

}