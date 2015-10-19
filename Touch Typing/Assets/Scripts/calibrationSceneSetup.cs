using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class calibrationSceneSetup : MonoBehaviour {

	GameObject text;
	public Mesh asteroidMesh;
	public Material asteroidMaterial;
	public List <GameObject> characterList=new List<GameObject>();
	// Use this for initialization
	void Start () {
		string alpha = "qwertyuiop[]asdfghjkl;'zxcvbnm,./";
		for (int i=1; i<34; i++) 
		{
			GameObject charObj;
			charObj = new GameObject ();
			charObj.name = alpha[i-1].ToString();
			charObj.AddComponent<MeshFilter> ();
			charObj.AddComponent<MeshRenderer> ();
			charObj.GetComponent<MeshFilter> ().mesh = asteroidMesh;
			charObj.GetComponent<MeshRenderer> ().material = asteroidMaterial;
			if(i<=12)
				charObj.transform.position = new Vector3 (-5.0f + i, 1.5f, 15f);
			else if(i<=23)
				charObj.transform.position = new Vector3 (-4.5f + i-12, 0.0f, 15f);
			else
				charObj.transform.position = new Vector3 (-4.0f + i-23, -1.5f, 15f);
			charObj.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			Vector3 rotate=charObj.transform.localRotation.eulerAngles;
			rotate += new Vector3 (0.0f, 180.0f, 0.0f);
			charObj.transform.localRotation= Quaternion.Euler(rotate);
			text = new GameObject ();
			//ModifyAsteroidRotation - WIP. Issue is text wont orientate correctly
			//Vector3 asRotate = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
			//gameObject.transform.localRotation = Quaternion.Euler (asRotate);
			
			text.transform.parent=charObj.transform;
			//Adds components to text
			text.AddComponent<Canvas>();
			text.AddComponent<Text>();
			text.AddComponent<CanvasScaler> ();
			
			//Sets up components
			text.transform.localPosition = Vector3.zero;
			Font myFont = Resources.Load<Font> ("Neuton-Regular");
			text.GetComponent<Text> ().font = myFont;
			text.GetComponent<Text> ().horizontalOverflow = HorizontalWrapMode.Overflow;
			text.GetComponent<Text> ().verticalOverflow = VerticalWrapMode.Overflow;
			text.GetComponent<Text>().text=charObj.name;
			//text.GetComponent<Text> ().color = new Color (0f, 0f, 0f);	Remove comment to turn text black
			text.GetComponent<Text> ().fontSize = 3;
			text.GetComponent<CanvasScaler> ().dynamicPixelsPerUnit = 80;
			text.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
			text.GetComponent<Text> ().name="Character Text";
			text.GetComponent<RectTransform> ().localPosition += new Vector3 (0.0f, 0.0f, 6.501f);
			rotate=text.GetComponent<RectTransform> ().localRotation.eulerAngles;
			text.GetComponent<RectTransform> ().localRotation=Quaternion.Euler(rotate);
			text.GetComponent<RectTransform> ().localScale=new Vector3(1,1,1);
			text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1.0f, 1.0f);

			charObj.transform.LookAt (Camera.main.transform.position);

			characterList.Add(charObj);

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
