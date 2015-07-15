using UnityEngine;
using System;
using System.Collections.Generic;
using Leap;

public class calibration : MonoBehaviour {

	/*Accessing HandController script*/
	public HandController controller;

	/*Accessing the leap device*/
	Controller leap_controller;
	
	/*Input for keyboard calibration*/
	protected readonly string[] Key = { "q", "]", "z", "/"};

	/*Index of Key and KeyPos*/
	protected int index = 0;

	/*distance between keys*/
	static Vector3 KeyDist;

	/**/
	protected bool Finish = false;

	/*x,y,z of each key position */
	public List <Vector3> KeyPos =new List<Vector3>();
	
	void Awake(){
		//DontDestroyOnLoad (this.gameObject);
	}

	/*Inialises the starting condition*/
	void Start(){
		CurrentLeap();
	}

	// Update is called once per frame
	void Update () {
		if(!leap_controller.IsConnected){
			Debug.Log("is Not connected");
			//ConnectLeap image
		}
		if (Finish & KeyPos.Count < 5) {
			Calibrate();
			Application.LoadLevel("Main");
		}else if (Input.GetKeyDown (Key [index])) {
			RightIndexPosition ();
			if(index == 3)
				Finish = true;
			else
				index++;
		}
	}

	//Returns the leap controller from HandController 
	void CurrentLeap(){
		GameObject go = GameObject.Find ("Calibration");
		HandController speedController = go.GetComponent <HandController> ();
		leap_controller = speedController.leap_controller_;
	} 

	//Finds and stores the xyz of the right index finger
	void RightIndexPosition(){
		Frame frame = leap_controller.Frame();
		foreach (Hand hand in frame.Hands) {
			if(hand.IsRight){
				foreach (Finger finger in hand.Fingers) {
					if(finger.Type.ToString() == "TYPE_INDEX"){
						Vector3 tmp = new Vector3(finger.TipPosition.x,finger.TipPosition.y,finger.TipPosition.z);
						KeyPos.Add(tmp);
					}
				}
			}
		}
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

		//*****also have to move a and z
		//add ] to the correct position
		//KeyPos.Add(right);
		//KeyPos.RemoveAt(1);

		//add ' to the correct position
		//KeyPos.Add(right);
		//KeyPos.RemoveAt(1);

		//add / to the correct position
		//KeyPos.Add(right);
		//KeyPos.RemoveAt(1);

		//int Num = KeyPos.Count;//this will only work if i remove/add the index 2,3,4,5

		//if (KeyPos.Count < 30)
		//	CalibrateKeyboard (KeyPos[],KeyPos[], Row-1);//change the index of KeyPos to the correct ones
	}
}



