using UnityEngine;
using System.Collections;
//From: http://forum.unity3d.com/threads/looking-with-the-mouse.109250/
public class cameraScript : MonoBehaviour {
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationY = 0F;

	//Mystuff

	public Transform target;
	public Camera camera1;
	// Use this for initialization
	void Start () {
		camera1 = this.GetComponent<Camera> ();
		float height = 2f * camera1.orthographicSize;
		float width = height * camera1.aspect;
		Debug.Log ("Height: " + height + " Width: " + width);
		//Instantiate (Transform, camera1.transform);

	}
	
	// Update is called once per frame
	void Update () {
		//Comment out this section to disable first person camera
		/*
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}*/
		//Vector3 screenPos = camera1.WorldToScreenPoint(target.position);
		//Debug.Log("target is " + screenPos.x + " pixels from the left");
	}
}
