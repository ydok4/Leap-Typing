using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Leap;

public class calibration : MonoBehaviour {

	public Material asteroidMaterialYellow;
	public Material asteroidMaterialRed;
	public Material asteroidMaterial;

	public AudioClip wrong;
	public AudioClip correct;
	
	
	private AudioSource source;
	

	//public AudioClip correct;

	/*Accessing HandController script*/
	public HandController controller;

	/*Accessing the leap device*/
	Controller leap_controller;

	/*Input for keyboard calibration*/
	protected readonly string[] Key = { "q", "]", "z", "/"};

	/*Index of Key and KeyPos*/
	protected int index = 0;

	/*distance between keys*/
	public static Vector3 KeyDist;

	/**/
	protected bool Finish = false;

	/*x,y,z of each key position */
	public static List <Vector3> KeyPos = new List<Vector3>();
	
	void Awake(){
		//DontDestroyOnLoad (this.gameObject);
	}

	/*Inialises the starting condition*/
	void Start(){
		index = 0;
		KeyPos.Clear();
		source = GetComponent<AudioSource>();
		CurrentLeap();
	}

	// Update is called once per frame
	void Update () {
		if (index < 4) {
			AsteroidKeyboard.characterList [AsteroidKeyboard.Pos [AsteroidKeyboard.index]].GetComponent<MeshRenderer> ().material = asteroidMaterialYellow;
		}
		if (!leap_controller.IsConnected) {
			Exit ();
		}
		if (index == 4) {
			Calibrate ();
			Exit();
			Application.LoadLevel (1);
		}else if (Input.anyKeyDown) {
			if (Input.GetKeyDown (Key [index])) {
				Debug.Log (Key [index]);
				Vector3 tmp = RightIndexPosition ();
				Debug.Log (tmp);
				if (tmp.x != 0 && tmp.y != 0 && tmp.z != 0) {
					AudioSource.PlayClipAtPoint (correct, Vector3.zero, 1.0f); 
					KeyPos.Add (tmp);
					index++;
					AsteroidKeyboard.characterList [AsteroidKeyboard.Pos [AsteroidKeyboard.index]].GetComponent<MeshRenderer> ().material = asteroidMaterial;
					AsteroidKeyboard.index++;
				} else {
					AsteroidKeyboard.characterList [AsteroidKeyboard.Pos [AsteroidKeyboard.index]].GetComponent<MeshRenderer> ().material = asteroidMaterialRed;
					AudioSource.PlayClipAtPoint (wrong, Vector3.zero, 1.0f);
				}
			}else if(!Input.GetMouseButtonDown(0)){
				AsteroidKeyboard.characterList [AsteroidKeyboard.Pos [AsteroidKeyboard.index]].GetComponent<MeshRenderer> ().material = asteroidMaterialRed;
				AudioSource.PlayClipAtPoint (wrong, Vector3.zero, 1.0f);
			}
		}
	}

	//Returns the leap controller from HandController 
	void CurrentLeap(){
		GameObject go = GameObject.Find ("Calibration");
		HandController speedController = go.GetComponent <HandController> ();
		leap_controller = speedController.leap_controller_;
	} 

	//Finds and stores the xyz of the right index finger
	Vector3 RightIndexPosition(){
		Vector3 tmp = new Vector3(0,0,0);
		Frame frame = leap_controller.Frame();
		foreach (Hand hand in frame.Hands) {
			if(hand.IsRight){
				foreach (Finger finger in hand.Fingers) {
					if(finger.Type.ToString() == "TYPE_INDEX"){
						Vector3 tmp1 = new Vector3(finger.TipPosition.x,finger.TipPosition.y,finger.TipPosition.z);
							return tmp1;
					}
				}
			}
		}
		return tmp;
	}

	//Sets up everything to calibrates the keyboard into xyz
	void Calibrate(){
		for (int i = 0; i < 2; i++) {
			Vector3 tmp = Position (KeyPos [i], KeyPos [i+2]);
			KeyPos.Insert (i+2, tmp);
		}
		CalibrateKeyboard (KeyPos[0], KeyPos[1], 11, 1);	
		CalibrateKeyboard (KeyPos[12], KeyPos[13], 10, 13);
		CalibrateKeyboard (KeyPos[23], KeyPos[24], 9, 24);
		KeyDist = KeyDistance ();
	}

	//Returns a vector3 that holds the xyz of either a or '
	Vector3 Position(Vector3 left, Vector3 right){
		float x = (left.x + right.x) / 2;
		float y = (left.y + right.y) / 2;
		float z = (left.z + right.z) / 2;
		Vector3 tmp = new Vector3(x,y,z);
		return tmp;
	}

	//Calibrates the keyboard into xyz
	void CalibrateKeyboard(Vector3 left, Vector3 right, int Row, int Pos){
		float distx = Math.Abs(left.x - right.x)/Row;
		float x = left.x;
		float disty = Math.Abs(left.y - right.y)/Row;
		float y = left.y;
		float distz = Math.Abs(left.z - right.z)/Row;
		float z = left.z;

		for (int i = 0; i < Row-1; i++) {
			x -= distx;
			y -= disty;
			z -= distz;
			Vector3 tmp = new Vector3(x,y,z);
			KeyPos.Insert(Pos + i, tmp);
		}
	}

	Vector3 KeyDistance (){
		float x = (KeyPos[0].x - KeyPos[1].x) * 2;
		if (x < 0)
			x *= -1;
		float y = KeyPos [0].y;
		if (y < 0)
			y *= -1;
		float z = (KeyPos[0].z - KeyPos[12].z) * 2;
		if (z < 0)
			z *= -1;
		Vector3 tmp = new Vector3(x,y,z);
		return tmp;
	}

	void Exit(){
		AsteroidKeyboard.Exit ();
		Application.LoadLevel(1);
	}

	public void MainMenu(){
		AsteroidKeyboard.Exit ();
		AudioSource.PlayClipAtPoint (correct, Vector3.zero, 1.0f);
		Application.LoadLevel (1);
	}
}



