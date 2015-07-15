using UnityEngine;
using System;
using System.Collections.Generic;
using Leap;

public class FingerPosition : MonoBehaviour {
	
	/*Accessing HandController script*/
	public HandController controller;
	
	/*Accessing the leap device*/
	Controller leap_controller;
	
	/*Inialises the starting condition*/
	void Start(){
		CurrentLeap();
	}
	
	//Update is called once per frame
	void Update () {
	}

	//Returns the score based on what finger was used to press the key
	public int IsPressed(string key){
		int score = 0;
		Vector3 location;
		key.ToLower();

		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected () == true) {
			switch (key) {
			case "q":
				location = CheckFinger ("TYPE_PINKY", "left");
				score = Compare (location, 0);
				break;
			case "a":
				location = CheckFinger ("TYPE_PINKY", "left");
				score = Compare (location, 12);
				break;
			case "z":
				location = CheckFinger ("TYPE_PINKY", "left");
				score = Compare (location, 23);
				break;
			case "w":
				location = CheckFinger ("TYPE_RING", "left");
				score = Compare (location, 1);
				break;
			case "s":
				location = CheckFinger ("TYPE_RING", "left");
				score = Compare (location, 13);
				break;
			case "x":
				location = CheckFinger ("TYPE_RING", "left");
				score = Compare (location, 24);
				break;
			case "e":
				location = CheckFinger ("TYPE_MIDDLE", "left");
				score = Compare (location, 2);
				break;
			case "d":
				location = CheckFinger ("TYPE_MIDDLE", "left");
				score = Compare (location, 14);
				break;
			case "c":
				location = CheckFinger ("TYPE_MIDDLE", "left");
				score = Compare (location, 25);
				break;
			case "r":
				location = CheckFinger ("TYPE_INDEX", "left");
				score = Compare (location, 3);
				break;
			case "f":
				location = CheckFinger ("TYPE_INDEX", "left");
				score = Compare (location, 15);
				break;
			case "v":
				location = CheckFinger ("TYPE_INDEX", "left");
				score = Compare (location, 26);
				break;
			case "t":
				location = CheckFinger ("TYPE_INDEX", "left");
				score = Compare (location, 4);
				break;
			case "g":
				location = CheckFinger ("TYPE_INDEX", "left");
				score = Compare (location, 16);
				break;
			case "b":
				location = CheckFinger ("TYPE_INDEX", "left");
				score = Compare (location, 27);
				break;
			case "y":
				location = CheckFinger ("TYPE_INDEX", "right");
				score = Compare (location, 5);
				break;
			case "h":
				location = CheckFinger ("TYPE_INDEX", "right");
				score = Compare (location, 17);
				break;
			case "n":
				location = CheckFinger ("TYPE_INDEX", "right");
				score = Compare (location, 28);
				break;
			case "u":
				location = CheckFinger ("TYPE_INDEX", "right");
				score = Compare (location, 6);
				break;
			case "j":
				location = CheckFinger ("TYPE_INDEX", "right");
				score = Compare (location, 18);
				break;
			case "m":
				location = CheckFinger ("TYPE_INDEX", "right");
				score = Compare (location, 29);
				break;
			case "i":
				location = CheckFinger ("TYPE_MIDDLE", "right");
				score = Compare (location, 7);
				break;
			case "k":
				location = CheckFinger ("TYPE_MIDDLE", "right");
				score = Compare (location, 19);
				break;
			case ",":
				location = CheckFinger ("TYPE_MIDDLE", "right");
				score = Compare (location, 30);
				break;
			case "<":
				location = CheckFinger ("TYPE_MIDDLE", "right");
				score = Compare (location, 30);
				break;
			case "o":
				location = CheckFinger ("TYPE_RING", "right");
				score = Compare (location, 8);
				break;
			case "l":
				location = CheckFinger ("TYPE_RING", "right");
				score = Compare (location, 20);
				break;
			case ".":
				location = CheckFinger ("TYPE_RING", "right");
				score = Compare (location, 31);
				break;
			case ">":
				location = CheckFinger ("TYPE_RING", "right");
				score = Compare (location, 31);
				break;
			case "p":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 9);
				break;
			case ";":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 21);
				break;
			case "/":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 32);
				break;
			case "[":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 10);
				break;
			case "]":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 11);
				break;
			case "'":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 22);
				break;
			case ":":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 21);
				break;
			case "?":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 32);
				break;
			case "{":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 10);
				break;
			case "}":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 11);
				break;
			case "\"":
				location = CheckFinger ("TYPE_PINKY", "right");
				score = Compare (location, 22);
				break;
			}
		}
		return score;
 	}

	//pass in the finger type and the direction i.e. left or right
	Vector3 CheckFinger(string FingerType, string direction){
		Vector3 location = new Vector3(0,0,0);
		Frame frame = leap_controller.Frame();
		foreach (Hand hand in frame.Hands) {
			if(HandType(hand, direction)){
				foreach (Finger finger in hand.Fingers) {
					if(finger.Type.ToString() == FingerType){
						location = new Vector3(finger.TipPosition.x,finger.TipPosition.y,finger.TipPosition.z);
					}
				}
			}
		}
		return location;
	}

	//return true if the direction is correct .i.e left or right hand
	bool HandType(Hand hand, string direction){
		if (direction == "left") {
			if(hand.IsLeft){
				return true;
			}
			return false;
		}
		if(hand.IsRight){
			return true;
		}
		return false;
	}

	//return the score depending on how close the finger is to the key
	public static int Compare(Vector3 finger, int index){
		int score = 0;
	
		/*flaot x = Math.Abs(finger.x - calibration.KeyPos[index].x);
		flaot y = Math.Abs(finger.y - calibration.KeyPos[index].y);
		flaot z = Math.Abs(finger.z - calibration.KeyPos[index].z);

		if (x < KeyDist.x)
			score = 3;
		if (x > KeyDist.x*/

		//calibration.KeyPos;
		//calibration.KeyDist;
	 
		return score;
	}

	//Returns the leap controller from HandController 
	void CurrentLeap(){
		GameObject go = GameObject.Find ("name of the object that has the handcontroller in it, should be the same one you put this into");
		HandController speedController = go.GetComponent <HandController> ();
		leap_controller = speedController.leap_controller_;
	} 
}




