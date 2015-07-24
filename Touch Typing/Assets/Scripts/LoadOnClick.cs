using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	
	//public GameObject loadingImage;
	void start(){
		//GameObject.Find ("SubMenu1").GetComponentInChildren<Canvas>().enabled = false;
	}

	public void LoadScene(int level)
	{
		switch(level)
		{
			case 0:	//Calibrate
				Application.LoadLevel(0);
				break;
			case 1:	//Back to Main Menu
				Application.LoadLevel(1);
				break;
			case 2:	//All rows
				controllerScript.rowToUse = 3;
				controllerScript.capitalChance = 15;
				controllerScript.goal = 57;
				controllerScript.mode = 0;
				Application.LoadLevel(2);
				break;
			case 3: //Top Row
				controllerScript.rowToUse = 0;
				controllerScript.capitalChance = 15;
				controllerScript.goal = 57;
				controllerScript.mode = 0;
				Application.LoadLevel(2);
				break;
			case 4:	//Middle Row
				controllerScript.rowToUse = 1;
				controllerScript.capitalChance = 15;
				controllerScript.goal = 57;
				controllerScript.mode = 0;
				Application.LoadLevel(2);
				break;
			case 5:	//Bottom Row
				controllerScript.rowToUse = 2;
				controllerScript.capitalChance = 15;
				controllerScript.goal = 57;
				controllerScript.mode = 0;
				Application.LoadLevel(2);
				break;
		}
		//loadingImage.SetActive(true);
		//Application.LoadLevel(level);
	}
}
