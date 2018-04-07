using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoginGoogle : MonoBehaviour {

    public Text logTxt;

	// Use this for initialization
	void Start () {

        //PlayGamesPlatform.Instance.Authenticate(SignInCallBack, true);

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.Activate();

	}

    private void SignInCallBack(bool success)
    {
        if (success)
        {
            logTxt.text = "Signed in as: " + Social.localUser.userName;
        }
        else
        {
            logTxt.text = "Not signed in";
        }
    }
	
	// Update is called once per frame
	public void SignIn () {
        
        logTxt.text = "Pushed a button";
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            logTxt.text = "Not signed in, trying to";
            PlayGamesPlatform.Instance.Authenticate(SignInCallBack, false);
        }
        else
        {
            logTxt.text = "I'm doing a sign-out";
            PlayGamesPlatform.Instance.SignOut();
        }
    }
}