using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdater : MonoBehaviour {
	
	public Text scoreText, missedText, timeText;

	// Update is called once per frame
	void Update () {
		if(scoreText!=null)
			scoreText.text = "Score: " +  GameObject.Find ("Main Camera").GetComponent<controllerScript> ().score;
		if(missedText!=null)
			missedText.text = "Missed: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed;
		if (timeText != null) {
			timeText.text = "Time: " + GameObject.Find ("Main Camera").GetComponent<controllerScript> ().time.ToString ("F2");
			if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().time < 0)
				timeText.text = "Time: 0";
		}
		if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().paused == true)
			this.GetComponent<CanvasGroup>().alpha = 1f;
		else
			this.GetComponent<CanvasGroup>().alpha = 0f;
	}
}
