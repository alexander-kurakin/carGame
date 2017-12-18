﻿	using UnityEngine;
	using System.Collections;

	public class InitPurchases : MonoBehaviour {

		public static bool initStore;

		void Start () {
		//subscribing to init finish action
		UM_InAppPurchaseManager.Client.OnServiceConnected += OnBillingConnectFinishedAction;
		UM_InAppPurchaseManager.Client.Connect ();
	}

		private void OnBillingConnectFinishedAction (UM_BillingConnectionResult result) {
			UM_InAppPurchaseManager.Client.OnServiceConnected -= OnBillingConnectFinishedAction;
			if(result.isSuccess) {
				Debug.Log("Connected");
				initStore = true;
			} else {
				Debug.Log("Failed to connect");
				initStore = true;
			}
		}  

	}
