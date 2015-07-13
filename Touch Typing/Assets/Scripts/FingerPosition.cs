using UnityEngine;
using System;
using System.Collections.Generic;
using Leap;

public class FingerPosition : MonoBehaviour {
	
	/*Accessing HandController script*/
	public HandController controller;
	
	/*Accessing the leap device*/
	Controller leap_controller;
	
	/*x,y,z of each key position */
	public List <Vector3> KeyPos =new List<Vector3>();
	
	/*Inialises the starting condition*/
	void Start(){
		CurrentLeap();
		GetKeyPos();
	}
	
	//Update is called once per frame
	void Update () {
	}

	//Returns the score based on what finger was used to press the key
	int test(string key){
		int score = 0;
		Vector3 location;
		key.ToLower();
		//need to ask about z p [ ] ' what fingers press Z pinky or ring?
		if (key == "q" || key == "a" || key == "z")
			location = test2("TYPE_PINKY", "left");
		else if(key == "w" || key == "s" || key == "x")
			location = test2("TYPE_RING", "left"); 
		else if(key == "e" || key == "d" ||  key == "c")
			location = test2("TYPE_MIDDLE", "left"); 
		else if(key == "r" || key == "f" ||key == "v" || key == "t" || key == "g" || key == "b")
			location = test2("TYPE_INDEX", "left");  
		else if(key == "y" || key == "h" ||key == "n" || key == "u" || key == "j" || key == "m")
			location = test2("TYPE_INDEX", "right");  
		else if(key == "i" || key == "k" ||  key == "," ||  key == "<")
			location = test2("TYPE_MIDDLE", "right"); 
		else if(key == "o" || key == "l" ||  key == "." ||  key == ">")
			location = test2("TYPE_RING", "right"); 
		else if(key == "p" || key == ";" ||key == "/" || key == "[" || key == "]" || key == "'"|| key == ":" || key == "?" || key == "{" || key == "}" /*|| key == '"'*/)
			location = test2("TYPE_PINKY", "right");
				
		return score;
 	}

	//pass in the finger type and the direction i.e. left or right
	Vector3 test2(string FingerType, string direction){
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

	//Returns the leap controller from HandController 
	void CurrentLeap(){
		GameObject go = GameObject.Find ("name of the object that has the handcontroller in it, should be the same one you put this into");
		HandController speedController = go.GetComponent <HandController> ();
		leap_controller = speedController.leap_controller_;
	} 

	//Returns the keyboard postions in xyz
	void GetKeyPos(){
		/*
		 * need to get KeyPos from calibration.cs somehow
		 */ 
	}
}




