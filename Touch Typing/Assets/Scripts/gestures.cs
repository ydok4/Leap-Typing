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
		//if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected () == true) {
		Debug.Log("Gestures Start");
		leap_controller = CurrentLeap ();
		leap_controller.EnableGesture (Gesture.GestureType.TYPE_KEY_TAP);
		leap_controller.Config.SetFloat ("Gesture.KeyTap.MinDownVelocity", 30.0f);
		leap_controller.Config.SetFloat ("Gesture.KeyTap.HistorySeconds", .05f);
		leap_controller.Config.SetFloat ("Gesture.KeyTap.MinDistance", 2.0f);
		leap_controller.Config.Save ();
		//}
	}
	
	// Update is called once per frame
	void Update () {
		leap_controller = CurrentLeap ();
		//KeyTap ("q");
	}
	
	public int KeyTap(string key){
		key = key.ToLower ();
		Frame frame = leap_controller.Frame();
		int score = 0;
		string direction;
		//Debug.Log (key);
		foreach (Gesture gesture in frame.Gestures()) {
			foreach (Hand hand in frame.Hands){
				leap_controller = CurrentLeap();
				
				if(hand.IsLeft)
					direction = "left";
				else
					direction = "right";
				KeyTapGesture keytap = new KeyTapGesture (gesture);
				HandList handsForGesture = gesture.Hands;
				bool leftHand = CheckHands(handsForGesture, "left");
				Pointable point = keytap.Pointable;
				Finger finger = new Finger (point);
				
				Finger.FingerType fingerType = finger.Type;
				
				//Debug.Log ("Finger: " + finger.Type);
				
				Debug.Log (key);
				
				
				switch (fingerType) {
				case Finger.FingerType.TYPE_INDEX:
					Debug.Log (direction + "Index");  
					score = CheckIndex(leftHand, key);
					break;
				case Finger.FingerType.TYPE_MIDDLE:
					Debug.Log (direction + "Middle");  
					score = CheckMiddle(leftHand, key);
					break;
				case Finger.FingerType.TYPE_RING:
					Debug.Log (direction + "Ring");  
					score = CheckRing(leftHand, key);
					break;
				case Finger.FingerType.TYPE_PINKY:
					Debug.Log (direction + "Pinky");  
					score = CheckPinky(leftHand, key);
					break;
					
				}
			}
		}
		leap_controller = CurrentLeap();
		//Debug.Log ("score :" + score);
		return score;
	}
	
	int CheckIndex(bool direction, string key){
		if (direction) {
			if(key == "r" || key == "t" || key == "f" || key == "g" || key == "v" || key == "b")
				return 2;
		} else {
			if(key == "y" || key == "u" || key == "h" || key == "j" || key == "n" || key == "m")
				return 2;
		}
		return 0;
	}
	
	int CheckMiddle(bool direction, string key){
		if (direction) {
			if(key == "e" || key == "d" || key == "c")
				return 2;
		} else {
			if(key == "i" || key == "k" || key == "," || key == "<")
				return 2;
		}
		return 0;
	}
	
	int CheckRing(bool direction, string key){
		if (direction) {
			if(key == "w" || key == "s" || key == "x")
				return 2;
		} else {
			if(key == "o" || key == "l" || key == "." || key == ">")
				return 2;
		}
		return 0;
	}
	
	int CheckPinky(bool direction, string key){
		if (direction) {
			if(key == "q" || key == "a" || key == "z")
				return 2;
		} else {
			if(key == "p" || key == ";" || key == ":" || key == "/" || key == "?" || key == "[" || key == "{" || key == "]" || key == "}" || key == "'" || key == "\"")
				return 2;
		}
		return 0;
	}
	
	//Returns the leap controller from HandController 
	Controller CurrentLeap(){
		GameObject go = GameObject.Find ("GestureController");
		HandController speedController = go.GetComponent <HandController> ();
		Controller leap_controller = speedController.leap_controller_;
		return leap_controller;
	} 
	
	bool CheckHands(HandList hands, string direction)
	{
		foreach (Hand hand in hands) {
			switch(direction)
			{
			case "left":
				if(hand.IsLeft)
				{
					return true;
				}
				break;
			case "right":	
				if(hand.IsRight)
				{
					return true;
				}
				break;
			}
			
		}
		return false;
	}
	
}