using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//This is the controller script for a timer based single character pressing typing lesson

public class controllerScript : MonoBehaviour {

	//Camera camera; 
	//Set these variables inside the editor
	public Mesh characterMesh;
	//Actually the projectile materials
	public Material characterMaterial;
	public Material characterMaterialRed;
	public Material characterMaterialOrange;
	public Material characterMaterialYellow;
	public Material characterMaterialPurple;
	public Material characterMaterialBlue;
	public Material characterMaterialCyan;
	public Material characterMaterialGreen;
	public Material characterMaterialBrown;
	//The asteroid materials
	public Material asteroidMaterial;
	public Material asteroidMaterialRed;
	public Material asteroidMaterialOrange;
	public Material asteroidMaterialYellow;
	public Material asteroidMaterialPurple;
	public Material asteroidMaterialBlue;
	public Material asteroidMaterialCyan;
	public Material asteroidMaterialGreen;
	public Material asteroidMaterialBrown;

	public Mesh projectileMesh;
	public Material projectileMaterial;

	//Determines what game mode the level is in. 0 is keyboard
	//Default is 0
	public static int mode;

	public int setMode;
	//boot camp level mode
	public static bool infinite;
	//track total spawned letters/words
	public int totalAsteroids;

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

	public float wait;

    //Amount of words typed
    public int wordsTyped; //   /levellmit

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

	//Checks what word is currently being typed. Used in mode 2 (keyboard string typing mode);
	public int wordTyping;
	//Players score
	public int score;

    //Determines if the player has passed the current level or not
    bool levelChecked;

	//Contains the gesture checking class functionality
	public gestures gestureVariable;

	//Captial spawn chance is an int representing a percentage, ie 10 is 10%. Set to 0 if you dont want captials. It also assumes that the alphabets are in the format lowercase,lowercase,uppercase,uppercase . IE abAB
	public static int capitalChance;

	//Tell the game what row to select characters from
	public static int rowToUse;

    public bool gameOver;

	//Sound stuff
	public AudioClip miss;
	AudioSource audio;

    //How many words need to be typed this level
    public int levelLimit;
    public float timeIncreasing;
    int levelStartTime;
    public int level;

	//Contains dictionary list of words
    //Default Words
	public static List<string> wordBank=new List<string>();
    public static List<string> wordBankEasy = new List<string>();
    public static List<string> wordBankMedium = new List<string>();
    public static List<string> wordBankHard = new List<string>();

    //Keeps track of how many asteroids the player has shot in keyboard mod
    public int rowCount;

