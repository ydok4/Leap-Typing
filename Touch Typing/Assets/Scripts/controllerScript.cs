using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//This is the controller script for a timer based single character pressing typing lesson

public class controllerScript : MonoBehaviour {

	//Camera camera; 
	//Set these variables inside the editor
	public Mesh characterMesh;

	public Material characterMaterial;
	public Material characterMaterialRed;
	public Material characterMaterialOrange;
	public Material characterMaterialYellow;
	public Material characterMaterialPurple;
	public Material characterMaterialBlue;
	public Material characterMaterialCyan;
	public Material characterMaterialGreen;
	public Material characterMaterialBrown;

	public Mesh projectileMesh;
	public Material projectileMaterial;

	//Determines what game mode the level is in. 0 is keyboard
	//Default is 0
	public static int mode;
	//boot camp level mode
	public static bool infinite;

	//The potential alphabet of the lesson
	public static string alpha;
	
	//"Top keyboard row"
	public static string alpha1;
	public static string alpha2;
	public static string alpha3;

	//Total length of level
	public float time; //change to int??

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

	//Captial spawn chance is an int representing a percentage, ie 10 is 10%. Set to 0 if you dont want captials. It also assumes that the alphabets are in the format lowercase,lowercase,uppercase,uppercase . IE abAB
	public static int capitalChance;

	//Tell the game what row to select characters from
	public static int rowToUse;

	//Sound stuff
	public AudioClip miss;
	AudioSource audio;

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

		//Changing the gun material
		GameObject.Find ("shipGunR1").GetComponentInChildren<MeshRenderer> ().material = characterMaterialBlue;
		GameObject.Find ("shipGunR2").GetComponentInChildren<MeshRenderer> ().material = characterMaterialCyan;
		GameObject.Find ("shipGunR3").GetComponentInChildren<MeshRenderer> ().material = characterMaterialGreen;
		GameObject.Find ("shipGunR4").GetComponentInChildren<MeshRenderer> ().material = characterMaterialBrown;
		GameObject.Find ("shipGunL1").GetComponentInChildren<MeshRenderer> ().material = characterMaterialPurple;
		GameObject.Find ("shipGunL2").GetComponentInChildren<MeshRenderer> ().material = characterMaterialYellow;
		GameObject.Find ("shipGunL3").GetComponentInChildren<MeshRenderer> ().material = characterMaterialOrange;
		GameObject.Find ("shipGunL4").GetComponentInChildren<MeshRenderer> ().material = characterMaterialRed;
		if (mode == 0) 
		{
			GameObject.Find ("GestureController").transform.localRotation = Quaternion.Euler (270f,180f,0f);
		}
		else if(mode == 1)
		{
			GameObject.Find ("GestureController").transform.localRotation = Quaternion.Euler (0f,0f,0f);
		}

		//Set time depending on mode or user input from main menu
		if (infinite == true)
			time = 10000;
		else
			time = LoadOnClick.inputtedTime;

		//Sets up function resevoir
		finger=new FingerPosition();
		con = new IsLeapConnected ();

		//for testing purposes
		//mode = 0;
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

		//Sets up audio
		audio = GetComponent<AudioSource>();


		//PlayerPrefsX2 Documentation: http://wiki.unity3d.com/index.php/ArrayPrefs2
		//If you try to get data from a pref that doesn't exist, it will have a size of 0
		//Setting the data in the prefs file for the first time. This will need to be done for each array (field) you are keeping track of.
		//At the minimum we need a user name. But we can have overall accuracy, score for each level etc.

		/*
			//Firstly declare the list
			List <int>aList = new List<int>();
			//Add relevant data to the list
			for (int i = 0; i < 10; i++) 
				aList.Add(i);
			//Convert list back to array
			int[] anArray = aList.ToArray();
			//Set the data in prefs
			PlayerPrefsX.SetIntArray ("Numbers", anArray);

			//Getting the data. Will need to be done on each game load.
			int[] anArray2 = PlayerPrefsX.GetIntArray ("Numbers");
		*/
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

		//????????????
		//if(infinite == true && time < 1 )
			//time = 10000;

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
					if(found==false && (!Input.GetKeyDown (KeyCode.LeftShift)&&!Input.GetKey (KeyCode.RightShift) && !Input.GetKey (KeyCode.Escape)))
					{
						missed++;
						GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = true;
						Invoke ("PopUp", 0.5f);
						audio.PlayOneShot(miss, 1F);
					}
				}
			}


			if ((int)time < goal && characterList.Count!=26) {
				Vector3 v3Pos=new Vector3(0,0,0);
				string c="a";
				int side=0;
				//Picks a random letter from the specified alphabet. Used when just using complete alphabet
				//c = alpha[Random.Range(0,alpha.Length)].ToString ();
				//FOR TESTING
				//rowToUse=3;

				int row;
				if(rowToUse==0)
					row=0;
				else if(rowToUse==1)
					row=1;
				else if(rowToUse==2)
					row=2;
				else//Selects what row get and put the character on.
					row=Random.Range (0,3);

				if(mode==0)
				{
					switch(row)
					{
						case 0:  
								
								for(int j=0;j<33;j++)
								{
									bool found=false;
									if(capitalChance==0)
										side=Random.Range(0,alpha1.Length);
									else
									{
											side=Random.Range(0,alpha1.Length/2);
											int randNum=Random.Range(0,100);
											if(randNum<capitalChance)
												side+=Random.Range(0,alpha1.Length/2);
											
									}
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
									if(capitalChance==0)
										side=Random.Range(0,alpha2.Length);
									else
									{
										side=Random.Range(0,alpha2.Length/2);
										int randNum=Random.Range(0,100);
										if(randNum<capitalChance)
											side+=Random.Range(0,alpha2.Length/2);
										
									}
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
									if(capitalChance==0)
										side=Random.Range(0,alpha3.Length);
									else
									{
										side=Random.Range(0,alpha3.Length/2);
										int randNum=Random.Range(0,100);
										if(randNum<capitalChance)
											side+=Random.Range(0,alpha3.Length/2);
										
									}
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
						v3Pos=new Vector3(-5.0f+side,1.5f,15f);
						break;
					case 1: 
						side=Random.Range(0,alpha2.Length);
						c = alpha2[side].ToString ();
						side=sideLocation(c);
						v3Pos=new Vector3(-5.0f+side,0.0f,15f);
						break;
					case 2: 
						side=Random.Range(0,alpha3.Length);
						c = alpha3[side].ToString ();
						side=sideLocation(c);
						v3Pos=new Vector3(-5.0f+side,-1.5f,15f);
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
		if (time <= 0)
			paused = true;
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
		else if (letter[0] == 'r' || letter[0] == 'f' || letter[0] == 'v' || letter[0] == 't' || letter[0] == 'g' || letter[0] == 'b') {
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

	public void PopUp()
	{
		GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = false;
	}
}