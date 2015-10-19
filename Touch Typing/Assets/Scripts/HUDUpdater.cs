using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdater : MonoBehaviour {
	
	public Text scoreText, missedText, timeText, rightText, leftText, accuracy, asteroids, timeStat;
	public static float timePlayed;
	void Start () {
		//ensure is not visible on start, press F5 to activate
		GameObject.Find ("DebugMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("StatsMenu").GetComponentInChildren<Canvas>().enabled = false;
	}

	void Update () {
		//udate HUD score, time and missed values
		if(scoreText!=null)
			scoreText.text = "Score: " +  GameObject.Find ("Main Camera").GetComponent<controllerScript> ().score;
		if(missedText!=null)
			missedText.text = "Missed: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed;
		if (timeText != null) {
			if(controllerScript.infinite == false){
				timeText.text = "Time: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().time.ToString ("F2");
				if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().time < 0)
					timeText.text = "Time: 0";
			}
			else{
				timeText.text = "Time: ∞";
			}
		}
		//toggle pause menu
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().paused == true && GameObject.Find("Main Camera").GetComponent<controllerScript>().gameOver ==false)
			GameObject.Find ("PauseMenu").GetComponentInChildren<Canvas>().enabled = true;
		 else 
			GameObject.Find ("PauseMenu").GetComponentInChildren<Canvas>().enabled = false;
		
		//toggle debug log
		if (Input.GetKeyDown (KeyCode.F5)) {
			if(GameObject.Find ("DebugMenu").GetComponentInChildren<Canvas>().enabled == true)
				GameObject.Find ("DebugMenu").GetComponentInChildren<Canvas>().enabled = false;
			else
				GameObject.Find ("DebugMenu").GetComponentInChildren<Canvas>().enabled = true;
		}
		if (rightText != null)
			rightText.text = "Right Palm x: " + GameObject.Find ("DebugMenu").GetComponent<PalmPosition>().right.x + "\nRight Palm y: " + GameObject.Find ("DebugMenu").GetComponent<PalmPosition>().right.y + "\nRight Palm z: " + GameObject.Find ("DebugMenu").GetComponent<PalmPosition>().right.z;
		if (leftText != null)
			leftText.text = "Left Palm x: " + GameObject.Find ("DebugMenu").GetComponent<PalmPosition>().left.x + "\nLeft Palm y: " + GameObject.Find ("DebugMenu").GetComponent<PalmPosition>().left.y + "\nLeft Palm z: " + GameObject.Find ("DebugMenu").GetComponent<PalmPosition>().left.z;
		if(timeStat!=null)
			timeStat.text = "Time: "+timePlayed;
		if(asteroids!=null)
			asteroids.text = "Asteroids: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids;
		//if(accuracy!=null)
			//accuracy.text = "Accuracy: " + ((GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids - GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed) / GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids) + "%";
	}


	public void PauseMenu(int button)
	{
		switch (button) {
			case 0: //back to Main Menu
				Application.LoadLevel(1);
				break;
			case 1: //Calibrate
				Application.LoadLevel(0);
				break;
		}
	}

	public void PopulateStatsMenu()
	{
		float t = GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids;
		float m = GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed;
		Debug.Log ("STATS MENU " + timePlayed + " " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids + " t:" + t + " m:" + m);
		/*if(timeStat!=null)
			timeStat.text = "Time: "+timePlayed;
		if(asteroids!=null)
			asteroids.text = "Asteroids: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids;
		*/
			//if(accuracy!=null)
			//accuracy.text = "Accuracy: " + ((GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids - GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed) / GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids) + "%";
	}
}