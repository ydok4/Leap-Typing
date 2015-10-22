using UnityEngine;
using System;
using System.Collections.Generic;
using Leap;

public class FingerPosition : MonoBehaviour {
	
	/*Accessing HandController script*/
	HandController controller;
	
	/*Accessing the leap device*/
	Controller leap_controller;
	public static int[] FingerStat = new int[16];

	/*Inialises the starting condition*/
	void Start(){
		for(int i = 0; i < 16; i++)
			FingerStat[i] = 0;
		CurrentLeap();
	}
	
	//Update is called once per frame
	void Update () {
	}

	/*public static FingerStat[] GetFingerStat() {
		return FingerStat;
	}*/

	//Returns the score based on what finger was used to press the key
	public int IsPressed(string key){
		int score = 2;
		int finger = -1;
		Vector3 location;
		key = key.ToLower();
		Debug.Log ("Key " + key);

		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected () == true) {
			switch (key) {
				case "q":
					location = CheckFinger ("TYPE_PINKY", "left");
					score = Compare (location, 0);
					finger = 0;
					break;
				case "a":
					location = CheckFinger ("TYPE_PINKY", "left");
					score = Compare (location, 12);
					finger = 0;
					break;
				case "z":
					location = CheckFinger ("TYPE_PINKY", "left");
					score = Compare (location, 23);
					finger = 0;
					break;
				case "w":
					location = CheckFinger ("TYPE_RING", "left");
					score = Compare (location, 1);
					finger = 2;
					break;
				case "s":
					location = CheckFinger ("TYPE_RING", "left");
					score = Compare (location, 13);
					finger = 2;
					break;
				case "x":
					location = CheckFinger ("TYPE_RING", "left");
					score = Compare (location, 24);
					finger = 2;
					break;
				case "e":
					location = CheckFinger ("TYPE_MIDDLE", "left");
					score = Compare (location, 2);
					finger = 4;
					break;
				case "d":
					location = CheckFinger ("TYPE_MIDDLE", "left");
					score = Compare (location, 14);
					finger = 4;
					break;
				case "c":
					location = CheckFinger ("TYPE_MIDDLE", "left");
					score = Compare (location, 25);
					finger = 4;
					break;
				case "r":
					location = CheckFinger ("TYPE_INDEX", "left");
					score = Compare (location, 3);
			    	finger = 6;
					break;
				case "f":
					location = CheckFinger ("TYPE_INDEX", "left");
					score = Compare (location, 15);
					finger = 6;
					break;
				case "v":
					location = CheckFinger ("TYPE_INDEX", "left");
					score = Compare (location, 26);
					finger = 6;
					break;
				case "t":
					location = CheckFinger ("TYPE_INDEX", "left");
					score = Compare (location, 4);
					finger = 6;
					break;
				case "g":
					location = CheckFinger ("TYPE_INDEX", "left");
					score = Compare (location, 16);
					finger = 6;
					break;
				case "b":
					location = CheckFinger ("TYPE_INDEX", "left");
					score = Compare (location, 27);
					finger = 6;
					break;
				case "y":
					location = CheckFinger ("TYPE_INDEX", "right");
					score = Compare (location, 5);
					finger = 8;
					break;
				case "h":
					location = CheckFinger ("TYPE_INDEX", "right");
					score = Compare (location, 17);
					finger = 8;
					break;
				case "n":
					location = CheckFinger ("TYPE_INDEX", "right");
					score = Compare (location, 28);
					finger = 8;
					break;
				case "u":
					location = CheckFinger ("TYPE_INDEX", "right");
					score = Compare (location, 6);
					finger = 8;
					break;
				case "j":
					location = CheckFinger ("TYPE_INDEX", "right");
					score = Compare (location, 18);
					finger = 8;
					break;
				case "m":
					location = CheckFinger ("TYPE_INDEX", "right");
					score = Compare (location, 29);
					finger = 8;
					break;
				case "i":
					location = CheckFinger ("TYPE_MIDDLE", "right");
					score = Compare (location, 7);
					finger = 10;
					break;
				case "k":
					location = CheckFinger ("TYPE_MIDDLE", "right");
					score = Compare (location, 19);
					finger = 10;
					break;
				case ",":
					location = CheckFinger ("TYPE_MIDDLE", "right");
					score = Compare (location, 30);
					finger = 10;
					break;
				case "<":
					location = CheckFinger ("TYPE_MIDDLE", "right");
					score = Compare (location, 30);
					finger = 10;
					break;
				case "o":
					location = CheckFinger ("TYPE_RING", "right");
					score = Compare (location, 8);
					finger = 12;
					break;
				case "l":
					location = CheckFinger ("TYPE_RING", "right");
					score = Compare (location, 20);
					finger = 12;
					break;
				case ".":
					location = CheckFinger ("TYPE_RING", "right");
					score = Compare (location, 31);
					finger = 12;
					break;
				case ">":
					location = CheckFinger ("TYPE_RING", "right");
					score = Compare (location, 31);
					finger = 12;
					break;
				case "p":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 9);
					finger = 14;
					break;
				case ";":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 21);
					finger = 14;
					break;
				case "/":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 32);
					finger = 14;
					break;
				case "[":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 10);
					finger = 14;
					break;
				case "]":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 11);
					finger = 14;
					break;
				case "'":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 22);
					finger = 14;	
					break;
				case ":":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 21);
					finger = 14;
					break;
				case "?":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 32);
					finger = 14;
					break;
				case "{":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 10);
					finger = 14;
					break;
				case "}":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 11);
					finger = 14;
					break;
				case "\"":
					location = CheckFinger ("TYPE_PINKY", "right");
					score = Compare (location, 22);
					finger = 14;
					break;
			}
		}
		if (score == 2 && finger > -1) {
			FingerStat[finger]++;
		}
		FingerStat[finger+1]++;
		return score;
 	}

	//pass in the finger type and the direction i.e. left or right
	Vector3 CheckFinger(string FingerType, string direction){
		Vector3 location = new Vector3(0,0,0);
		CurrentLeap ();
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
		if (finger.x == 0 && finger.y == 0 && finger.z == 0) {
			Debug.Log ("Out of frame");
			return 1;
		}
		Vector3 KeyPos = calibration.KeyPos[index];
		Vector3 KeyDist = calibration.KeyDist;
		float x = Math.Abs(finger.x - KeyPos.x);
		float y = Math.Abs(finger.y - KeyPos.y);
		float z = Math.Abs(finger.z - KeyPos.z);
		//Testing****************************************
		Vector3 ad = new Vector3 (x,y,z);
		Debug.Log("KeyPos " + KeyPos);
		Debug.Log ("Finger " + finger);
		Debug.Log("Min Distance " + KeyDist);
		Debug.Log ("Actual Distance " + ad);
		//Testing****************************************
		if (x  < KeyDist.x && y < KeyDist.y && z < KeyDist.z)
			return 2;

		return 1;
	}

	//Returns the leap controller from HandController 
	void CurrentLeap(){
		GameObject go = GameObject.Find ("LeapTracker");
		HandController speedController = go.GetComponent <HandController> ();
		leap_controller = speedController.leap_controller_;
	} 
}




