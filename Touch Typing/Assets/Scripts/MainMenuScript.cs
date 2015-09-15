using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	//variables
	/*public static bool BCMode, timeOFF, capOFF;
	public int row;//, mode;
	public static float inputtedTime;*/
	public Text gameTimeText, spawnRateText, capitalChanceText;

	GameObject capSlider, capText, capVText;
	GameObject timeSlider, timeText, timeVText;
	
	void Start(){
		//mode = -1;
		GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode = false;
		GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().row = 3;
		ReservoirScript.inputtedTime = 60;
		GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().timeOFF = false;
		GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().capOFF = false;
		//sub menu's invisible on start
		GameObject.Find ("ModeMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("RowMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("InputMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("BootCampMenu").GetComponentInChildren<Canvas>().enabled = false;

		capSlider = GameObject.Find ("CapitalChanceSlider");
		capText = GameObject.Find ("CapitalChanceText");
		capVText = GameObject.Find ("CapitalChanceValueText");

		timeSlider = GameObject.Find ("TimeSlider");
		timeText = GameObject.Find ("GameTimeText");
		timeVText = GameObject.Find ("GameTimeValueText");
	
	}
	
	void Update()
	{
		if(gameTimeText!=null)
			gameTimeText.text = GameObject.Find ("TimeSlider").GetComponent<Slider> ().value + "m";
		if(spawnRateText!=null)
			spawnRateText.text = GameObject.Find ("SpawnRateSlider").GetComponent<Slider> ().value + "/s";
		if(capitalChanceText!=null)
			capitalChanceText.text = (5 * (int)GameObject.Find ("CapitalChanceSlider").GetComponent<Slider> ().value) + "%";
	}

	public void MainMenu (int button)
	{
		switch (button) {
			case 0:	//Keyboard Mode levels
					GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode = false;
					toggleMenu ("ModeMenu");
				break;
			case 1:	//Tap Mode
					GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().mode = 1;
					GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode = false;
					showInputMenu ();
				break;
			case 2://Calibrate
					Application.LoadLevel(0);
				break;
			case 3://Boot Camp
					GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode = true;
					toggleMenu ("BootCampMenu");
				break;
		}
	}

	public void modeMenu(int button)
	{
		switch (button) {
		case 0:	//Letters
				GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().mode = 0;
				toggleMenu ("RowMenu");
			break;
		case 1:	//Words
				GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().mode = 2;
				showInputMenu ();
			break;
		case 2: //back
				toggleMenu ("ModeMenu");
			break;
		}
	}

	public void RowMenu(int button)
	{
		if (button == -1) {	//Back to Main Menu
			toggleMenu ("RowMenu");
		}else {
			GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().row = button;
			//Debug.Log ("row=" + GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().row);
			
			GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().mode = 0;
			showInputMenu ();
		}
	}

	public void bootCampMenu(int button)
	{
		if (button == 0) {
			//Back Button
			toggleMenu ("BootCampMenu");
		} else {
			if(button==1)//letter
				toggleMenu ("ModeMenu");
			if(button==2)//Tap mode
			{
				GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().mode = 1;
				showInputMenu();
			}
		}
	}

	public void showInputMenu ()
	{
		//toggle menu ON
		toggleMenu ("InputMenu");

		//toggle individual sliders OFF
		if (GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode == true && GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().timeOFF == false) 
		{
			GameObject.Find ("TimeSlider").SetActive(false);
			GameObject.Find ("GameTimeText").SetActive(false);
			GameObject.Find ("GameTimeValueText").SetActive (false);
			GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().timeOFF = true;
		}
		if(GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode == false)
		{
			timeSlider.SetActive(true);
			timeText.SetActive(true);
			timeVText.SetActive(true);
		}
		switch (GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().mode) {
			case 0:
			case 1: //capitals on for letters or tap
					//Debug.Log("CapON");
					capSlider.SetActive(true);
					capText.SetActive(true);
					capVText.SetActive(true);
					GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().capOFF = false;
				break;
			case 2://capitals off for words
					if(GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().capOFF == false)
					{
						//Debug.Log("CapOFF");
						GameObject.Find ("CapitalChanceSlider").SetActive(false);
						GameObject.Find ("CapitalChanceText").SetActive(false);
						GameObject.Find ("CapitalChanceValueText").SetActive (false);
						GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().capOFF = true;
					}
				break;
		}
	}

	public void loadLevel ()
	{
		//Debug.Log("loadLevel "+ mode);
		if (GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode == true)
			controllerScript.infinite = true;
		else
			controllerScript.infinite = false;

		controllerScript.rowToUse = GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().row;
		controllerScript.mode = GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().mode;

		//set time
		if (timeSlider.activeSelf == true)
			ReservoirScript.inputtedTime = 60 * GameObject.Find ("TimeSlider").GetComponent<Slider> ().value;
		else 
			ReservoirScript.inputtedTime = 60;

		HUDUpdater.timePlayed = ReservoirScript.inputtedTime;

		//set capital chance
		if (capSlider.activeSelf == true)
			controllerScript.capitalChance = 5 * (int)GameObject.Find ("CapitalChanceSlider").GetComponent<Slider> ().value;
		else
			controllerScript.capitalChance = 25;

		//set spawn rate
		if (GameObject.Find ("SpawnRateSlider").activeSelf == true) 
		{
			if (GameObject.Find ("Reservoir").GetComponent<ReservoirScript>().BCMode == true)
				controllerScript.goal = 10000 - GameObject.Find ("SpawnRateSlider").GetComponent<Slider> ().value;
			else
				controllerScript.goal = ReservoirScript.inputtedTime - GameObject.Find ("SpawnRateSlider").GetComponent<Slider> ().value;

		}

		Application.LoadLevel(2);
	}
	

	public void toggleMenu(string menu)
	{
		if (GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled == false)
			GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled = true;
		else 
			GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled = false;
	}
}
