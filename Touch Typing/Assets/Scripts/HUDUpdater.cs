/**
*Filename HUDUpdater.cs
*Description Handles all GUI/HUD for the scene touchTyping (Game)
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdater : MonoBehaviour {
	
	public Text scoreText, missedText, timeText, rightText, leftText, accuracy, asteroids, timeStat, prog, lvl;
    public Text lpinky, lring, lmiddle, lindex, rindex, rmiddle, rring, rpinky;
	public static float timePlayed;
    public string currentPlayerName;
	void Start () {
		//ensure is not visible on start, press F5 to activate
		GameObject.Find ("DebugMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("StatsMenu").GetComponentInChildren<Canvas>().enabled = false;
        GameObject.Find("NameEntry").GetComponentInChildren<Canvas>().enabled = true;
        GameObject.Find("FingerStats").GetComponentInChildren<Canvas>().enabled = false;
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
                if(controllerScript.mode == 1)
                    timeText.text = "Time: " + (GameObject.Find("Main Camera").GetComponent<controllerScript>().time).ToString("F2");
                else
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
        if (GameObject.Find("Main Camera").GetComponent<controllerScript>().paused == true && GameObject.Find("Main Camera").GetComponent<controllerScript>().gameOver == false && ReservoirScript.nameEntered == true)
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
		//if(timeStat!=null)
		//	timeStat.text = "Time: "+timePlayed;
		/*if(asteroids!=null && GameObject.Find("Main Camera").GetComponent<controllerScript>().setMode!=2)
			asteroids.text = "Asteroids: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().totalAsteroids;//WRONG
        else if(asteroids != null && GameObject.Find("Main Camera").GetComponent<controllerScript>().setMode == 2)
            asteroids.text = "Words: " + GameObject.Find("Main Camera").GetComponent<controllerScript>().wordsTyped;*/
       

        //NEW GAME OVER SCREEN STATS
        if (asteroids != null)
            asteroids.text = "Asteroids: " + GameObject.Find("Main Camera").GetComponent<controllerScript>().totalAsteroids;

        if (controllerScript.mode == 1)//tap mode, uses score rather than words typed
        {
            if (accuracy != null)
                accuracy.text = "Accuracy: " + (GameObject.Find("Main Camera").GetComponent<controllerScript>().score / (float)(GameObject.Find("Main Camera").GetComponent<controllerScript>().totalAsteroids + GameObject.Find("Main Camera").GetComponent<controllerScript>().missed)) + "%";//total + missed####
        }
        else
        {
            if (accuracy != null)
                accuracy.text = "Accuracy: " + (GameObject.Find("Main Camera").GetComponent<controllerScript>().wordsTyped / (float)(GameObject.Find("Main Camera").GetComponent<controllerScript>().totalAsteroids + GameObject.Find("Main Camera").GetComponent<controllerScript>().missed)) * 100 + "%";
          }
        //FINGER STATS
        // lpinky, lring, lmiddle, lindex, rindex, rmiddle, rring, rpinky;
        // if (GameObject.Find("Main Camera").GetComponent<controllerScript>().con.LeapConnected() == true)
        // {
            if (lpinky != null)
                lpinky.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[0] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[1];
            if (lring != null)
                lring.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[2] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[3];
            if (lmiddle != null)
                lmiddle.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[4] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[5];
            if (lindex != null)
                lindex.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[6] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[7];
            if (rindex != null)
                rindex.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[8] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[9];
            if (rmiddle != null)
                rmiddle.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[10] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[11];
            if (rring != null)
                rring.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[12] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[13];
            if (rpinky != null)
                rpinky.text = GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[14] + "/" + GameObject.Find("Main Camera").GetComponent<controllerScript>().finger.FingerStat[15];
        // }

    }

    public void nameEntered()
    {
        if (GameObject.Find("InputField").GetComponent<InputField>().text != "")
        {
            currentPlayerName = GameObject.Find("InputField").GetComponent<InputField>().text;
            GameObject.Find("InputField").GetComponent<InputField>().text = "";
            GameObject.Find("NameEntry").GetComponentInChildren<Canvas>().enabled = false;
            GameObject.Find("Main Camera").GetComponent<controllerScript>().paused = false;
            ReservoirScript.nameEntered = true;
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

}