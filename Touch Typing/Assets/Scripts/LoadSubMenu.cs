using UnityEngine;
using System.Collections;

public class LoadSubMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("SubMenu1").GetComponentInChildren<Canvas>().enabled = false;
	}

	public void LoadSubMenuOnClick (int button)
	{
		switch (button) {
		case 0:	//Keyboard Mode levels
			if (GameObject.Find ("SubMenu1").GetComponentInChildren<Canvas>().enabled == false)
			    	GameObject.Find ("SubMenu1").GetComponentInChildren<Canvas>().enabled = true;
			    else 
			    	GameObject.Find ("SubMenu1").GetComponentInChildren<Canvas>().enabled = false;
			    break;
			    case 1:	//Tap Mode?
				controllerScript.capitalChance = 15;
				controllerScript.goal = 57;
				controllerScript.mode = 1;
				Application.LoadLevel(2);
			    break;
			    }
			    
			    }
}
