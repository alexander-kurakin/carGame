using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyCoins : MonoBehaviour {

	public AudioClip failAudio, successAudio;
	public string coins_amount;
	public Text coins;
	private int addCoins;

	// Use this for initialization
	void OnMouseUp () {
		if (InitPurchases.initStore) {
			UM_InAppPurchaseManager.Client.OnPurchaseFinished += OnPurchaseFlowFinishedAction;
			UM_InAppPurchaseManager.Client.Purchase (coins_amount);	
		} else {
			new MobileNativeMessage ("Oops!", "Something went wrong! Bad connection =/");
		}
	}



	private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) {
		UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
		if(result.isSuccess) {
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Success";
			switch (coins_amount) {
			case "100_coins":
				addCoins = 100;
				break;
			case "300_coins":
				addCoins = 300;
				break;
			case "500_coins":
				addCoins = 500;
				break;
			case "1000_coins":
				addCoins = 1000;
				break;
			}
			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + addCoins);
			coins.text = PlayerPrefs.GetInt ("Coins").ToString ();

			if (PlayerPrefs.GetString ("Music") != "off") {
				transform.GetChild (0).GetComponent<AudioSource> ().clip = successAudio;
				transform.GetChild (0).GetComponent<AudioSource> ().Play ();
			}
		} else  {
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Failed";


			if (PlayerPrefs.GetString ("Music") != "off") {
				transform.GetChild (0).GetComponent<AudioSource> ().clip = failAudio;
				transform.GetChild (0).GetComponent<AudioSource> ().Play ();
			}
		}
	}

}