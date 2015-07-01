using UnityEngine;
using System.Collections.Generic;
using Leap;

public class calibration : MonoBehaviour {

	/*Accessing HandController script*/
	public HandController controller;

	/*Accessing the leap device*/
	Controller leap_controller;
	
	/*Input for keyboard calibration*/
	protected readonly string[] Key = { "q", "p", "z", "/"};

	/*Index of Key and KeyPos*/
	protected int index = 0;

	/**/
	protected bool Finish = false;

	/*x,y,z of each key position */
	public List <Vector3> KeyPos =new List<Vector3>();
	
	void Awake(){
		//DontDestroyOnLoad (this.gameObject);
	}

	/*Inialises the starting condition*/
	void Start(){
		leap_controller = CurrentLeap();
	}

	// Update is called once per frame
	void Update () {
		//this doesn't work for some reason
		if(!leap_controller.IsConnected){
			Debug.Log("is Not connected");
		} else{
			Debug.Log("is connected");	
		}

		if (Finish) {
			CalibrateKeyboard ();
			Application.LoadLevel("Main");
		}else if (Input.GetKeyDown (Key [index])) {
			RightIndexPosition ();
			if(index == 3)
				Finish = true;
			else
				index++;
		}
	}

	Controller CurrentLeap(){
		GameObject go = GameObject.Find ("Calibration");
		HandController speedController = go.GetComponent <HandController> ();
		Controller leap_controller = speedController.leap_controller_;
		return leap_controller;
	} 

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

	void CalibrateKeyboard(){
	}
}


