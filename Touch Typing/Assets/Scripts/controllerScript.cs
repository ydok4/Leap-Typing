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
	public string alpha;
	
	//"Top keyboard row"
	public string alpha1;
	public string alpha2;
	public string alpha3;
	public float time;
	public float goal; 
	public int missed;
	public float delay;
	float delayGoal;

	//Contains a list of all the currenlty active character strings
	public List <characters> characterList=new List<characters>();

	//Contains a list of active single characters. Needed for multi character strings, although still used by single char strings
	public List <string> charList=new List<string>();

	//Set to true to pause game
	public bool paused;

	//The current Character string that a player is typing. Used for detecting misses and resetting progress
	public string currentCharTyping;
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
		Camera.main.fieldOfView = 180.0f;
		//alpha = "abcdefghjklmnopqrstuvwxyz[];',.";
		alpha = "abcdefghijklmnopqrstuvwxyz[];',./ABCDEFGHIJKLMNOPQRSTUVWXYZ{}:\"<>?";
		alpha1="qwertyuiop[]QWERTYUIOP{}";
		alpha2="asdfghjkl;'ASDFGHJKL:\"";
		alpha3="zxcvbnm,./ZXCVBNM<>?";
		currentCharTyping = "-1";

		//Sets variables to default
		Reset ();
		//Sets the players model to where the camera is
		GameObject.Find ("Body").GetComponent<Transform> ().position=GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position;
	}
	void Reset()
	{
		//controls Speed of the FoV Zoom effect
		delay = 5;
		delayGoal =4;
		//Sets the number of missed variables to zero
		missed = 0;	
		//Time is total time for the lesson
		time = 60;
		//Goal is when the next character will spawn. Need to update the decrement as well.
		goal = 57;
	}
	void FixedUpdate()
	{
		if (delay > 0 && paused==false) {
			
			delay -= Time.deltaTime;
			//Controls rate of FoV effect
			Camera.main.fieldOfView-=0.44f;
			if(delay<delayGoal)
			{
				delayGoal-=1;

			}
		}
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
		if (time > 0 && paused==false && delay<=0) {
			time -= Time.deltaTime;

			if (Input.anyKeyDown ) {
				bool found=false;
				if(Input.GetKey (KeyCode.LeftShift))
				{
					foreach(string c in charList)
					{
						if(c ==(Input.inputString))
							if(c==c.ToUpper())
								found=true;
					}
				}
				else
				{
					foreach(string c in charList)
					{
						if(c ==(Input.inputString))
							found=true;
					}
				}
				currentCharTyping="-1";
				if(found==false && !Input.GetKeyDown (KeyCode.LeftShift))
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
							for(int j=0;j<33;j++)
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
				//#############################################################Used for testing strings with multiple chars and cases.
				//Comment out for loop if you want only 1 char
				for(int i=0;i<2;i++)
				{
					c+=alpha[Random.Range(0,alpha.Length)].ToString ();
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
				goal-=3;
			}
		}
	}
	void OnTriggerEnter(Collider other) {
		//Debug.Log("Collision1");
	}
}