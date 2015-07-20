﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//This is the controller script for a timer based single character pressing typing lesson

public class controllerScript : MonoBehaviour {

	//Camera camera; 
	//Set these variables inside the editor
	public Mesh characterMesh;

	public Material characterMaterial;
	public Material characterMaterialRed;
	public Material characterMaterialLightRed;
	public Material characterMaterialYellow;
	public Material characterMaterialDarkYellow;
	public Material characterMaterialAqua;
	public Material characterMaterialSkyBlue;
	public Material characterMaterialGreen;
	public Material characterMaterialLime;

	public Mesh projectileMesh;
	public Material projectileMaterial;

	//Determines what game mode the level is in. 0 is keyboard, 1 is tap. 
	//Default is 0
	public static int mode;

	//The potential alphabet of the lesson
	public static string alpha;
	
	//"Top keyboard row"
	public static string alpha1;
	public static string alpha2;
	public static string alpha3;

	//Total length of level
	public float time;

	//Time when first spawn happens
	public static float goal;

	//Float interval between next spawn
	float spawn;

	public int missed;
	public float delay;
	float delayGoal;

	//Stores leap checking functions
	public FingerPosition finger;
	public IsLeapConnected con;
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

	//Contains the gesture checking class functionality
	public gestures gestureVariable;

	public class characters {
		public GameObject charObj;
		public int location;
		public string chars;
		
		public characters(int l, string c, Vector3 cameraOut, int m)
		{
			location = l;
			chars = c;
			charObj = new GameObject ();
			charObj.name = c;
			charObj.AddComponent<charScript>();
			charObj.GetComponent<charScript> ().start=cameraOut;
			charObj.GetComponent<charScript> ().val = chars;
			charObj.GetComponent<charScript> ().loc=l;
			charObj.GetComponent<charScript>().mode=m;
		}
	};

