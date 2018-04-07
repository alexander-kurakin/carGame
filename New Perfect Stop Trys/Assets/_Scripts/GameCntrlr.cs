﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCntrlr : MonoBehaviour {

    private GameObject carInst;
	private GameObject carNow;
	private int countCars = 0;
	private bool addStop;
    public static int multiplier = 1;

	public GameObject collectCoin;
	public Text score, bestScore, coins;
    public float stopObj = MoveObjects.speed/3;
	public GameObject[] car;
    public GameObject carParked, brakes;
    [HideInInspector]
    public static bool stop, inTurn;   
	public Color winColor;


	void Awake(){
		stop = false;
		CheckClick.click = false;
		MoveObjects.speed = 25f;
	}

    void Start() {
		inTurn = false;
		Application.targetFrameRate = 60;

		for (int i = 0; i < car.Length; i++) {
			if (car[i].name == PlayerPrefs.GetString ("Current car")){
				carNow = car[i];
				break;
			}
		}

				carInst = Instantiate(carNow, new Vector3(5.98f, -0.41f, -9.9f), Quaternion.Euler(0, 270, 0)) as GameObject;
    }

    void Update() {

		if (MoveObjects.speed > 0 && stop) {
			MoveObjects.speed -= stopObj * Time.deltaTime;
			if (MoveObjects.speed < 0)
				MoveObjects.speed = 0;

		}

		if (stop && !PlayerLose.lose && !CreateBorder.blink_destroyed) {
			inTurn = true;

            //подкрутка
			float rot = CreateBorder.side == "Left" ? 400	: -425; //400 -425
            //глубина въезда
            float z = CreateBorder.side == "Left" ? -15f : -5f; //-15f/-5f
            //here lies the core physics..
            carInst.transform.position = 
                Vector3.MoveTowards (carInst.transform.position, 
                                     new Vector3 (carInst.transform.position.x, 
                                                  carInst.transform.position.y, 
                                                  z), 
                                     15 * Time.deltaTime);  //15*

            //поворотики
			carInst.transform.Rotate (Vector3.up * rot * Time.deltaTime );

		if (CreateBorder.side == "Left" && Mathf.Abs (carInst.transform.eulerAngles.y - 90) < 4f)
			StopRotate ();
		if (CreateBorder.side == "Right" && carInst.transform.eulerAngles.y - 90 < 4f)
			StopRotate ();
	}
      
		if (CheckClick.click && !addStop) {
			addStop = true;
			stop = true;
			Instantiate (brakes, new Vector3 (carInst.transform.position.x-8f, 8.2f, carInst.transform.position.z+3f), Quaternion.Euler (0, 0, 0));

		}
    }

    void OnMouseDown() {

		if (!PlayerLose.lose)
        {
			print("Key pressed!");
            stop = true;
			MoveObjects.speed = 25f;
        }
    }

    void StopRotate()
    {
        float zPos = CreateBorder.side == "Left" ? -15f : -5f;
		CreateBorder.blink.GetComponent<MeshRenderer> ().sharedMaterial.color = winColor;
		if (PlayerPrefs.GetString ("Music") != "off") {
			GetComponent<AudioSource> ().Play ();
		}

		StartCoroutine(NextCar(zPos));
        StartCoroutine(SetToBorder(zPos));

        stop = false;
        CheckClick.click = false;
        MoveObjects.speed = 0;
        carInst.transform.rotation = Quaternion.Euler(0, 90, 0);
		inTurn = false;

    }

    IEnumerator NextCar(float zPos)
    {

		Instantiate(carParked, new Vector3 (carInst.transform.position.x,5,carInst.transform.position.z), Quaternion.Euler(41,90,0));

        yield return new WaitForSeconds(1f);
        if (!PlayerLose.lose) {
            print("Next car");
			addStop = false;
			carInst.AddComponent<MoveObjects>();

            if (PlayerPrefs.GetString ("Study") != "yes")
				PlayerPrefs.SetString ("Study", "yes");

			countCars++;
			score.text = countCars.ToString (); 
			if (PlayerPrefs.GetInt("Score") < countCars){
				PlayerPrefs.SetInt ("Score", countCars);
				bestScore.text = "Best: " + countCars.ToString ();
			}

            if (countCars >= 5 && countCars % 5 == 0)
            {
                if (multiplier <= 8)
                    multiplier = multiplier * 2;
                else
                {
                    multiplier = 16;
                }
            }

            if (countCars % 3 == 0) {
                PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + 1*multiplier);
				coins.text = PlayerPrefs.GetInt ("Coins").ToString ();
				if (PlayerPrefs.GetString ("Music") != "off") 
					Instantiate (collectCoin, new Vector3 (0, 0, 0), Quaternion.identity);
                    
			}



            carInst = Instantiate(carNow, new Vector3(5.98f, -0.41f, -9.9f), Quaternion.Euler(0, 270, 0)) as GameObject;
			MoveObjects.speed = Random.Range (25f, 44f);
			CreateBorder.blink.GetComponent<MeshRenderer> ().sharedMaterial.color = winColor;
        }
        CreateBorder.blink.GetComponent<MeshRenderer> ().sharedMaterial.color = winColor;
    }

    IEnumerator SetToBorder(float zPos) {

        while (carInst.transform.position.z != zPos && stop != true) {
            CreateBorder.blink.GetComponent<MeshRenderer>().sharedMaterial.color = winColor;
            carInst.transform.position = Vector3.MoveTowards(carInst.transform.position,
                                                              new Vector3(carInst.transform.position.x,
                                                                           carInst.transform.position.y,
                                                                           zPos),
                                                                          5 * Time.deltaTime); //5 *
            yield return new WaitForSeconds(0.02f);
        }
    }
}