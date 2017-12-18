using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NeedCoins : MonoBehaviour {

	public Text needMore;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("Coins") >= 100) {
			transform.GetChild (0).gameObject.SetActive (false);
			transform.GetChild (1).gameObject.SetActive (true);
		} else {
			transform.GetChild (1).gameObject.SetActive (false);
			transform.GetChild (0).gameObject.SetActive (true);

			needMore.text = "You need " + (100 - PlayerPrefs.GetInt("Coins")).ToString() +" more";
		}
	}
	
}
