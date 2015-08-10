using UnityEngine;
using System.Collections;

public class projectileScript : MonoBehaviour {
	public string target;
	string spawn;
	public Vector3 start;
	
	// Use this for initialization
	void Start () {
		//Sets target to lower case version so it comes out the right place
		spawn = target.ToLower ();

		//Sets up naming conventions, starting location and rigidbodys and colliders
		gameObject.transform.position=Camera.main.transform.position;

		gameObject.name = "Projectile :" + target;

		gameObject.AddComponent<BoxCollider> ();
		gameObject.AddComponent<Rigidbody> ();
		gameObject.GetComponent<Rigidbody> ().useGravity = false;
		gameObject.GetComponent<BoxCollider> ().isTrigger=true;



		//Creates a second gameobject parented to the other which controls the projectile meshes visuals
		GameObject objectMesh=new GameObject();

		objectMesh.transform.parent = gameObject.transform;
		objectMesh.name = "Projectile " + target + " Mesh";
		objectMesh.AddComponent<MeshFilter> ();
		objectMesh.AddComponent<MeshRenderer> ();
		objectMesh.GetComponent<MeshFilter> ().mesh=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().projectileMesh;
		objectMesh.GetComponent<MeshRenderer> ().material = projectileMaterial (spawn);
		objectMesh.transform.rotation = Quaternion.Euler(0, 90, 90);

		//Changes the scale of the mesh it matches better
		objectMesh .transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		objectMesh.transform.position = gameObject.transform.position;

		//Changes X spawn point depending on what the target is. This is also where you would change the colour of the materials
		if (controllerScript.mode == 0) {
			if (spawn == "q" || spawn == "a" || spawn == "z") {
				gameObject.transform.position = GameObject.Find ("shipGunL4").transform.position;
			} else if (spawn == "w" || spawn == "s" || spawn == "x") {
				gameObject.transform.position = GameObject.Find ("shipGunL3").transform.position;
			} else if (spawn == "e" || spawn == "d" || spawn == "c") {
				gameObject.transform.position = GameObject.Find ("shipGunL2").transform.position;
			} else if (spawn == "r" || spawn == "f" || spawn == "v" || spawn == "t" || spawn == "g" || spawn == "b") {
				gameObject.transform.position = GameObject.Find ("shipGunL1").transform.position;
			} else if (spawn == "y" || spawn == "h" || spawn == "n" || spawn == "u" || spawn == "j" || spawn == "m") {
				gameObject.transform.position = GameObject.Find ("shipGunR1").transform.position;
			} else if (spawn == "i" || spawn == "k" || spawn == "," || spawn == "<") {
				gameObject.transform.position = GameObject.Find ("shipGunR2").transform.position;
			} else if (spawn == "o" || spawn == "l" || spawn == "." || spawn == ">") {
				gameObject.transform.position = GameObject.Find ("shipGunR3").transform.position;
			} else if (spawn == "p" || spawn == ";" || spawn == "slash" || spawn == "[" || spawn == "'" || spawn == "]" || spawn == "{" || spawn == "}" || spawn == ":" || spawn == "\"" || spawn == "/" || spawn == "?") {
				gameObject.transform.position = GameObject.Find ("shipGunR4").transform.position;
			}
		}
		start = gameObject.transform.position;


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
	Material projectileMaterial(string val)
	{
		val = val.ToLower ();
		if (val == "q" || val == "a" || val == "z") {
			//Debug.Log ("In RED");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialRed;
		}
		else if (val == "w" || val == "s" || val == "x") {
			//Debug.Log ("In ORANGE");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialOrange;
		}
		else if (val == "e" || val == "d" || val == "c") {
			//Debug.Log ("In YELLOW");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialYellow;
		}
		else if (val == "r" || val == "f" || val == "v" || val == "t" || val == "g" || val == "b") {
			//Debug.Log ("In PURPLE");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialPurple;
		}
		else if (val == "y" || val == "h" || val == "n" || val == "u" || val == "j" || val == "m") {
			//Debug.Log ("In BLUE");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialBlue;
		}
		else if (val == "i" || val == "k" || val == "," || val == "<") {
			//Debug.Log ("In CYAN");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialCyan;
		}
		else if (val == "o" || val == "l" || val == "." || val == ">") {
			//Debug.Log ("In GREEN");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialGreen;
		}
		else if (val == "p" || val == ";" || val == "[" || val == "\'" || val == "]" || val == "{" || val == "}" || val == ":" || val == "\"" || val == "/" || val == "?") {
			//Debug.Log ("In BROWN");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterialBrown;
		}
		//Debug.Log ("In DEFAULT");
		return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterial;
	}
}