	// Use this for initialization
	void Start () {

		//Sets up function resevoir
		finger=new FingerPosition();
		con = new IsLeapConnected ();

		//for testing purposes
		mode = 1;
		if (mode == 1) {
			GameObject.Find ("SpaceShip_v003:Layer1").SetActive (false);
		}
		//Sets up alphabet
		Camera.main.fieldOfView = 180.0f;
		//UPPER and Lower Case
		//alpha = "abcdefghjklmnopqrstuvwxyz[];',.";
		alpha = "abcdefghijklmnopqrstuvwxyz[];',./ABCDEFGHIJKLMNOPQRSTUVWXYZ{}:\"<>?";
		alpha1="qwertyuiop[]QWERTYUIOP{}";
		alpha2="asdfghjkl;'ASDFGHJKL:\"";
		alpha3="zxcvbnm,./ZXCVBNM<>?";
		/*alpha = "abcdefghjklmnopqrstuvwxyz[];',.";
		alpha1="uiop[]";
		alpha2="hjkl;'";
		alpha3="nm,./Z";*/
		//alpha = "abcdefghijklmnopqrstuvwxyz";
		//alpha1="qwertyuiop[]";
		//alpha2="asdfghjkl;'";
		//alpha3="zxcvbnm,./";
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


		//Sets the default time if time is 0
		if(time==0)
			time = 60;
		//Goal is when the next character will spawn. Need to update the decrement as well.
		if (mode == 0 && goal ==0)
			goal = 58;
		else if( goal ==0)
			goal = 55;

		spawn = time - goal;
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

			if(mode==0) //Miss checking is performed externally from the characters in keyboard. It is handled in charScript for tap mode
			{
				if (Input.anyKeyDown ) {
					bool found=false;
					if(Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
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
					if(found==false && (!Input.GetKeyDown (KeyCode.LeftShift)&&!Input.GetKey (KeyCode.RightShift)))
						missed++;
				}
			}


			if ((int)time < goal && characterList.Count!=26) {
				Vector3 v3Pos=new Vector3(0,0,0);
				string c="a";
				int side=0;
				//Picks a random letter from the specified alphabet. Used when just using complete alphabet
				//c = alpha[Random.Range(0,alpha.Length)].ToString ();

				//Selects what row get and put the character on.
				int row=Random.Range (0,3);

				if(mode==0)
				{
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
								side=sideLocation(c);
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
								side=sideLocation(c);
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
								side=sideLocation(c);
								if(side<=5)
									v3Pos=new Vector3(-6.0f,-0.5f,1.5f);
								else
									v3Pos=new Vector3(8.0f,-0.5f,1.5f);
									break;
							default:
									Debug.Log("Default");
									break;
					}
				}
				else if(mode==1)
				{
					switch(row)
					{
					case 0:  
						side=Random.Range(0,alpha1.Length);
						c = alpha1[side].ToString ();
						side=sideLocation(c);
						v3Pos=new Vector3(-5.0f+side,1.5f,25f);
						break;
					case 1: 
						side=Random.Range(0,alpha2.Length);
						c = alpha2[side].ToString ();
						side=sideLocation(c);
						v3Pos=new Vector3(-5.0f+side,0.0f,25f);
						break;
					case 2: 
						side=Random.Range(0,alpha3.Length);
						c = alpha3[side].ToString ();
						side=sideLocation(c);
						v3Pos=new Vector3(-5.0f+side,-1.5f,25f);
						break;
					default:
						Debug.Log("Default");
						break;
					}
				}
				//#############################################################Used for testing strings with multiple chars and cases.
				//Comment out for loop if you want only 1 char
				/*for(int i=0;i<2;i++)
				{
					c+=alpha[Random.Range(0,alpha.Length)].ToString ();
				}*/
				//Spawns characters outside the camera range. Comment out if not desired behaviour
				/*v3Pos = new Vector3(0.857f, 0.857f, 0.0f);
				v3Pos = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * v3Pos;
				v3Pos += new Vector3(0.5f, 0.5f, 6.0f);
				v3Pos = Camera.main.ViewportToWorldPoint(v3Pos);*/

				//Spawns the new character. v3Pos is where it spawns in the world
				characters newChar=new characters(characterList.Count, c,v3Pos,mode);

				//Adds them to the list which keeps track of all active characters
				characterList.Add(newChar);

				//DECREMENT: should match the interval between the initial time and intial goal.
				goal-=spawn;
			}
		}
	}
	int sideLocation(string letter)
	{
		letter = letter.ToLower ();
		if (letter[0] == 'q' || letter[0] == 'a' || letter[0] == 'z') {
			return 1;
		}
		else if (letter[0] == 'w' || letter[0] == 's' || letter[0] == 'x') {
			return 2;
		}
		else if (letter[0] == 'e' || letter[0] == 'd' || letter[0] == 'c') {
			return 3;
		}
		else if (letter[0] == 'r' || letter[0] == 'f' || letter[0] == 'v' || letter[0] == 't' || letter[0] == 'g' || letter[0] == 'v') {
			return 4;
		}
		else if (letter[0] == 'y' || letter[0] == 'h' || letter[0] == 'n' || letter[0] == 'u' || letter[0] == 'j' || letter[0] == 'm') {
			return 5;
		}
		else if (letter[0] == 'i' || letter[0] == 'k' || letter[0] == ',' || letter[0] == '<') {
			return 6;
		}
		else if (letter[0] == 'o' || letter[0] == 'l' || letter[0] == '.' || letter[0] == '>') {
			return 7;
		}
		else if (letter[0] == 'p' || letter[0] == ';' || letter[0] == '[' || letter[0] == '\'' || letter[0] == ']' || letter[0] == '{' || letter[0] == '}' || letter[0] == ':' || letter[0] == '\"' || letter[0] == '/' || letter[0] == '?') {
			return 8;
		}
		return 0;
	}
	void OnTriggerEnter(Collider other) {
		//Debug.Log("Collision1");
	}
}