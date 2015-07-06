using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdater : MonoBehaviour {
	
	public Text scoreText, missedText, timeText;
	void Start () {
		//GameObject.Find ("PauseMenu").GetComponent<Canvas> ().SetActive(false);
	}
	// Update is called once per frame
	void Update () {
		//udate HUD score, time and missed values
		if(scoreText!=null)
			scoreText.text = "Score: " +  GameObject.Find ("Main Camera").GetComponent<controllerScript> ().score;
		if(missedText!=null)
			missedText.text = "Missed: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed;
		if (timeText != null) {
			timeText.text = "Time: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().time.ToString ("F2");
			if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().time < 0)
				timeText.text = "Time: 0";
		}
		//toggle pause menu
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().paused == true)
			GameObject.Find ("PauseMenu").GetComponentInChildren<Canvas>().enabled = true;
		 else 
			GameObject.Find ("PauseMenu").GetComponentInChildren<Canvas>().enabled = false;

	}


}
