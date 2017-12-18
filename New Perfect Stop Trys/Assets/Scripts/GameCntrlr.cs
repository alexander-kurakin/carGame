using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCntrlr : MonoBehaviour {
    
    private GameObject carInst;
	private GameObject carNow;
	private int countCars = 0;
	private bool addStop;

	public GameObject collectCoin;
	public Text score, bestScore, coins;
    public float stopObj = 8f;
	public GameObject[] car;
    public GameObject carParked, brakes;
    [HideInInspector]
    public static bool stop, inTurn;   
	public Color winColor;


	void Awake(){
		stop = false;
		CheckClick.click = false;
		MoveObjects.speed = 20;
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

		if (stop && !PlayerLose.lose) {
			inTurn = true;
			float rot = create_border.side == "Left" ? 300	: -325;
			float z = create_border.side == "Left" ? -20f : 0f;
			carInst.transform.position = Vector3.MoveTowards (carInst.transform.position, new Vector3 (carInst.transform.position.x, carInst.transform.position.y, z), 9 * Time.deltaTime); 
			carInst.transform.Rotate (Vector3.up * rot * Time.deltaTime);

		if (create_border.side == "Left" && Mathf.Abs (carInst.transform.eulerAngles.y - 90) < 4f)
			StopRotate ();
		if (create_border.side == "Right" && carInst.transform.eulerAngles.y - 90 < 4f)
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
			MoveObjects.speed = 20;
        }
    }

    void StopRotate()
    {
        float zPos = create_border.side == "Left" ? -15f : -5f;
		create_border.blink.GetComponent<MeshRenderer> ().sharedMaterial.color = winColor;
		if (PlayerPrefs.GetString ("Music") != "off") {
			GetComponent<AudioSource> ().Play ();
		}
		
		StartCoroutine(NextCar(zPos));

        stop = false;
        CheckClick.click = false;
        MoveObjects.speed = 0;
        carInst.transform.rotation = Quaternion.Euler(0, 90, 0);
		inTurn = false;

    }

    IEnumerator NextCar(float zPos)
    {
		while (carInst.transform.position.z != zPos &&stop!=true) {
			create_border.blink.GetComponent<MeshRenderer> ().sharedMaterial.color = winColor;
			carInst.transform.position = Vector3.MoveTowards (carInst.transform.position, new Vector3 (carInst.transform.position.x, carInst.transform.position.y, zPos), 7 * Time.deltaTime);
			yield return new WaitForSeconds (0.02f);
		}

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

			if (countCars % 3 == 0) {
				PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + 1);
				coins.text = PlayerPrefs.GetInt ("Coins").ToString ();
				if (PlayerPrefs.GetString ("Music") != "off") 
					Instantiate (collectCoin, new Vector3 (0, 0, 0), Quaternion.identity);
			}

			carInst = Instantiate(carNow, new Vector3(5.98f, -0.41f, -9.9f), Quaternion.Euler(0, 270, 0)) as GameObject;
			MoveObjects.speed = Random.Range (18f, 35f);
			create_border.blink.GetComponent<MeshRenderer> ().sharedMaterial.color = winColor;
        }
		create_border.blink.GetComponent<MeshRenderer> ().sharedMaterial.color = winColor;
    }

}