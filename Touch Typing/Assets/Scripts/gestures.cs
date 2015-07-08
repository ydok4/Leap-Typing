using UnityEngine;
using System;
using System.Collections.Generic;
using Leap;

public class gestures : MonoBehaviour {
	
	/*Accessing HandController script*/
	public HandController controller;
	
	/*Accessing the leap device*/
	Controller leap_controller;
	
	void Awake(){
		//DontDestroyOnLoad (this.gameObject);
	}
	
	/*Inialises the starting condition*/
	void Start(){
		leap_controller = CurrentLeap();
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = leap_controller.Frame();
		foreach (Hand hand in frame.Hands) {
			/*if(hand.IsRight){
				foreach (Finger finger in hand.Fingers) {
					if(finger.Type.ToString() == "TYPE_INDEX"){
						Debug.Log(finger.TipPosition);
					}
				}
			}*/
			//Debug.Log (hand.PalmPosition);
		}

	}
	
	//Returns the leap controller from HandController 
	Controller CurrentLeap(){
		GameObject go = GameObject.Find ("GestureController");
		HandController speedController = go.GetComponent <HandController> ();
		Controller leap_controller = speedController.leap_controller_;
		return leap_controller;
	} 

}



