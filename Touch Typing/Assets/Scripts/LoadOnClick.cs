using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	public int prevClick;

	void Awake(){
		//sub menu's invisible on start
		GameObject.Find ("SubMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("InputMenu").GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Find ("BootCampMenu").GetComponentInChildren<Canvas>().enabled = false;
	}

	public void MainMenu (int button)
	{
		switch (button) {
		case 0:	//Keyboard Mode levels
			toggleMenu ("SubMenu");
			break;
		case 1:	//Tap Mode
			controllerScript.capitalChance = 15;
			controllerScript.goal = 57;
			controllerScript.mode = 1;//tap
			Application.LoadLevel (2);
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
			toggleMenu ("InputMenu");
			if(button!=1)
				prevClick = button;
		}
	}

	public void InputMenu()
	{
		switch(prevClick)
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
			loadLevel(3, 15, 0, 2);
			break;
		case 2: //Top Row
			loadLevel(0, 15, 0, 2);
			break;
		case 3:	//Middle Row
			loadLevel(1, 15, 0, 2);
			break;
		case 4:	//Bottom Row
			/*controllerScript.rowToUse = 2;
			controllerScript.capitalChance = 15;
			controllerScript.goal = 57;
			controllerScript.mode = 0;
			Application.LoadLevel(2);*/
			loadLevel(2, 15, 0, 2);
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
				controllerScript.mode = 0;
			if(button==2)//Tap mode
				controllerScript.mode = 1;

			controllerScript.capitalChance = 25;
			controllerScript.goal = 9997;
			controllerScript.infinite = true;
			controllerScript.rowToUse = 3;
			Application.LoadLevel (2);
		}
	}
	
	public void loadLevel(int row, int cap, int mode, int level)
	{ Debug.Log("inLoadLevel"+GameObject.Find ("InputText").GetComponent<Text> ().text);
		controllerScript.rowToUse = row;
		controllerScript.capitalChance = cap;
		if (GameObject.Find ("InputText").GetComponent<Text> ().text == "") {
			controllerScript.goal = 57;
			Debug.Log("in");
		}
		else 
			controllerScript.goal = 60 - float.Parse(GameObject.Find ("InputText").GetComponent<Text>().text);
		controllerScript.mode = mode;
		Application.LoadLevel(level);
	}

	public void toggleMenu(string menu)
	{
		if (GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled == false)
			GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled = true;
		else 
			GameObject.Find (menu).GetComponentInChildren<Canvas>().enabled = false;
	}
}
