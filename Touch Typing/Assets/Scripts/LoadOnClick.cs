using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	private int prevClick, keybOrTap;
	public Text gameTimeText, spawnRateText, capitalChanceText;
	public Text BCSpawnRateText, BCCapitalChanceText;
	public static float inputtedTime;

	void Awake(){
		//sub menu's invisible on start
		GameObject.Find ("SubMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("InputMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("BootCampMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("BCInputMenu").GetComponentInChildren<Canvas>().enabled = false;
		//GameObject.Find ("ModeMenu").GetComponentInChildren<Canvas>().enabled = false;
		//GameObject.Find ("ModeInputMenu").GetComponentInChildren<Canvas>().enabled = false;
	}

	void Update()
	{
		if(gameTimeText!=null)
			gameTimeText.text = GameObject.Find ("TimeSlider").GetComponent<Slider> ().value + " min(s)";
		if(spawnRateText!=null)
			spawnRateText.text = "A letter every " + GameObject.Find ("SpawnRateSlider").GetComponent<Slider> ().value + " second(s)";
		if(BCSpawnRateText!=null)
			BCSpawnRateText.text = "A letter every " + GameObject.Find ("BCSpawnRateSlider").GetComponent<Slider> ().value + " second(s)";
		if(BCCapitalChanceText!=null)
			BCCapitalChanceText.text = (5 * (int)GameObject.Find ("BCCapitalChanceSlider").GetComponent<Slider> ().value) + "%";
		if(capitalChanceText!=null)
			capitalChanceText.text = (5 * (int)GameObject.Find ("CapitalChanceSlider").GetComponent<Slider> ().value) + "%";
	}

	public void MainMenu (int button)
	{
		switch (button) {
			case 0:	//Keyboard Mode levels
				//keybOrTap = button;
				toggleMenu ("ModeMenu");
				break;
			case 1:	//Tap Mode
				keybOrTap = button;
				toggleMenu ("InputMenu");
				break;
			case 2://Calibrate
				Application.LoadLevel(0);
				break;
			case 3://Boot Camp
				toggleMenu ("BootCampMenu");
				break;
		}
	}
	public void SubMenu(int button)
	{
		if (button == 0) {	//Back to Main Menu
			toggleMenu ("SubMenu");
		}else {
			if(button!=1)
				prevClick = button;
			toggleMenu ("InputMenu");
		}
	}

	public void InputMenu()
	{
		if (keybOrTap == 0) //keyboard mode
		{ 
			switch (prevClick) 
			{
				case 0:	
				case 1:	//All rows
					/*controllerScript.rowToUse = 3;
					controllerScript.capitalChance = 15;
					if(GameObject.Find ("InputText").GetComponent<Text>().text == "")
						controllerScript.goal = 57;
					else 
						controllerScript.goal = 60 - int.Parse(GameObject.Find ("InputText").GetComponent<Text>().text);
					controllerScript.mode = 0;
					Application.LoadLevel(2);*/
					loadLevel (3, 0, 2);
					break;
				case 2: //Top Row
					loadLevel (0, 0, 2);
					break;
				case 3:	//Middle Row
					loadLevel (1, 0, 2);
					break;
				case 4:	//Bottom Row
					/*controllerScript.rowToUse = 2;
					controllerScript.capitalChance = 15;
					controllerScript.goal = 57;
					controllerScript.mode = 0;
					Application.LoadLevel(2);*/
					loadLevel (2, 0, 2);
					break;
			}
		} else if (keybOrTap == 1)//tap mode
			loadLevel(3, 1, 2);
	}

	public void modeMenu(int button)
	{
		switch (button) {
			case 0:	//Letters
				keybOrTap = button;
				toggleMenu ("SubMenu");
				break;
			case 1:	//Words
				toggleMenu ("ModeInputMenu");
				break;
			case 2: //back
					toggleMenu ("ModeMenu");
			break;
		}
	}

	public void modeInputMenu(int button)
	{
		switch (button) {
			case 0:	//start
				controllerScript.goal = 10000 - GameObject.Find ("ModeSpawnRateSlider").GetComponent<Slider> ().value;
				controllerScript.rowToUse = 3;
				inputtedTime = 60 * GameObject.Find ("ModeGameTimeSlider").GetComponent<Slider> ().value;
				controllerScript.mode = 2;
				Application.LoadLevel (2);
				break;
			case 1:	//Back
					toggleMenu ("ModeInputMenu");
				break;
		}
	}

	public void bootCampMenu(int button)
	{
		if (button == 0) {
			//Keyboard Mode levels
			toggleMenu ("BootCampMenu");
		} else {
			if(button==1)//keyboard mode
				prevClick = 0;
			if(button==2)//Tap mode
				prevClick = 1;
			toggleMenu ("BCInputMenu");
		}
	}
	public void bootCamptInputMenu(int button)
	{
		if (button == 0)//clicked back button
			toggleMenu ("BCInputMenu");
		else if (button == 1) {//clicked start button
			if (prevClick == 1)//keyboard mode
				controllerScript.mode = 0;
			if (prevClick == 2)//Tap mode
				controllerScript.mode = 1;

			controllerScript.capitalChance = 5 * (int)GameObject.Find ("BCCapitalChanceSlider").GetComponent<Slider> ().value;
			controllerScript.goal = 10000 - GameObject.Find ("BCSpawnRateSlider").GetComponent<Slider> ().value;
			controllerScript.infinite = true;
			controllerScript.rowToUse = 3;
			inputtedTime = 60 * GameObject.Find ("TimeSlider").GetComponent<Slider> ().value;
			Application.LoadLevel (2);
		}
	}
	public void loadLevel(int row, int mode, int level)
	{ 
		controllerScript.rowToUse = row;
		controllerScript.capitalChance = 5 * (int)GameObject.Find ("CapitalChanceSlider").GetComponent<Slider> ().value;
		/*if (GameObject.Find ("InputText").GetComponent<Text> ().text == "") {
			controllerScript.goal = (GameObject.Find ("TimeSlider").GetComponent<Slider> ().value * 60) - 3;
		}
		else 
			controllerScript.goal = 60 - float.Parse(GameObject.Find ("InputText").GetComponent<Text>().text);*/
		controllerScript.goal = (GameObject.Find ("TimeSlider").GetComponent<Slider> ().value * 60) - GameObject.Find ("SpawnRateSlider").GetComponent<Slider> ().value;
		controllerScript.mode = mode;
		inputtedTime = 60 * GameObject.Find ("TimeSlider").GetComponent<Slider> ().value;
		Application.LoadLevel(level);
	}

	public void toggleMenu(string menu)
	{
		if (GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled == false)
			GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled = true;
		else 
			GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("TimeSlider").SetActive(false);
	}
}
