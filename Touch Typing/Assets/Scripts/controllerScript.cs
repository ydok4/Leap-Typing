using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//This is the controller script for a timer based single character pressing typing lesson

public class controllerScript : MonoBehaviour {

	//Camera camera; 
	//Set these two variables inside the editor
	public Mesh characterMesh;
	public Material characterMaterial;

	public Mesh projectileMesh;
	public Material projectileMaterial;
	//The potential alphabet of the lesson
	public string alpha = "abcdefghijklmnopqrstuvwxyz[];',.";
	//"Top keyboard row"
	public string alpha1;
	public string alpha2;
	public string alpha3;
	public float time;
	public float goal; 
	public int missed;
	public int delay;
	//Contains a list of all the currenlty active characters
	public List <characters> characterList=new List<characters>();

	//Set to true to pause game
	public bool paused;

	//Players score
	public int score;

	public class characters {
		public GameObject charObj;
		public int location;
		public string chars;
		
		public characters(int l, string c, Vector3 cameraOut)
		{
			location = l;
			chars = c;
			charObj = new GameObject ();

			charObj.name = c;
			charObj.AddComponent<charScript>();
			charObj.GetComponent<charScript> ().start=cameraOut;
			charObj.GetComponent<charScript> ().val = chars;
			charObj.GetComponent<charScript> ().loc=l;
		}
	};

	// Use this for initialization
	void Start () {
		//Sets up alphabet
		alpha1="qwertyuiop[]";
		alpha2="asdfghjkl;'";
		alpha3="zxcvbnm,.";


		//Sets variables to default
		Reset ();
		//Sets the players model to where the camera is
		GameObject.Find ("Body").GetComponent<Transform> ().position=GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position;
	}
	void Reset()
	{
		//Sets the number of missed variables to zero
		missed = 0;	
		//Time is total time for the lesson
		time = 60;
		//Goal is when the next character will spawn. Need to update the decrement as well.
		goal = 58;
	}
	// Update is called once per frame
	void Update () {
		//Pauses or unpauses the game if escape is pressed
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (paused == false)
				paused = true;
			else
				paused = false;
		}
		//Checks if there is any more time for the game to run or if the game is paused
		if (time > 0 && paused==false) {
			time -= Time.deltaTime;

			if (Input.anyKeyDown) {
				bool found=false;
				foreach(characters c in characterList)
				{
					if(c.chars==Input.inputString)
						found=true;
				}
				if(found==false)
					missed++;
			}


			if ((int)time < goal && characterList.Count!=26) {
				Vector3 v3Pos=new Vector3(0,0,0);
				string c="a";
				int side=0;
				//Picks a random letter from the specified alphabet. Used when just using complete alphabet
				//c = alpha[Random.Range(0,alpha.Length)].ToString ();

				//Selects what row get and put the character on.
				int row=Random.Range (0,3);
				switch(row)
				{
					case 0:  
							
							for(int j=0;j<33;j++)
							{
								bool found=false;
								side=Random.Range(0,alpha1.Length);
								c = alpha1[side].ToString ();
								for(int i=0;i<characterList.Count;i++)
								{
									if(characterList[i].chars==c)
										found=true;
								}
								if(found!=true)
									break;
							}
							if(side<=5)
								v3Pos=new Vector3(-6.0f,2.5f,1.5f);
							else
								v3Pos=new Vector3(8.0f,2.5f,1.5f);
							break;
					case 1: 
							for(int j=0;j<33;j++)
							{
								bool found=false;
								side=Random.Range(0,alpha2.Length);
								c = alpha2[side].ToString ();
								for(int i=0;i<characterList.Count;i++)
								{
									if(characterList[i].chars==c)
										found=true;
								}
								if(found!=true)
									break;
							}
							if(side<=5)
								v3Pos=new Vector3(-6.0f,1.0f,1.5f);
							else
								v3Pos=new Vector3(8.0f,1.0f,1.5f);
								break;
					case 2: 
							for(int j=0;j<22;j++)
							{
								bool found=false;
								side=Random.Range(0,alpha3.Length);
								c = alpha3[side].ToString ();
								for(int i=0;i<characterList.Count;i++)
								{
									if(characterList[i].chars==c)
										found=true;
								}
								if(found!=true)
									break;
							}
							if(side<=4)
								v3Pos=new Vector3(-6.0f,-0.5f,1.5f);
							else
								v3Pos=new Vector3(8.0f,-0.5f,1.5f);
								break;
						default:
							break;
				}
				//Spawns characters outside the camera range. Comment out if not desired behaviour
				//v3Pos = new Vector3(0.857f, 0.857f, 0.0f);
				//v3Pos = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * v3Pos;
				//v3Pos += new Vector3(0.5f, 0.5f, 6.0f);
				//v3Pos = Camera.main.ViewportToWorldPoint(v3Pos);

				//Spawns the new character. v3Pos is where it spawns in the world
				characters newChar=new characters(characterList.Count, c,v3Pos);

				//Adds them to the list which keeps track of all active characters
				characterList.Add(newChar);

				//DECREMENT: should match the interval between the initial time and intial goal.
				goal-=2;
			}
		}
	}
	void OnTriggerEnter(Collider other) {
		//Debug.Log("Collision1");
	}
}