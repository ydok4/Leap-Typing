using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class charScript : MonoBehaviour {
	public string val;
	public int loc;
	GameObject model;
	GameObject text;
	public Vector3 start;
	Font myFont = Resources.Load<Font> ("PoisonHope-Regular");
	// Use this for initialization
	void Start () {
		gameObject.transform.position = GameObject.Find ("Main Camera").GetComponent<controllerScript> ().transform.position;
		gameObject.transform.parent = GameObject.Find ("Main Camera").transform;
		gameObject.transform.localPosition = start;
		//Adds components to character object
		gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();
		gameObject.AddComponent<BoxCollider> ();
		gameObject.GetComponent<BoxCollider> ().isTrigger=true;
		//Sets the object mesh and material. Both are stored as public variables in the controllerScript
		gameObject.GetComponent<MeshFilter> ().mesh=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMesh;
		gameObject.GetComponent<MeshRenderer> ().material=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterial;

		text = new GameObject ();


		text.transform.parent=gameObject.transform;
		//Adds components to text
		text.AddComponent<Canvas>();
		text.AddComponent<Text>();
		text.AddComponent<CanvasScaler> ();

		//Sets up components
		text.transform.localPosition = Vector3.zero;
		Font myFont = Resources.Load<Font> ("PoisonHope-Regular");
		text.GetComponent<Text> ().font = myFont;
		text.GetComponent<Text> ().horizontalOverflow = HorizontalWrapMode.Overflow;
		text.GetComponent<Text> ().verticalOverflow = VerticalWrapMode.Overflow;
		text.GetComponent<Text>().text=val;
		text.GetComponent<Text> ().fontSize = 1;
		text.GetComponent<CanvasScaler> ().dynamicPixelsPerUnit = 60;
		text.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		text.GetComponent<Text> ().name="Character Text";
		text.GetComponent<RectTransform> ().localPosition += new Vector3 (0.0f, 0.0f, 0.501f);
		Vector3 rotate=text.GetComponent<RectTransform> ().localRotation.eulerAngles;
		rotate += new Vector3 (0.0f, 180.0f, 0.0f);
		text.GetComponent<RectTransform> ().localRotation=Quaternion.Euler(rotate);
		text.GetComponent<RectTransform> ().localScale=new Vector3(1,1,1);
		text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1.0f, 1.0f);

		//Makes character look at camera
		gameObject.transform.LookAt(Camera.main.transform.position); 
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (val)) {
			//Debug.Log("Add Score");
			GameObject.Find ("Main Camera").GetComponent<controllerScript> ().score+=1;
			//Debug.Log("Destroy Attempt");
			destroyCharacter();
		}
		//Makes sure the object is looking at the camera
		gameObject.transform.LookAt(Camera.main.transform.position); 
		//Uses the start location to determine what direction to move
		if(start.x<0)
			gameObject.transform.position += -transform.right * Time.deltaTime *2;
		else if(start.x>0)
			gameObject.transform.position += transform.right * Time.deltaTime *2;
		if (gameObject.transform.localPosition.z < 1)
			destroyCharacter ();
		//gameObject.transform.position += transform.forward * Time.deltaTime * 2;

	}
	void OnTriggerEnter(Collider other) {
		Debug.Log("Collision");
		//If the object hits a trigger attached to the camera then it will be destroyed
		if (other.name == "Main Camera") {
			destroyCharacter();

		}
	}
	void destroyCharacter()
	{
		for(int i=0;i<GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.Count;i++)
		{
			if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[i].chars==val)
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.RemoveAt(i);
		}
		Destroy (this.gameObject);
	}
}