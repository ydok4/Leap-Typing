using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class charScript : MonoBehaviour {
	//The character that this object is
	public string val;
	//Where this character is stored in the characterlist
	public int loc;
	//GameObject model;
	//The text object attached to the game object
	GameObject text;
	
	//Set in controller script this determines where the object starts
	public Vector3 start;

	//Checks if the material has been changed for tap mode
	bool doOnceMaterial;

	//checks whether there is already a projectile heading for that character
	public bool fired;
	
	//Sets what character is currently being checked
	public int checkedChar;
	
	//Loads the fond  resource
	//Font myFont = Resources.Load<Font> ("DigitaldreamFatNarrowModified");
	
	//Checks the next char in the list of string 
	string checkChar;
	bool charUpper;
	
	//Checks if char is [];',.
	bool isNonAlpha;
	//Checks if char is {}:"<>
	bool isNonAlphaUpper;
	//Tells the word to start from the beginning
	public bool reset;
	
	public float aliveTime;

	//Loads the explosion prefab to be used during 'OnTriggerEnter'
	//NB: It should load the prefab from the 'Resources' folder. Add file path if moving the prefab file elsewhere
	public GameObject explosion;// = (GameObject)Resources.Load ("explosion_asteroid.prefab");

	public int mode;

	//Sound stuff
	public AudioClip miss;
	public AudioClip hit;
	AudioSource audio;


	// Use this for initialization
	void Start () {
		aliveTime = 12f;
		gameObject.transform.position = GameObject.Find ("Main Camera").transform.position;
		gameObject.transform.parent = GameObject.Find ("Main Camera").transform;
		gameObject.transform.localPosition = start;
		//Adds components to character object
		gameObject.AddComponent<AudioSource> ();
		gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();
		gameObject.AddComponent<BoxCollider> ();
		gameObject.GetComponent<BoxCollider> ().isTrigger=true;
		//Sets the object mesh and material. Both are stored as public variables in the controllerScript
		gameObject.GetComponent<MeshFilter> ().mesh=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMesh;
		//Material changes depending on the incoming letter through a function call that returns a material value
		if (mode == 1)
			gameObject.GetComponent<MeshRenderer> ().material = GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterMaterial;
		else if (mode == 0)
			gameObject.GetComponent<MeshRenderer> ().material = asteroidMaterial (val);
		else if(mode==2 && loc>2)
			gameObject.GetComponent<MeshRenderer> ().material = asteroidMaterial ("-1");
		else if(mode==2)
			gameObject.GetComponent<MeshRenderer> ().material = asteroidMaterial (val[0].ToString());

        //Modify Asteroid Scale
        if (mode == 0)
            gameObject.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
        else
            gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        text = new GameObject ();
		//ModifyAsteroidRotation - WIP. Issue is text wont orientate correctly
		//Vector3 asRotate = new Vector3 (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360));
		//gameObject.transform.localRotation = Quaternion.Euler (asRotate);
		
		text.transform.parent=gameObject.transform;
		//Adds components to text
		text.AddComponent<Canvas>();
		text.AddComponent<Text>();
		text.AddComponent<CanvasScaler> ();
		
		//Sets up components
		text.transform.localPosition = Vector3.zero;
		Font myFont = Resources.Load<Font> ("Neuton-Regular");
		text.GetComponent<Text> ().font = myFont;
		text.GetComponent<Text> ().horizontalOverflow = HorizontalWrapMode.Overflow;
		text.GetComponent<Text> ().verticalOverflow = VerticalWrapMode.Overflow;
		text.GetComponent<Text>().text=val;
		//text.GetComponent<Text> ().color = new Color (0f, 0f, 0f);	Remove comment to turn text black
		text.GetComponent<Text> ().fontSize = 3;
		text.GetComponent<CanvasScaler> ().dynamicPixelsPerUnit = 80;
		text.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		text.GetComponent<Text> ().name="Character Text";
		text.GetComponent<RectTransform> ().localPosition += new Vector3 (0.0f, 0.0f, 6.501f);
		Vector3 rotate=text.GetComponent<RectTransform> ().localRotation.eulerAngles;
		rotate += new Vector3 (0.0f, 180.0f, 0.0f);
		text.GetComponent<RectTransform> ().localRotation=Quaternion.Euler(rotate);
		text.GetComponent<RectTransform> ().localScale=new Vector3(1,1,1);
		text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1.0f, 1.0f);

		if (mode == 0)//Only for keboard mode
			//Makes character look at camera
			gameObject.transform.LookAt (Camera.main.transform.position);
		else
			gameObject.transform.rotation = Quaternion.Euler (180f, 0f, 180f);
		fired = false;
		//Initialises checkedChar to the first character location (of Val)
		checkedChar = -1;
		
		NextLetter(checkedChar,true);
		
		reset = false;
		gameObject.transform.parent = null;

		//Loads explosion prefab
		explosion = (GameObject)Resources.Load ("explosion_asteroid");

		//Get audio Source
		audio = GetComponent<AudioSource>();
		miss = Resources.Load<AudioClip> ("DryGun");
		hit = Resources.Load<AudioClip> ("LaserHit");

		//Add underscore if it is an uppercase letter
		if (val.ToUpper() == val && val!="[" && val!="]" && val!=";" && val!="'" && val!="," && val!="." && val!="/") {
			GameObject underScore = (GameObject)	Instantiate(text, text.transform.position, text.transform.rotation);
			underScore.GetComponent<Text>().text="_";
			underScore.transform.parent=gameObject.transform;
			underScore.name="underScore";
			underScore.transform.localPosition=new Vector3(-0.05f, -0.67f, 6.66f);
			//underScore.GetComponent<CanvasScaler> ().dynamicPixelsPerUnit = 100;
			//underScore.GetComponent<Text> ().fontSize =1;
			underScore.transform.localScale=new Vector3(1f,1f,1.0f);
		}
	}
	
	// LateUpdate is called once per frame after all other update functions have been completed
	void LateUpdate () {
		
		//Checks if game is paused
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().paused == false ) 
		{
			
			//Checks if the the key pressed meets conditions to be considered for checking. Ie A button has been pressed, the word is not completed and the current typing char is empty or already set to the current char
			if (((Input.anyKeyDown) && fired == false && (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping=="-1" || GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping==val[checkedChar].ToString())) && (mode==0 || mode==2) && (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().wordTyping==loc || GameObject.Find ("Main Camera").GetComponent<controllerScript> ().wordTyping==-1 ||mode==0))
			{
				
				bool foundLetter=false;
				bool continueOn=false;
				//Checks if the NonAlpha char button has actually been pressed and determines which one is correct. This had to be done non-standard due to unity not recognising {}
				if(isNonAlpha==true)
				{
					if(checkChar=="{" && Input.GetKeyDown("[") && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
					{
						//Debug.Log("KEY { CHECKED");
						continueOn=true;
					}
					else if(checkChar=="}" && Input.GetKeyDown("]") && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
					{
						//Debug.Log("KEY } CHECKED");
						continueOn=true;
					}
					else if(checkChar==":" && Input.GetKeyDown(";") && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
					{
						//Debug.Log("KEY : CHECKED");
						continueOn=true;
					}
					else if(checkChar=="\"" && Input.GetKeyDown("'") && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
					{
						//Debug.Log("KEY \" CHECKED");
						continueOn=true;
					}
					else if(checkChar=="<" && Input.GetKeyDown(",") && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
					{
						//Debug.Log("KEY < CHECKED");
						continueOn=true;
					}
					else if(checkChar==">" && Input.GetKeyDown(".") && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
					{
						//Debug.Log("KEY > CHECKED");
						continueOn=true;
					}
					else if(checkChar=="?" && Input.GetKeyDown("/") && (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)))
					{
						//Debug.Log("KEY ? CHECKED");
						continueOn=true;
					}
					else if(checkChar!="{" && checkChar!="}")
					{
						if (Input.GetKeyDown (checkChar) &&!Input.GetKey (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))
						{
							//Debug.Log("STANDARD NONALPHA CHECKING METHOD CHECKED");
							continueOn=true;
						}
					}
				}
				else if(checkChar!="{" && checkChar!="}")//Needed because unity doesnt recognise "{" or "}" and throws a bitch fit
				{
					//Used for alpha characters. If only NonAlpha characters worked the same way
					if(Input.GetKeyDown (checkChar.ToLower ()))
					{
						foundLetter=true;
					}
				}
				
				
				
				if(foundLetter==true || continueOn==true)
				{
					int findFinger=0;
					if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected() == true)
						findFinger=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().finger.IsPressed(val[checkedChar].ToString());
					//if(findFinger==2 || GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected() == false)
					{
						//Checks if the char is the first char of the string and that there is string currently being typed and sets the the current typed string to this objects value

						if(checkedChar==0 && val.Length!=1 && GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping=="-1" && mode==0)
							GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping=val[0].ToString();

						//Checks to make sure the case is correct and that the character pressed is correct. It double checks uppercase nonAlpha characters, probably should fix at some point but it works atm.
						if((((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && charUpper==true && foundLetter==true )|| (charUpper==false && !Input.GetKey (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))&& isNonAlpha==false && foundLetter==true)|| (continueOn==true && ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && isNonAlphaUpper==true || isNonAlphaUpper==false && !Input.GetKey (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))))
						{
							
							//Checks if the character pressed was the last char and spawns a projectile to kill it. Also checks if it is in the front row if it is in string keyboard mode
							if(checkedChar+1==val.Length || (mode==2 && loc<3 && checkedChar+1==val.Length) || mode==0)
							{


								//Creates a projectile object which will move towards this character
								GameObject projectile = new GameObject ();
								
								projectile.AddComponent<projectileScript> ();
								//Special case for / because unity cant find the object by name ie /
								if(val=="/")
								{
									gameObject.name="slash";
									projectile.GetComponent<projectileScript> ().target="slash";
									val="slash";
								}
								else
									projectile.GetComponent<projectileScript> ().target = val;
								fired=true;
								if(findFinger==2)
									GameObject.Find ("Main Camera").GetComponent<controllerScript> ().score += 1;
								//If the game is in single char keyboard mode remove the char, if it is in string mode remove 3 chars
								//if(mode==0)
								removeCharacter();
								//else if(mode==2)
								//	remove3Character();
								GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping="-1";
								audio.PlayOneShot(hit, 1F);
								if(mode==2)
								{
									GameObject.Find ("Main Camera").GetComponent<controllerScript> ().wordTyping=-1;
									//GameObject.Find ("Main Camera").GetComponent<controllerScript> ().wait=0.01f;
								}
							}
							//Picks the next char and updates components to reflect that
							else
							{
								//Should only move to the next char if the correct button was pressed and it is single char keyboard mode or if it is multi char keyboard, the correct button was pressed and it was correct for only any asteroid in the first row
								if(mode==0 || (mode==2 && loc<3))
								{
									//Removes current character from charList. So it is once again a non missable character
									removeCharacter();
									//Gets the next char in the string by passing it the index of the current char
									NextLetter(checkedChar,true);
									//Debug.Log ("Checkchar: " + checkChar);
									GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping=checkChar;
									//Change character/mesh colour 
									//apply small explosion / shake effect 
									gameObject.GetComponent<MeshRenderer> ().material = asteroidMaterial (val[checkedChar].ToString());
									//Updates displayed text
									text.GetComponent<Text>().text="";

									for(int i=checkedChar;i<val.Length;i++)
									{
										text.GetComponent<Text>().text+=val[i];
									}
									if(mode==2)
									{
										GameObject.Find ("Main Camera").GetComponent<controllerScript> ().wordTyping=loc;
									}
								}
							}
						}
						else if(!Input.GetKeyDown (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))
						{
							//Debug.Log("RESET STRING 1");
							ResetString (false);
						}

						
					}
				}
				//This checks to see if the character string should reset to its original value due to a typo
				else if(checkedChar>0 && Input.anyKeyDown)
				{
					//Makes sure left shift (by itself) doesnt reset char string
					if(!Input.GetKeyDown (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))
					{
						//Debug.Log("RESET STRING 2");
						ResetString (true);
					}
				}
			}
			else if(mode==1 && fired == false) //If it is in tap mode and leap is connected
			{
				if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected() == true)
				{
					if(loc==0) //Checks to see if this is the first character. 
					{
						if(doOnceMaterial==false)
							gameObject.GetComponent<MeshRenderer> ().material = asteroidMaterial (val);
						//  (Needs to know 1 of 3 things. 1. If the correct finger is being used. 2. If the wrong finger is being used. 3. If no finger is being used. OPTIONAL: If the finger is close to the correct finger)
						int returnVal=GameObject.Find ("GestureController").GetComponent<gestures> ().KeyTap(val);
						if(returnVal==2)
						{
							 //Creates a projectile object which will move towards this character
							GameObject projectile = new GameObject ();

							projectile.AddComponent<projectileScript> ();
							//Special case for / because unity cant find the object by name ie /
							if(val=="/")
							{
								gameObject.name="slash";
								projectile.GetComponent<projectileScript> ().target="slash";
								val="slash";
							}
							else
								projectile.GetComponent<projectileScript> ().target = val;
							fired=true;
							removeCharacter();
							audio.PlayOneShot(hit, 1f);
						}
						else if(returnVal==1)
						{
							GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed++;
							GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = true;
							Invoke ("PopUp", 0.5f);
							audio.PlayOneShot(miss, 1f);
						}
					}
				}
			}
			//This checks to see if the character string should reset to its original value due to a typo
			else if(checkedChar>0 && Input.anyKeyDown)
			{
				//Makes sure left shift (by itself) doesnt reset char string
				if(!Input.GetKeyDown (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))
				{
					//Debug.Log("RESET STRING 3");
					ResetString (false);
				}
			}
			
			

			if(mode==0)//Special condition for leap mode so asteroids will move past the camera
				//Makes sure the object is looking at the camera
				gameObject.transform.LookAt (Camera.main.transform.position); 
			if(mode==0)
			{
				//Uses the start location to determine what direction to move for keyboard mode
				if (start.x < 0)
					gameObject.transform.position += -transform.right * Time.deltaTime * 1.5f;
				else if (start.x > 0)
					gameObject.transform.position += transform.right * Time.deltaTime * 1.5f;
			}
			else if (mode == 1 || mode==2) //Tap/leap mode
			{
				gameObject.transform.position += transform.forward * 0.0027f; //0.0045
			}
			aliveTime-= Time.deltaTime;
			//Despawn if criteria has been met
			if ((aliveTime<=0 && GetComponent<Renderer>().isVisible==false) || (mode != 0 && gameObject.transform.position.z < 3)) 
			{
				
				if (GameObject.Find ("Projectile :" + val) != null) {
					Destroy (GameObject.Find ("Projectile :" + val));
				}
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed++;
				GameObject.Find ("MissedPopup").GetComponentInChildren<Canvas>().enabled = true;
				Invoke ("PopUp", 0.5f);
				if(mode==1 || mode==0)
				{
					destroyCharacterString ();
					removeCharacter();
				}
				if(mode==2)
				{
					destroy3CharacterString ();
					//Once collision between the projectile and object is met, it will instantiate the explosion prefab.
					Object explosionObj = Instantiate(explosion, transform.position, transform.rotation);
					
					//This destroys the created prefab after 2 seconds, freeing up resources
					Destroy (explosionObj,3.0f);
					//Makes sure that the data structure are updated to reflect the destruction of the 3 asteroids
					if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().rowCount == 3)
					{
						remove3Character();
					}
				}
			}	
		}
	}
	//Makes string go back to default value if typo is detected. Also resets the currentCharTyping string
	void ResetString(bool addNewChar)
	{
		removeCharacter();
		checkedChar=-1;
		NextLetter(checkedChar,addNewChar);
		//GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping = "-1";
		text.GetComponent<Text>().text=val;
		//GameObject.Find ("Main Camera").GetComponent<controllerScript> ().wordTyping=-1;
		gameObject.GetComponent<MeshRenderer> ().material = asteroidMaterial (val[checkedChar].ToString());
	}
	void NextLetter(int currLetter, bool addNewChar)
	{
		//Change to the next char and check case
		checkedChar++;
		//Work around for Input.GetKeyDown not working with chars and ToString wouldnt work
		checkChar = "";
		checkChar+=val[checkedChar];
		if(addNewChar==true)
			GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList.Add(checkChar);
		//used to check if current char is upper or lowercase
		charUpper=false;
		isNonAlpha=false;
		if (checkChar == checkChar.ToUpper()) 
		{
			charUpper=true;
			//Sets the correct charUpper for non alphanumeric characters and checks if the nonAlpha is uppercase. Ie {}:"<>?
			if(checkChar == "[" || checkChar == "]" || checkChar == ";" || checkChar == "'" || checkChar == "," || checkChar == "." || checkChar == "/")
			{
				charUpper=false;
				isNonAlpha=true;
				isNonAlphaUpper=false;
			}
			else if (checkChar == "{" || checkChar == "}" || checkChar == ":" || checkChar == "\"" || checkChar == "<" || checkChar == ">" || checkChar == "?")
			{
				charUpper=false;
				isNonAlpha=true;
				isNonAlphaUpper=true;
			}
		}


	}
	void OnTriggerEnter(Collider other) {
		//If the object is hit by the projectile it will update the score and destroy itself.
		if (other.name == "Projectile :"+val) {
			GameObject.Find ("Main Camera").GetComponent<controllerScript> ().score += 1;
            GameObject.Find("Main Camera").GetComponent<controllerScript>().wordsTyped++;
            GameObject.Find("Main Camera").GetComponent<controllerScript>().wordsForLevel++;
            if (mode==0 ||mode==1 )
			{
				destroyCharacterString();
				removeCharacter();
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().rowCount++;
			}
			else if(mode==2 && GameObject.Find ("Main Camera").GetComponent<controllerScript> ().rowCount<=3)
			{
				destroy3CharacterString();

				if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().rowCount == 3)
					remove3Character();
			}
			//if(mode==0 || mode==2)
			{
				//Once collision between the projectile and object is met, it will instantiate the explosion prefab.
				Object explosionObj = Instantiate(explosion, transform.position, transform.rotation);

				//This destroys the created prefab after 2 seconds, freeing up resources
				Destroy (explosionObj,3.0f);
			}
		}
	}
	void destroyCharacterString() //Called whenever the string is destroyed. Ie hit by projectile or moves into the kill zone.
	{
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.RemoveAt (loc);
		for(int i=loc;i<GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.Count;i++)
		{
			//Updates the location of each character
			GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[i].location--;
			GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[i].charObj.GetComponent<charScript>().loc--;

			//Finds the character object in the list and removes itself from it OLD
			//if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[i].chars==val)
			//	GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.RemoveAt(i);
		}
		Destroy (this.gameObject);
	}
	void removeCharacter() //Removes the current character from the active characterlist. Could probably be made redundant at some point. Although it is useful for finding missed keys
	{
		for(int i=0;i<GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList.Count;i++)
		{
			//Finds the character object in the list and removes itself from it
			if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList[i]==checkChar)
			{
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList.RemoveAt(i);
				break;
			}
		}
	}
	void remove3Character() //Removes the asteroids in the same row when they have all been destroyed
	{
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.RemoveAt(2);
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.RemoveAt(1);
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.RemoveAt(0);
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().rowCount = 0;

        //Change material to the correct colour
        if (GameObject.Find("Main Camera").GetComponent<controllerScript>().characterList.Count >=3)
        {
            GameObject.Find("Main Camera").GetComponent<controllerScript>().characterList[0].charObj.GetComponent<MeshRenderer>().material = asteroidMaterial(GameObject.Find("Main Camera").GetComponent<controllerScript>().characterList[0].charObj.GetComponent<charScript>().val[0].ToString());
            GameObject.Find("Main Camera").GetComponent<controllerScript>().characterList[1].charObj.GetComponent<MeshRenderer>().material = asteroidMaterial(GameObject.Find("Main Camera").GetComponent<controllerScript>().characterList[1].charObj.GetComponent<charScript>().val[0].ToString());
            GameObject.Find("Main Camera").GetComponent<controllerScript>().characterList[2].charObj.GetComponent<MeshRenderer>().material = asteroidMaterial(GameObject.Find("Main Camera").GetComponent<controllerScript>().characterList[2].charObj.GetComponent<charScript>().val[0].ToString());
        }
     }
	void destroy3CharacterString() //Called whenever the string is destroyed. Ie hit by projectile or moves into the kill zone.
	{
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().rowCount == 2) 
		{

			for (int i=3; i<GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.Count; i++)
			{
				//Updates the location of each character
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList [i].location -= 3;
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList [i].charObj.GetComponent<charScript> ().loc -= 3;
			}
		} 
		//else
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().rowCount++;
		Destroy (this.gameObject);
        //Destroy(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[0].charObj);
        //Destroy(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[1].charObj);
        //Destroy(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[2].charObj);
    }
	Material asteroidMaterial(string val) //make 'spawn' for projectileScript
	{
		val = val.ToLower ();
		if (val == "q" || val == "a" || val == "z") {
			//Debug.Log ("In RED");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialRed;
		}
		else if (val == "w" || val == "s" || val == "x") {
			//Debug.Log ("In ORANGE");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialOrange;
		}
		else if (val == "e" || val == "d" || val == "c") {
			//Debug.Log ("In YELLOW");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialYellow;
		}
		else if (val == "r" || val == "f" || val == "v" || val == "t" || val == "g" || val == "b") {
			//Debug.Log ("In PURPLE");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialPurple;
		}
		else if (val == "y" || val == "h" || val == "n" || val == "u" || val == "j" || val == "m") {
			//Debug.Log ("In BLUE");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialBlue;
		}
		else if (val == "i" || val == "k" || val == "," || val == "<") {
			//Debug.Log ("In CYAN");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialCyan;
		}
		else if (val == "o" || val == "l" || val == "." || val == ">") {
			//Debug.Log ("In GREEN");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialGreen;
		}
		else if (val == "p" || val == ";" || val == "[" || val == "\'" || val == "]" || val == "{" || val == "}" || val == ":" || val == "\"" || val == "/" || val == "?") {
			//Debug.Log ("In BROWN");
			return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterialBrown;
		}
		//Debug.Log ("In DEFAULT");
		return GameObject.Find ("Main Camera").GetComponent<controllerScript> ().asteroidMaterial;
	}
}