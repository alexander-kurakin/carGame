using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class noAds : MonoBehaviour {

	public Sprite inactive;
	public AudioClip failAudio, successAudio, disabled;

	void Start(){
		if (PlayerPrefs.GetString ("no_ads") == "yes") 
			inactiveButton (); 
		else { 
			if (InitPurchases.initStore) {
				if (UM_InAppPurchaseManager.Client.IsProductPurchased ("no_ads")) {
					PlayerPrefs.SetString ("no_ads", "yes");
					inactiveButton ();
				}
			}
		}
	}

	// Use this for initialization
	void OnMouseUp () {
		if (PlayerPrefs.GetString ("no_ads")!="yes"){
		if (InitPurchases.initStore) {
			UM_InAppPurchaseManager.Client.OnPurchaseFinished += OnPurchaseFlowFinishedAction;
			UM_InAppPurchaseManager.Client.Purchase ("no_ads");	
		} else 
			new MobileNativeMessage ("Oops!", "Something went wrong! Bad connection =/");
			} else {
			if (PlayerPrefs.GetString ("Music") != "off") {
				transform.GetChild (0).GetComponent<AudioSource> ().clip = disabled;
				transform.GetChild (0).GetComponent<AudioSource> ().Play ();
			}
		}
}



	private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) {
		UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
		if(result.isSuccess) {
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Success";
			PlayerPrefs.SetString ("no_ads", "yes");
			inactiveButton ();
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
			new MobileNativeMessage ("Oops!", "Something went wrong! Bad connection =/");
		}
	}

	void inactiveButton(){
		GetComponent<Image> ().sprite = inactive;
		transform.GetChild (0).gameObject.transform.localPosition = new Vector3 (transform.GetChild (0).gameObject.transform.localPosition.x, 0, transform.GetChild (0).gameObject.transform.localPosition.z); 
	}

}