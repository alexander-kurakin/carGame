﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class coins : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text> ().text = PlayerPrefs.GetInt ("Coins").ToString ();
	}
	

}
