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

	// Use this for initialization
	void Start () {
		aliveTime = 12f;
		gameObject.transform.position = GameObject.Find ("Main Camera").transform.position;
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

		//Modify Asteroid Scale
		gameObject.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
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

		//Makes character look at camera
		gameObject.transform.LookAt(Camera.main.transform.position); 
		fired = false;
		//Initialises checkedChar to the first character location (of Val)
		checkedChar = -1;

		NextLetter(checkedChar);

		reset = false;
		gameObject.transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {

		//Checks if game is paused
		if (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().paused == false ) 
		{
			//Checks if the the key pressed meets conditions to be considered for checking. Ie A button has been pressed, the word is not completed and the current typing char is empty or already set to the current char
			if ((Input.anyKeyDown) && fired == false && (GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping=="-1" || GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping==val))
			{

				int findFinger=GameObject.Find ("Main Camera").GetComponent<controllerScript> ().finger.IsPressed(val[checkedChar].ToString());
				if(findFinger==2 || GameObject.Find ("Main Camera").GetComponent<controllerScript> ().con.LeapConnected() == false)
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
							//Debug.Log("STANDARD LETTER CHECKING METHOD CHECKED");
							foundLetter=true;
						}
					}
					//Checks if the char is the first char of the string and that there is string currently being typed and sets the the current typed string to this objects value
					if(checkedChar==0 && val.Length!=1 && GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping=="-1")
						GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping=val;

					//Checks to make sure the case is correct and that the character pressed is correct. It double checks uppercase nonAlpha characters, probably should fix at some point but it works atm.
					if((((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && charUpper==true && foundLetter==true )|| (charUpper==false && !Input.GetKey (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))&& isNonAlpha==false && foundLetter==true)|| (continueOn==true && ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && isNonAlphaUpper==true || isNonAlphaUpper==false && !Input.GetKey (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))))
					{

						//Checks if the character pressed was the last char and spawns a projectile to kill it
						if(checkedChar+1==val.Length)
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
							GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping="-1";
						}
						//Picks the next char and updates components to reflect that
						else
						{


							//Removes current character from charList. So it is once again a non missable character
							removeCharacter();
							//Gets the next char in the string by passing it the index of the current char
							NextLetter(checkedChar);

							//Change character/mesh colour 
							//apply small explosion / shake effect 

							//Updates displayed text
							text.GetComponent<Text>().text="";
							for(int i=checkedChar;i<val.Length;i++)
							{
								text.GetComponent<Text>().text+=val[i];
							}
						}
					}
					else if(!Input.GetKeyDown (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))
						ResetString ();

				}
			}
			//This checks to see if the character string should reset to its original value due to a typo
			else if(checkedChar>0 && Input.anyKeyDown)
			{
				//Makes sure left shift (by itself) doesnt reset char string
				if(!Input.GetKeyDown (KeyCode.LeftShift) &&!Input.GetKey (KeyCode.RightShift))
					ResetString ();
			}



			//Makes sure the object is looking at the camera
			gameObject.transform.LookAt (Camera.main.transform.position); 
			//Uses the start location to determine what direction to move
			if (start.x < 0)
				gameObject.transform.position += -transform.right * Time.deltaTime * 1.5f;
			else if (start.x > 0)
				gameObject.transform.position += transform.right * Time.deltaTime * 1.5f;
			aliveTime-= Time.deltaTime;

			if (aliveTime<=0 && GetComponent<Renderer>().isVisible==false) {

				if (GameObject.Find ("Projectile :" + val) != null) {
					Destroy (GameObject.Find ("Projectile :" + val));
				}
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().missed++;
				destroyCharacterString ();
				removeCharacter();
			}	
		}
	}
	//Makes string go back to default value if typo is detected. Also resets the currentCharTyping string
	void ResetString()
	{
		removeCharacter();
		checkedChar=-1;
		NextLetter(checkedChar);
		text.GetComponent<Text>().text=val;
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().currentCharTyping="-1";	
	}
	void NextLetter(int currLetter)
	{
		//Change to the next char and check case
		checkedChar++;
		//Work around for Input.GetKeyDown not working with chars and ToString wouldnt work
		checkChar = "";
		checkChar+=val[checkedChar];
		
		GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList.Add(checkChar);
		//used to check if current char is upper or lowercase
		charUpper=false;
		isNonAlpha=false;
		string testChar="{";
		if (checkChar == checkChar.ToUpper()) {
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
			destroyCharacterString();
			removeCharacter();

		}
	}
	void destroyCharacterString() //Called whenever the string is destroyed. Ie hit by projectile or moves into the kill zone.
	{
		for(int i=0;i<GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.Count;i++)
		{
			//Finds the character object in the list and removes itself from it
			if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList[i].chars==val)
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().characterList.RemoveAt(i);
		}
		Destroy (this.gameObject);
	}
	void removeCharacter() //Removes the current character from the active characterlist. Could probably be made redundant at some point. Although it is useful for finding missed keys
	{
		for(int i=0;i<GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList.Count;i++)
		{
			//Finds the character object in the list and removes itself from it
			if(GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList[i]==checkChar)
				GameObject.Find ("Main Camera").GetComponent<controllerScript> ().charList.RemoveAt(i);
		}
	}
}