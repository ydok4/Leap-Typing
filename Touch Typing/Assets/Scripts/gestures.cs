using UnityEngine;
using System;
using System.Collections.Generic;
using Leap;

public class gestures : MonoBehaviour {
	
	/*Accessing HandController script*/
	public HandController controller;
	
	/*Accessing the leap device*/
	Controller leap_controller;
	
	/*Inialises the starting condition*/
	void Start(){
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected () == true) {
			Debug.Log("Gestures Start");
			leap_controller = CurrentLeap ();
			leap_controller.EnableGesture (Gesture.GestureType.TYPE_KEY_TAP);
			leap_controller.Config.SetFloat ("Gesture.KeyTap.MinDownVelocity", 40.0f);
			leap_controller.Config.SetFloat ("Gesture.KeyTap.HistorySeconds", .2f);
			leap_controller.Config.SetFloat ("Gesture.KeyTap.MinDistance", 1.0f);
			leap_controller.Config.Save ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public int KeyTap(string key){
		key = key.ToLower ();
		Frame frame = leap_controller.Frame();
		int score = 0;
		string direction;
		Debug.Log (key);
		foreach (Gesture gesture in frame.Gestures()) {
			foreach (Hand hand in frame.Hands){
				if(hand.IsLeft)
					direction = "left";
				else
					direction = "right";

				KeyTapGesture keytap = new KeyTapGesture (gesture);
				Pointable point = keytap.Pointable;
				Finger finger = new Finger (point);

				Finger.FingerType fingerType = finger.Type;
				
				Debug.Log ("Finger: " + finger.Type);
				
				switch (fingerType) {
				case Finger.FingerType.TYPE_INDEX:
					Debug.Log ("Index");  
					score = CheckIndex(direction, key);
					break;
				case Finger.FingerType.TYPE_MIDDLE:
					Debug.Log ("Middle");  
					score = CheckMiddle(direction, key);
					break;
				case Finger.FingerType.TYPE_RING:
					Debug.Log ("Ring");  
					score = CheckRing(direction, key);
					break;
				case Finger.FingerType.TYPE_PINKY:
					Debug.Log ("Pinky");  
					score = CheckPinky(direction, key);
					break;
					
				}
			}
		}
		Debug.Log ("score :" + score);
		return score;
	}

	int CheckIndex(string direction, string key){
		if (direction == "left") {
			if(key == "r" || key == "t" || key == "f" || key == "g" || key == "v" || key == "b")
				return 2;
		} else {
			if(key == "y" || key == "u" || key == "h" || key == "j" || key == "n" || key == "m")
				return 2;
		}
		return 1;
	}

	int CheckMiddle(string direction, string key){
		if (direction == "left") {
			if(key == "e" || key == "d" || key == "c")
				return 2;
		} else {
			if(key == "i" || key == "k" || key == "," || key == "<")
				return 2;
		}
		return 1;
	}

	int CheckRing(string direction, string key){
		if (direction == "left") {
			if(key == "w" || key == "s" || key == "x")
				return 2;
		} else {
			if(key == "o" || key == "l" || key == "." || key == ">")
				return 2;
		}
		return 1;
	}

	int CheckPinky(string direction, string key){
		if (direction == "left") {
			if(key == "q" || key == "a" || key == "z")
				return 2;
		} else {
			if(key == "p" || key == ";" || key == ":" || key == "/" || key == "?" || key == "[" || key == "{" || key == "]" || key == "}" || key == "'" || key == "\"")
				return 2;
		}
		return 1;
	}

	//Returns the leap controller from HandController 
	Controller CurrentLeap(){
		GameObject go = GameObject.Find ("GestureController");
		HandController speedController = go.GetComponent <HandController> ();
		Controller leap_controller = speedController.leap_controller_;
		return leap_controller;
	} 

}



