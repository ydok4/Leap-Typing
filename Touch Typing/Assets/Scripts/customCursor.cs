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

		/*Plane XYPlane = new Plane(Vector3.up, Vector3.zero);
		//public static Vector3 GetMousePositionOnXYPlane() {
		float distance;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 hitPoint = new Vector3(0,0,0);
		if(XYPlane.Raycast (ray, out distance)) {
			hitPoint = ray.GetPoint(distance);
			//Just double check to ensure the z position is exactly zero
			hitPoint.z = 0;
			//return hitPoint;
			//}
			//return Vector3.zero;
		}*/

		/*float dist = transform.position.z - Camera.main.transform.position.z;
		targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
		targetPos = Camera.main.ScreenToWorldPoint(targetPos);
		transform.position = Vector3.Lerp (transform.position, targetPos, speed);//* Time.deltaTime*/

		Vector3 temp = Input.mousePosition;
		temp.z = 10f;
		transform.position = Camera.main.ScreenToWorldPoint(temp);

		//transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 0);



	}
}