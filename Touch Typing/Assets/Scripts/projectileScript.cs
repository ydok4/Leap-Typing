using UnityEngine;
using System.Collections;

public class projectileScript : MonoBehaviour {
	public string target;
	// Use this for initialization
	void Start () {
		//Sets up naming conventions, starting location and rigidbodys and colliders
		gameObject.transform.position=Camera.main.transform.position;
		gameObject.name = "Projectile :" + target;
		gameObject.AddComponent<BoxCollider> ();
		gameObject.AddComponent<Rigidbody> ();
		gameObject.GetComponent<Rigidbody> ().useGravity = false;
		gameObject.GetComponent<BoxCollider> ().isTrigger=true;

		//Creates a second gameobject parented to the other which controls the projectile meshes visuals
		GameObject objectMesh=new GameObject();
		objectMesh.transform.position = gameObject.transform.position;
		objectMesh.transform.parent = gameObject.transform;
		objectMesh.name = "Projectile " + target + " Mesh";
		objectMesh.AddComponent<MeshFilter> ();
		objectMesh.AddComponent<MeshRenderer> ();
		objectMesh.GetComponent<MeshFilter> ().mesh=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().projectileMesh;
		objectMesh.GetComponent<MeshRenderer> ().material=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().projectileMaterial;
		objectMesh.transform.rotation = Quaternion.Euler(0, 90, 90);


		//Changes X spawn point depending on what the target is. This is also where you would change the colour of the materials
		if (target == "q" || target == "a" || target == "z") {
			gameObject.transform.position+=new Vector3(-3,0,0);
		}
		else if (target == "w" || target == "s" || target == "x") {
			gameObject.transform.position+=new Vector3(-2,0,0);
		}
		else if (target == "e" || target == "d" || target == "c") {
			gameObject.transform.position+=new Vector3(-1,0,0);
		}
		else if (target == "r" || target == "f" || target == "v" || target == "t" || target == "g" || target == "v") {
			
		}
		else if (target == "y" || target == "h" || target == "n" || target == "u" || target == "j" || target == "m") {
			gameObject.transform.position+=new Vector3(1,0,0);
		}
		else if (target == "i" || target == "k" || target == ",") {
			gameObject.transform.position+=new Vector3(2,0,0);
		}
		else if (target == "o" || target == "l" || target == ".") {
			gameObject.transform.position+=new Vector3(3,0,0);
		}
		else if (target == "p" || target == ";" || target == "/" || target == "[" || target == "'" || target == "]") {
			gameObject.transform.position+=new Vector3(4,0,0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Checks if game is paused
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().paused == false) {
			//Makes sure the object is always looking at the target char and moves towards it
			gameObject.transform.LookAt (GameObject.Find (target).transform.position); 
			gameObject.transform.position += transform.forward * Time.deltaTime * 6;
		}
	}
	void OnTriggerEnter(Collider col)
	{
		//If it collides with the target destroys the object
		if (col.name == target) {
			Destroy (gameObject);
		}
	}
}
