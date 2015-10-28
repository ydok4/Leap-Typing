/**
*Filename customCursor.cs
*Description Creates a visible mouse for the rift
*/
using UnityEngine;
using System.Collections;

public class customCursor : MonoBehaviour 
{
	public float speed = 3.0f;
	//private Vector3 targetPos;
	
	void Start() {
		//targetPos = transform.position;    
	}
	
	void Update () {

		Vector3 temp = Input.mousePosition;
		temp.z = 10f;
		transform.position = Camera.main.ScreenToWorldPoint(temp);

	}
}