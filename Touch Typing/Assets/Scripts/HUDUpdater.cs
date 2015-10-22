using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdater : MonoBehaviour {
	
	public Text scoreText, missedText, timeText, rightText, leftText, accuracy, asteroids, timeStat, prog, lvl;
	public static float timePlayed;
    public string currentPlayerName;
	void Start () {
		//ensure is not visible on start, press F5 to activate
		GameObject.Find ("DebugMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("StatsMenu").GetComponentInChildren<Canvas>().enabled = false;
        GameObject.Find("NameEntry").GetComponentInChildren<Canvas>().enabled = true;
        currentPlayerName = "Default";
	}

	void Update () {
		//udate HUD score, time and missed values
		if(scoreText!=null)
			scoreText.text = "Score: " +  GameObject.Find ("Main Camera").GetComponent<controllerScript> ().score;
		if(missedText!=null)
			missedText.text = "Missed: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed;
		if (timeText != null) {
			if(controllerScript.infinite == false){
				timeText.text = "Time: " + (60f-GameObject.Find ("Main Camera").GetComponent<controllerScript> ().timeIncreasing).ToString ("F2");
				if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().time < 0)
					timeText.text = "Time: 0";
			}
			else{
				timeText.text = "Time: ∞";
			}
		}
        //THIS IS THE NEW SHIT
        //Debug.Log("LEVEL: " + GameObject.Find("Main Camera").GetComponent<controllerScript>().level + " LEVEL LIMIT " + GameObject.Find("Main Camera").GetComponent<controllerScript>().levelLimit);
		if(prog!=null)
			prog.text = "Progress: " + GameObject.Find("Main Camera").GetComponent<controllerScript>().wordsForLevel + "/"+ GameObject.Find ("Main Camera").GetComponent<controllerScript> ().levelLimit;
		if(lvl!=null)
			lvl.text = "Level: " +  (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().level+1);

        if ((controllerScript.infinite == false) && (controllerScript.mode != 1))//arcade mode (not, bc or tap)
            GameObject.Find("Arcade").GetComponentInChildren<Canvas>().enabled = true;
        else
            GameObject.Find("Arcade").GetComponentInChildren<Canvas>().enabled = false;

        //on enter key pressed after entered name, unpause level
        if (GameObject.Find("InputField").GetComponent<InputField>().isFocused && GameObject.Find("InputField").GetComponent<InputField>().text != "" && Input.GetKey(KeyCode.Return))
        {
            nameEntered();
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
		if(asteroids!=null && GameObject.Find("Main Camera").GetComponent<controllerScript>().setMode!=2)
			asteroids.text = "Asteroids: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids;
        else if(asteroids != null && GameObject.Find("Main Camera").GetComponent<controllerScript>().setMode == 2)
            asteroids.text = "Words Typed: " + GameObject.Find("Main Camera").GetComponent<controllerScript>().wordsTyped;
        //if(accuracy!=null)
        //accuracy.text = "Accuracy: " + ((GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids - GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed) / GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids) + "%";
    }

    public void nameEntered()
    {
        if (GameObject.Find("InputField").GetComponent<InputField>().text != "")
        {
            currentPlayerName = GameObject.Find("InputField").GetComponent<InputField>().text;
            GameObject.Find("InputField").GetComponent<InputField>().text = "";
            GameObject.Find("NameEntry").GetComponentInChildren<Canvas>().enabled = false;
            GameObject.Find("Main Camera").GetComponent<controllerScript>().paused = false;
        }
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
		//Debug.Log ("STATS MENU " + timePlayed + " " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids + " t:" + t + " m:" + m);
		/*if(timeStat!=null)
			timeStat.text = "Time: "+timePlayed;
		if(asteroids!=null)
			asteroids.text = "Asteroids: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids;
		*/
			//if(accuracy!=null)
			//accuracy.text = "Accuracy: " + ((GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids - GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed) / GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids) + "%";
	}
}