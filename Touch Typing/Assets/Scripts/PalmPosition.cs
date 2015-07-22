using UnityEngine;
using System.Collections;
using Leap;

public class PalmPosition : MonoBehaviour {

	/*Accessing HandController script*/
	public HandController controller;

	/*Vecto3 of right/left palm*/
	public Vector3 right;
	public Vector3 left;

	/*Accessing the leap device*/
	Controller leap_controller;

	// Use this for initialization
	void Start () {
		CurrentLeap();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected () == false) {
			//Debug.Log ("is Not connected");
			//ConnectLeap image
		} else {
			Frame frame = leap_controller.Frame();
			foreach (Hand hand in frame.Hands) {
				if(hand.IsRight){
					right = new Vector3 (hand.PalmPosition.x,hand.PalmPosition.y,hand.PalmPosition.z);
					}
				if(hand.IsLeft){
					left = new Vector3 (hand.PalmPosition.x,hand.PalmPosition.y,hand.PalmPosition.z);
				}
			}
		}
	}
	void CurrentLeap(){
		GameObject go = GameObject.Find ("LeapTracker");
		HandController speedController = go.GetComponent <HandController> ();
		leap_controller = speedController.leap_controller_;
	} 
}