	//External way to invoke the miss method
	public bool playMissSound;


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
	void Start () 
	{
		//Changing the gun material
		GameObject.Find ("shipGunR1").GetComponentInChildren<MeshRenderer> ().material = characterMaterialBlue;
		GameObject.Find ("shipGunR2").GetComponentInChildren<MeshRenderer> ().material = characterMaterialCyan;
		GameObject.Find ("shipGunR3").GetComponentInChildren<MeshRenderer> ().material = characterMaterialGreen;
		GameObject.Find ("shipGunR4").GetComponentInChildren<MeshRenderer> ().material = characterMaterialBrown;
		GameObject.Find ("shipGunL1").GetComponentInChildren<MeshRenderer> ().material = characterMaterialPurple;
		GameObject.Find ("shipGunL2").GetComponentInChildren<MeshRenderer> ().material = characterMaterialYellow;
		GameObject.Find ("shipGunL3").GetComponentInChildren<MeshRenderer> ().material = characterMaterialOrange;
		GameObject.Find ("shipGunL4").GetComponentInChildren<MeshRenderer> ().material = characterMaterialRed;
		if (mode == 0 || mode == 2) 
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
			time = ReservoirScript.inputtedTime;
		
		//Sets up function resevoir
		finger = new FingerPosition(); 
		con = new IsLeapConnected ();
		//^^^These 2 lines give an error, not sure if thats ok or not (*note by kane), error below
		//You are trying to create a MonoBehaviour using the 'new' keyword.  This is not allowed.  MonoBehaviours can only be added using AddComponent().  Alternatively, your script can inherit from ScriptableObject or no base class at all


		//for testing purposes
		//mode = 0;
		if (mode == 1) 
		{
			GameObject.Find ("SpaceShip_v003:Layer1").SetActive (false);
		} 
		else if (mode == 2)
		{
			setUpWordBank ();
			wordTyping=-1;
			rowCount = 0;
            wordsTyped = 0;
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

		wait = -1;
        timeIncreasing = 0;
        levelLimit = 20;
        levelStartTime = 0;
        level = 1;
        gameOver = false;
        levelChecked = false;
        if (mode == 2 && infinite == false) {
			spawn = 5;
			time = 10000;
			goal = time - 5;
		}

		setMode = mode;
		Debug.Log ("Goal: " + goal);
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
	// Update is called once per frame before LateUpdate is called
	void Update () {
		//Pauses or unpauses the game if escape is pressed

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (paused == false)
				paused = true;
			else
				paused = false;
		}
        if(paused==false && mode==2 && infinite== false && delay <= 0)
        { 
            timeIncreasing += Time.deltaTime;
            if (wordsTyped == levelLimit)
            {
                //if (timeIncreasing > 240)
                levelLimit = levelLimit + 5;
                //else
                //    levelLimit = levelLimit + 20;

                score += (int)timeIncreasing - levelStartTime;
                level++;
                levelChecked = true;
                if (level % 3 == 0)
                    spawn--;
            }
            else if ((int)timeIncreasing % 60 == 0 && wordsTyped < levelLimit && (int)timeIncreasing != 0 && levelChecked == false)
            {
                //if (levelChecked == false)
                {
                    GameObject.Find("HUD").GetComponentInChildren<Canvas>().enabled = false;
                    GameObject.Find("StatsMenu").GetComponent<HUDUpdater>().PopulateStatsMenu();
                    GameObject.Find("StatsMenu").GetComponentInChildren<Canvas>().enabled = true;
                    gameOver = true;
                    paused = true;
                }
            }
            else if(levelChecked==true && (int)timeIncreasing % 60 == 1)
            {
                levelChecked = false;
            }
        }
        //Checks if there is any more time for the game to run or if the game is paused
        if (time > 0 && paused == false && delay <= 0)
        {
            time -= Time.deltaTime;


            /*if(wait>0)
				wait-=Time.deltaTime;
			if(wait<0)
				wait=-1;*/
            if (mode==0 || mode == 2) //Miss checking is performed externally from the characters in keyboard. It is handled in charScript for tap mode and it is handled here for single char and string mode
			{
				//string debug="";

				if (Input.anyKeyDown )
				{
					//Debug.Log("currentCharTyping: "+currentCharTyping);
					bool found=false;
					if(mode==2 && wordTyping==-1)
					{
						for(int i=0;i<characterList.Count;i++)
						{
							//debug+="CHARLIST STRING: "+characterList[i].chars[0].ToString()+" Input.inputString: "+Input.inputString+" | ";
							if(characterList[i].chars[0].ToString()==Input.inputString)
							{
								found=true;
								break;
							}
						}		
						//if(found==false)
						//	Debug.Log(debug);
					}
					else if(mode==0 )
					{
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
								//debug+="c: "+c+" Input.inputString: "+Input.inputString+" | ";
								if(c ==(Input.inputString))
									found=true;
							}
						}
						//if(found==false)
						//	Debug.Log("SIZE: "+charList.Count+"-MODE0:\n "+debug);
						currentCharTyping="-1";
					}
					else if(mode==2 && wordTyping!=-1)
					{
						for(int i=0;i<characterList[wordTyping].chars.Length;i++)
						{
							//debug+="wordTyping: "+wordTyping+" characterListChar: "+characterList[wordTyping].chars[i].ToString()+" currentCharTyping: "+currentCharTyping+" Input.inputString: "+Input.inputString+" | ";
							if(characterList[wordTyping].chars[i].ToString()==currentCharTyping && currentCharTyping==Input.inputString)
							{
								found=true;
								break;
							}
						}
						//if(found==false)
						//	Debug.Log("MISSED-MODE2:\n "+debug);
					}
					else
					{
						//Debug.Log("DEFAULT CONDITION");
						found=true;
					}
					if(found==false && (!Input.GetKeyDown (KeyCode.LeftShift)&&!Input.GetKey (KeyCode.RightShift) && !Input.GetKey (KeyCode.Escape)))
					{
						//Debug.Log("MISSED");
						missed++;
						GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = true;
						Invoke ("PopUp", 0.5f);
						audio.PlayOneShot(miss, 1F);
						if(mode==2)
							wordTyping=-1;
						currentCharTyping="-1";
					}
				}
			}


			if ((int)time < goal && characterList.Count!=26) 
			{
				Vector3 v3Pos=new Vector3(0,0,0);
				string c="a";
				int side=0;
				//Picks a random letter from the specified alphabet. Used when just using complete alphabet
				//c = alpha[Random.Range(0,alpha.Length)].ToString ();
				//FOR TESTING
				//rowToUse=3;


				int row;
				if(rowToUse==0)//(rush) top 
					row=0;
				else if(rowToUse==1)//(solo) mid 
					row=1;
				else if(rowToUse==2)//bottom
					row=2;
				else//Selects what row get and put the character on.
					row=Random.Range (0,3);
				//Keyboard mode single char
				if(mode==0)
				{
					switch(row)
					{
						case 0:  
								//Will iterate through a maximum of 33 times to find out if the letter selected already exists
								for(int j=0;j<33;j++)
								{
									bool found=false;
									//Checks if there is a chance for a capital letter to spawn
									if(capitalChance==0)
										side=Random.Range(0,alpha1.Length);
									else
									{
											side=Random.Range(0,alpha1.Length/2);
											//Generate a random int and if it is below the capital chance spawn a random capital letter
											int randNum=Random.Range(0,100);
											if(randNum<capitalChance)
												side=alpha1.Length/2+Random.Range(0,alpha1.Length/2);
											
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
											side=alpha2.Length/2+Random.Range(0,alpha2.Length/2);
										
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
											side=alpha3.Length/2+Random.Range(0,alpha3.Length/2);
										
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
				//Tap mode
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
				//keyboard mode string
				else if(mode == 2)
				{
					//Spawn positions of each string
					Vector3 v3Pos1= new Vector3(-3.0f,0.0f,15f);
					Vector3 v3Pos2= new Vector3(0.0f,0.0f,15f);
					Vector3 v3Pos3= new Vector3(3.0f,0.0f,15f);

					//String value of each string
					string c1="";
					string c2="";
					string c3="";//(po)

                    if (infinite != true)
                    {
                        if (timeIncreasing > 240 && wordsTyped > levelLimit)
                        {
                           // Debug.Log("Hard Spawn");
                            c1 = wordBankHard[Random.Range(0, wordBankHard.Count)];
                            do
                            {
                                c2 = wordBankHard[Random.Range(0, wordBankHard.Count)];
                            } while (c2 == c1 || c2[0] == c1[0]);
                            do
                            {
                                c3 = wordBankHard[Random.Range(0, wordBankHard.Count)];
                            } while (c3 == c1 || c3 == c2 || c3[0] == c1[0] || c3[0] == c2[0]);
                        }
                        else if(timeIncreasing > 120 && wordsTyped > levelLimit)
                        {
                           // Debug.Log("Medium Spawn");
                            c1 = wordBankMedium[Random.Range(0, wordBankMedium.Count)];
                            do
                            {
                                c2 = wordBankMedium[Random.Range(0, wordBankMedium.Count)];
                            } while (c2 == c1 || c2[0] == c1[0]);
                            do
                            {
                                c3 = wordBankMedium[Random.Range(0, wordBankMedium.Count)];
                            } while (c3 == c1 || c3 == c2 || c3[0] == c1[0] || c3[0] == c2[0]);
                        }
                        else if(wordsTyped <= 40 )
                        {
                           // Debug.Log("Easy Spawn");
                            c1 = wordBankEasy[Random.Range(0, wordBankEasy.Count)];
                            do
                            {
                                c2 = wordBankEasy[Random.Range(0, wordBankEasy.Count)];
                            } while (c2 == c1 || c2[0] == c1[0]);
                            do
                            {
                                c3 = wordBankEasy[Random.Range(0, wordBankEasy.Count)];
                            } while (c3 == c1 || c3 == c2 || c3[0] == c1[0] || c3[0] == c2[0]);
                        }
                    }
                    else if (Random.Range(0, 2) == 0)
                    {
                        c1 = wordBankEasy[Random.Range(0, wordBankEasy.Count)];
                        do
                        {
                            c2 = wordBankEasy[Random.Range(0, wordBankEasy.Count)];
                        } while (c2 == c1 || c2[0] == c1[0]);
                        do
                        {
                            c3 = wordBankEasy[Random.Range(0, wordBankEasy.Count)];
                        } while (c3 == c1 || c3 == c2 || c3[0] == c1[0] || c3[0] == c2[0]);
                    }
                    else if (Random.Range(0, 2) == 1)
                    {
                        c1 = wordBankMedium[Random.Range(0, wordBankMedium.Count)];
                        do
                        {
                            c2 = wordBankMedium[Random.Range(0, wordBankMedium.Count)];
                        } while (c2 == c1 || c2[0] == c1[0]);
                        do
                        {
                            c3 = wordBankMedium[Random.Range(0, wordBankMedium.Count)];
                        } while (c3 == c1 || c3 == c2 || c3[0] == c1[0] || c3[0] == c2[0]);
                    }
                    else if (Random.Range(0, 2) == 2)
                    {
                        c1 = wordBankHard[Random.Range(0, wordBankHard.Count)];
                        do
                        {
                            c2 = wordBankHard[Random.Range(0, wordBankHard.Count)];
                        } while (c2 == c1 || c2[0] == c1[0]);
                        do
                        {
                            c3 = wordBankHard[Random.Range(0, wordBankHard.Count)];
                        } while (c3 == c1 || c3 == c2 || c3[0] == c1[0] || c3[0] == c2[0]);
                    }
                    else
                    {
                        c1 = wordBank[Random.Range(0, wordBank.Count)];
                        do
                        {
                            c2 = wordBank[Random.Range(0, wordBank.Count)];
                        } while (c2 == c1 || c2[0] == c1[0]);
                        do
                        {
                            c3 = wordBank[Random.Range(0, wordBank.Count)];
                        } while (c3 == c1 || c3 == c2 || c3[0] == c1[0] || c3[0] == c2[0]);
                    }
					//Spawn each string and Adds them to the list which keeps track of all active characters
					characters newChar1=new characters(characterList.Count, c1,v3Pos1,mode);
					characterList.Add(newChar1);
					characters newChar2=new characters(characterList.Count, c2,v3Pos2,mode);
					characterList.Add(newChar2);
					characters newChar3=new characters(characterList.Count, c3,v3Pos3,mode);
					characterList.Add(newChar3);
				}

				/*if(mode == 2)
				{
					//#############################################################Used for testing strings with multiple chars and cases.
					//Comment out for loop if you want only 1 char
					for(int i=0;i<2;i++)
					{
						c+=alpha[Random.Range(0,alpha.Length)].ToString ();
					}
				}*/
				//Spawns characters outside the camera range. Comment out if not desired behaviour
				/*v3Pos = new Vector3(0.857f, 0.857f, 0.0f);
				v3Pos = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * v3Pos;
				v3Pos += new Vector3(0.5f, 0.5f, 6.0f);
				v3Pos = Camera.main.ViewportToWorldPoint(v3Pos);*/

				if(mode==0 || mode==1)
				{
					//Spawns the new character. v3Pos is where it spawns in the world
					characters newChar=new characters(characterList.Count, c,v3Pos,mode);

					totalAsteroids++;
					//Debug.Log ("totalAsteroids="+totalAsteroids);

					//Adds them to the list which keeps track of all active characters
					characterList.Add(newChar);
				}
				//DECREMENT: should match the interval between the initial time and intial goal.
				goal-=spawn;
			}
		}
		if (time <= 0 && (mode==1 || mode==0)) //when time runs out, show end of level statistics
		{
			GameObject.Find ("HUD").GetComponentInChildren<Canvas> ().enabled = false;
			GameObject.Find ("StatsMenu").GetComponent<HUDUpdater>().PopulateStatsMenu();
			GameObject.Find ("StatsMenu").GetComponentInChildren<Canvas> ().enabled = true;
            gameOver = true;
            paused = true;
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
	void setUpWordBank()
	{
		//nouns
		wordBank.Add ("time");
		wordBank.Add ("person");
		wordBank.Add ("year");
		wordBank.Add ("way");
		wordBank.Add ("day");
		wordBank.Add ("man");
		wordBank.Add ("world");
		wordBank.Add ("life");
		wordBank.Add ("hand");
		wordBank.Add ("part");
		wordBank.Add ("child");
		wordBank.Add ("eye");
		wordBank.Add ("woman");
		wordBank.Add ("place");
		wordBank.Add ("work");
		wordBank.Add ("week");
		wordBank.Add ("point");
		wordBank.Add ("government");
		wordBank.Add ("company");
		wordBank.Add ("number");
		wordBank.Add ("group");
		wordBank.Add ("problem");
		wordBank.Add ("fact");
		wordBank.Add ("apple");
		wordBank.Add ("igloo");
		wordBank.Add ("jump");
		wordBank.Add ("king");
		wordBank.Add ("queen");
		wordBank.Add ("rain");
		wordBank.Add ("safe");
		wordBank.Add ("usual");
		wordBank.Add ("valley");
		wordBank.Add ("zebra");
		//verbs
		wordBank.Add ("good");
		wordBank.Add ("new");
		wordBank.Add ("first");
		wordBank.Add ("last");
		wordBank.Add ("long");
		wordBank.Add ("great");
		wordBank.Add ("little");
		wordBank.Add ("able");
		wordBank.Add ("other");
		wordBank.Add ("old");
		wordBank.Add ("right");
		wordBank.Add ("big");
		wordBank.Add ("high");
		wordBank.Add ("different");
		wordBank.Add ("small");
		wordBank.Add ("large");
		wordBank.Add ("next");
		wordBank.Add ("early");
		wordBank.Add ("young");
		wordBank.Add ("important");
		wordBank.Add ("same");
		wordBank.Add ("public");
		wordBank.Add ("bad");
		wordBank.Add ("cross");
		wordBank.Add ("invent");
		wordBank.Add ("jab");
		wordBank.Add ("kick");
		wordBank.Add ("meet");
		wordBank.Add ("question");
		wordBank.Add ("thousand");
		wordBank.Add ("under");
		wordBank.Add ("value");

        //Easy Words
        wordBankEasy.Add("at");
        wordBankEasy.Add("am");
        wordBankEasy.Add("be");
        wordBankEasy.Add("go");
        wordBankEasy.Add("is");
        wordBankEasy.Add("to");
        wordBankEasy.Add("too");
        wordBankEasy.Add("and");
        wordBankEasy.Add("age");
        wordBankEasy.Add("aid");
        wordBankEasy.Add("all");
        wordBankEasy.Add("bow");
        wordBankEasy.Add("bye");
        wordBankEasy.Add("cam");
        wordBankEasy.Add("doe");
        wordBankEasy.Add("dog");
        wordBankEasy.Add("dot");
        wordBankEasy.Add("dug");
        wordBankEasy.Add("dye");
        wordBankEasy.Add("eel");
        wordBankEasy.Add("fig");
        wordBankEasy.Add("gap");
        wordBankEasy.Add("gum");
        wordBankEasy.Add("ham");
        wordBankEasy.Add("hex");
        wordBankEasy.Add("jaw");
        wordBankEasy.Add("jug");
        wordBankEasy.Add("kid");
        wordBankEasy.Add("lab");
        wordBankEasy.Add("lag");
        wordBankEasy.Add("lie");
        wordBankEasy.Add("maw");
        wordBankEasy.Add("moa");
        wordBankEasy.Add("mob");
        wordBankEasy.Add("mum");
        wordBankEasy.Add("nap");
        wordBankEasy.Add("net");
        wordBankEasy.Add("new");
        wordBankEasy.Add("nag");
        wordBankEasy.Add("oak");
        wordBankEasy.Add("oar");
        wordBankEasy.Add("odd");
        wordBankEasy.Add("off");
        wordBankEasy.Add("one");
        wordBankEasy.Add("pad");
        wordBankEasy.Add("peg");
        wordBankEasy.Add("pen");
        wordBankEasy.Add("pod");
        wordBankEasy.Add("pro");
        wordBankEasy.Add("pur");
        wordBankEasy.Add("pup");
        wordBankEasy.Add("rev");
        wordBankEasy.Add("rex");
        wordBankEasy.Add("rib");
        wordBankEasy.Add("run");
        wordBankEasy.Add("sad");
        wordBankEasy.Add("sap");
        wordBankEasy.Add("sat");
        wordBankEasy.Add("say");
        wordBankEasy.Add("sim");
        wordBankEasy.Add("sir");
        wordBankEasy.Add("sky");
        wordBankEasy.Add("tab");
        wordBankEasy.Add("tar");
        wordBankEasy.Add("ute");
        wordBankEasy.Add("use");
        wordBankEasy.Add("van");
        wordBankEasy.Add("way");
        wordBankEasy.Add("wax");
        wordBankEasy.Add("web");
        wordBankEasy.Add("wow");
        wordBankEasy.Add("yak");
        wordBankEasy.Add("you");
        wordBankEasy.Add("zip");
        wordBankEasy.Add("zit");
        wordBankEasy.Add("zoo");
        wordBankEasy.Add("beer");
        wordBankEasy.Add("beet");
        wordBankEasy.Add("bees");
        wordBankEasy.Add("bell");
        wordBankEasy.Add("cook");
        wordBankEasy.Add("dodo");
        wordBankEasy.Add("doom");
        wordBankEasy.Add("food");
        wordBankEasy.Add("fool");
        wordBankEasy.Add("foot");
        wordBankEasy.Add("free");
        wordBankEasy.Add("keen");
        wordBankEasy.Add("moon");
        wordBankEasy.Add("null");
        wordBankEasy.Add("noon");
        wordBankEasy.Add("puss");
        wordBankEasy.Add("putt");
        wordBankEasy.Add("read");
        wordBankEasy.Add("reel");
        wordBankEasy.Add("reef");
        wordBankEasy.Add("seen");
        wordBankEasy.Add("soot");
        wordBankEasy.Add("tutu");
        wordBankEasy.Add("wall");


        //Medium Words
        wordBankMedium.Add("aloe");
        wordBankMedium.Add("arid");
        wordBankMedium.Add("apex");
        wordBankMedium.Add("aqua");
        wordBankMedium.Add("babe");
        wordBankMedium.Add("baby");
        wordBankMedium.Add("bent");
        wordBankMedium.Add("beat");
        wordBankMedium.Add("beta");
        wordBankMedium.Add("cart");
        wordBankMedium.Add("card");
        wordBankMedium.Add("cash");
        wordBankMedium.Add("cask");
        wordBankMedium.Add("cold");
        wordBankMedium.Add("duel");
        wordBankMedium.Add("dogs");
        wordBankMedium.Add("glue");
        wordBankMedium.Add("grow");
        wordBankMedium.Add("goal");
        wordBankMedium.Add("jail");
        wordBankMedium.Add("liar");
        wordBankMedium.Add("lice");
        wordBankMedium.Add("lick");
        wordBankMedium.Add("mine");
        wordBankMedium.Add("mane");
        wordBankMedium.Add("mega");
        wordBankMedium.Add("muck");
        wordBankMedium.Add("nail");
        wordBankMedium.Add("navy");
        wordBankMedium.Add("neck");
        wordBankMedium.Add("onyx");
        wordBankMedium.Add("opal");
        wordBankMedium.Add("open");
        wordBankMedium.Add("path");
        wordBankMedium.Add("pads");
        wordBankMedium.Add("poem");
        wordBankMedium.Add("rest");
        wordBankMedium.Add("sewn");
        wordBankMedium.Add("slim");
        wordBankMedium.Add("slug");
        wordBankMedium.Add("text");
        wordBankMedium.Add("toes");
        wordBankMedium.Add("toil");
        wordBankMedium.Add("wren");
        wordBankMedium.Add("jazzy");
        wordBankMedium.Add("frizz");
        wordBankMedium.Add("pizza");
        wordBankMedium.Add("quack");
        wordBankMedium.Add("junky");
        wordBankMedium.Add("quirk");
        wordBankMedium.Add("whiff");
        wordBankMedium.Add("zesty");
        wordBankMedium.Add("zebra");
        wordBankMedium.Add("kinky");
        wordBankMedium.Add("chick");
        wordBankMedium.Add("fixed");
        wordBankMedium.Add("picky");
        wordBankMedium.Add("waxed");
        wordBankMedium.Add("buffy");
        wordBankMedium.Add("pixel");
        wordBankMedium.Add("retina");
        wordBankMedium.Add("retain");
        wordBankMedium.Add("ratine");
        wordBankMedium.Add("eolian");
        wordBankMedium.Add("tonier");
        wordBankMedium.Add("ornate");
        wordBankMedium.Add("orient");
        wordBankMedium.Add("norite");
        wordBankMedium.Add("atoner");
        wordBankMedium.Add("auntie");
        wordBankMedium.Add("ration");
        wordBankMedium.Add("aroint");
        wordBankMedium.Add("teniae");
        wordBankMedium.Add("iodate");
        wordBankMedium.Add("roadie");
        wordBankMedium.Add("renail");
        wordBankMedium.Add("nailer");
        wordBankMedium.Add("linear");
        wordBankMedium.Add("larine");
        wordBankMedium.Add("aliner");
        wordBankMedium.Add("tolane");
        wordBankMedium.Add("etalon");
        wordBankMedium.Add("tineal");
        wordBankMedium.Add("tenail");
        wordBankMedium.Add("tailer");
        wordBankMedium.Add("retial");
        wordBankMedium.Add("retail");
        wordBankMedium.Add("entail");
        wordBankMedium.Add("ariose");
        wordBankMedium.Add("entoil");
        wordBankMedium.Add("retine");
        wordBankMedium.Add("entire");
        wordBankMedium.Add("qwerty");
        wordBankMedium.Add("reloan");
        wordBankMedium.Add("loaner");
        wordBankMedium.Add("loiter");
        wordBankMedium.Add("toiler");
        wordBankMedium.Add("rained");
        wordBankMedium.Add("uniter");
        wordBankMedium.Add("triune");
        wordBankMedium.Add("Dylan");
        wordBankMedium.Add("Kody");
        wordBankMedium.Add("Kane");
        wordBankMedium.Add("Josh");
        wordBankMedium.Add("Julian");

        //Hard Words
        wordBankHard.Add("elation ");
        wordBankHard.Add("routine   ");
        wordBankHard.Add("alphabet ");
        wordBankHard.Add("outerwear   ");
        wordBankHard.Add("urinate   ");
        wordBankHard.Add("nitrate   ");
        wordBankHard.Add("nectarine   ");
        wordBankHard.Add("ruminate   ");
        wordBankHard.Add("oriental   ");
        wordBankHard.Add("aileron");
        wordBankHard.Add("evasion");
        wordBankHard.Add("toenail");
        wordBankHard.Add("retinal");
        wordBankHard.Add("reliant");
        wordBankHard.Add("ratline");
        wordBankHard.Add("latrine");
        wordBankHard.Add("aneroid");
        wordBankHard.Add("trainee");
        wordBankHard.Add("retainer");
        wordBankHard.Add("arenite");
        wordBankHard.Add("inertia");
        wordBankHard.Add("aeolian");
        wordBankHard.Add("trained");
        wordBankHard.Add("trainer");
        wordBankHard.Add("attired");
        wordBankHard.Add("probate");
        wordBankHard.Add("aconite");
        wordBankHard.Add("bandeau");
        wordBankHard.Add("raincoat");
        wordBankHard.Add("neurone");
        wordBankHard.Add("facebook");
        wordBankHard.Add("uranium");
        wordBankHard.Add("unimpaired");
        wordBankHard.Add("stonier");
        wordBankHard.Add("orients");
        wordBankHard.Add("shoestring");
        wordBankHard.Add("nitrites");
        wordBankHard.Add("venation");
        wordBankHard.Add("neuronal");
        wordBankHard.Add("steering");
        wordBankHard.Add("stainer");
        wordBankHard.Add("retsina");
        wordBankHard.Add("retinas");
        wordBankHard.Add("retains");
        wordBankHard.Add("ratlines");
        wordBankHard.Add("nastier");
        wordBankHard.Add("antsier");
        wordBankHard.Add("aesthetic");
        wordBankHard.Add("alunite");
        wordBankHard.Add("aliener");
        wordBankHard.Add("treason");
        wordBankHard.Add("senator");
        wordBankHard.Add("stationers");
        wordBankHard.Add("outlier");
        wordBankHard.Add("mozzarella");
        wordBankHard.Add("neutral");
        wordBankHard.Add("moraine");
        wordBankHard.Add("airline");
        wordBankHard.Add("reginae");
        wordBankHard.Add("niterie");
        wordBankHard.Add("uterine");
        wordBankHard.Add("reunite");
        wordBankHard.Add("retinue");
        wordBankHard.Add("outline");
        wordBankHard.Add("elution");
        wordBankHard.Add("potential");
        wordBankHard.Add("regulator");
        wordBankHard.Add("attained");
        wordBankHard.Add("retinol");
        wordBankHard.Add("radiation");
        wordBankHard.Add("tearing");
        wordBankHard.Add("tangier");
        wordBankHard.Add("repaint");
        wordBankHard.Add("pertain");
        wordBankHard.Add("painter");
        wordBankHard.Add("lineage");
        wordBankHard.Add("ingrate");
        wordBankHard.Add("ingratiate");
        wordBankHard.Add("granite");
        wordBankHard.Add("amniotic");
        wordBankHard.Add("rations");
        wordBankHard.Add("foliate");
        wordBankHard.Add("aroints");
        wordBankHard.Add("larcenous");
        wordBankHard.Add("burnoose");
        wordBankHard.Add("trailed");
        wordBankHard.Add("swordtail");
        wordBankHard.Add("artesian");
        wordBankHard.Add("dilation");
        wordBankHard.Add("alliterate");
        wordBankHard.Add("cutaneous");
        wordBankHard.Add("arteriole");
        wordBankHard.Add("audience");
        wordBankHard.Add("outland");
        wordBankHard.Add("erotica");
        wordBankHard.Add("entrain");
        wordBankHard.Add("violate");
        wordBankHard.Add("unities");
        wordBankHard.Add("excellence");
        wordBankHard.Add("extraordinary");




    }
    public void PopUp()
	{
		GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = false;
	}
}