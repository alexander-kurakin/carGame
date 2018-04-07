using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadGame : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneLoader.Instance.LoadNewLevel("Menu");
    }
}
