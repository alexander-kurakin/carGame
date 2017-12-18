using UnityEngine;
using System.Collections;

public class ButtonsLose : MonoBehaviour {

	public GameObject buttons, video, carGet;
	private bool done;
	private static int loses = 0;

	// Update is called once per frame
	void Update () {
		if (PlayerLose.lose&&!done){
			buttons.SetActive (true);
			carGet.SetActive (true);
			done = true;
			loses++;
			if (loses % 4 == 0)
				video.SetActive (true);
	}
}
}