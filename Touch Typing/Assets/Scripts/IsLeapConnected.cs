using UnityEngine;
using System.Collections;
using Leap;

public class IsLeapConnected : MonoBehaviour {

	/*Accessing the leap device*/
	Controller leap_controller;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool LeapConnected(){
		CurrentLeap ();
		if (!leap_controller.IsConnected) {
			return false;
		} 
		else 
		{
			return true;
		}
	}

	//Returns the leap controller from HandController 
	void CurrentLeap(){
		GameObject go = GameObject.Find ("LeapTracker");
		HandController speedController = go.GetComponent <HandController> ();
		leap_controller = speedController.leap_controller_;
	} 
}
