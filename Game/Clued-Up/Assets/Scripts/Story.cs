using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Persistant story class that stores all game/ global variables and initialises Clues and Character objects
/// </summary>
public class Story : MonoBehaviour {
	/// <summary>
	/// Variable to make sure there is only one instance of this object
	/// </summary>
	public static Story Instance;
	/// <summary>
	/// Index of weather conditions where 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy. Preset for debugging different condtitions
	/// </summary>
	private int weather = 3;
	/// <summary>
	/// The detective int set by user in character selection
	/// </summary>
	private int detective; 
	/// <summary>
	/// Dictionary to store which character Indexs are stored in each room index
	/// </summary>
	public Dictionary<int, List<int>> charactersInRoom = new Dictionary<int,List<int>>();
	/// <summary>
	/// The murderer for this instance of the story
	/// </summary>
	private GameObject murderer;
	/// <summary>
	/// The victim for this instance of the story
	/// </summary>
	private GameObject victim;
	/// <summary>
	/// Subset of all characters, ones that are not the victim
	/// </summary>
	private List<GameObject> aliveCharacters;
	/// <summary>
	/// Dictionary of list of clue names present in each room in this instance of the game.
	/// </summary>
	public Dictionary<int, List<string>> cluesInRoom = new Dictionary<int, List<string>>();
	/// <summary>
	/// The number of room that will be in the game
	/// </summary>
	private static int NUMBER_OF_ROOMS = 8;
	/// <summary>
	/// Makes the object persistant throughout scenes.
	/// </summary>
	void Awake () {
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}

	/// <summary>
	/// Gets a random line from a file
	/// </summary>
	/// <returns>A line from the file (string)</returns>
	/// <param name="filename">Path to file (from root)</param>
	private string randomLineFrom(string filename){
		StreamReader stream = new StreamReader(filename);
		List<string> lines = new List<string> ();

		while(!stream.EndOfStream){
			lines.Add(stream.ReadLine());
		}
		stream.Close( );
		return lines [Random.Range (0, lines.Count)];
	}

	/// <summary>
	/// Selects a random first introductory paragraph
	/// </summary>
	/// <returns>1st introductory sentence (string)</returns>
	public string getIntro1(){
		string weatherString;
		switch (weather) {
		case 0:
			weatherString = "beautiful, sunny afternoon";
			break;
		case 1:
			weatherString = "wet stormy night";
			break;
		case 2:
			weatherString = "warm autumn's sunset";
			break;
		case 3:
			weatherString = "cold, snowy evening";
			break;
		default:
			throw new System.ArgumentOutOfRangeException ("Weather out of range");
		}

		string intro1 = "It was a " + weatherString + randomLineFrom("Assets/TextFiles/intro1.txt");
		return intro1;
	}

	/// <summary>
	/// Selects a random introductory paragraph from relevent file
	/// </summary>
	/// <returns>2nd introductory sentence (string)</returns>
	/// <param name="introIndex">Index of the intro file to read a line from</param>
	public string getIntro(int introIndex){
		return randomLineFrom("Assets/TextFiles/intro"+introIndex.ToString()+".txt");
	}
	/// <summary>
	/// Sets the weather for the rest of the game
	/// </summary>
	/// <returns>Weather index</returns>
	/// <param name="materialArray">array of background materials; one each weather condition</param>
	public int setWeather(Material[] materialArray){
		weather = Random.Range (0, materialArray.Length);
		return weather;
	}
	/// <summary>
	/// Gets weather index
	/// </summary>
	/// <returns>The weather index</returns>
	public int getWeather(){
		return weather;
	}
	/// <summary>
	/// Sets the detective.
	/// </summary>
	/// <param name="detectiveInt">Index for the detective to set, passed from when the corresponding button is pressed in character selection</param>
	public void setDetective(int detectiveInt){
		detective = detectiveInt;
		Debug.Log ("You have chosen dectective " + detective.ToString ());
	}
	/// <summary>
	/// Gets the detective.
	/// </summary>
	/// <returns>The detective.</returns>
	public int getDetective(){
		return this.detective;
	}
		
	/// <summary>
	/// Initial definitions for all characters and selection of victim/murderer
	/// </summary>
	private void setCharacters(){
		List<GameObject> characters = new List<GameObject> ();

		//Set up characters
		GameObject character0 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character0.GetComponent<Character>().initialise(0);

		GameObject character1 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character1.GetComponent<Character>().initialise(1);

		GameObject character2 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character2.GetComponent<Character>().initialise(2);

		GameObject character3 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character3.GetComponent<Character>().initialise(3);

		GameObject character4 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character4.GetComponent<Character>().initialise(4);

		GameObject character5 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character5.GetComponent<Character>().initialise(5);

		characters.Add (character0);
		characters.Add (character1);
		characters.Add (character2);
		characters.Add (character3);
		characters.Add (character4);
		characters.Add (character5);

		this.victim = characters[Random.Range(0,characters.Count)];
		characters.Remove(this.victim);
		this.murderer = characters[Random.Range(0,characters.Count)];
		this.aliveCharacters = characters;

		this.victim.GetComponent<Character> ().isVictim = true;
		this.murderer.GetComponent<Character> ().isMurderer = true;
	}
	/// <summary>
	/// Decides which of the alive characters occupy each room, excluding the initial crime scene room.
	/// </summary>
	private void setCharacterRooms(){
		int randomRoom;
		for(int characterIndex = 0; characterIndex < this.aliveCharacters.Count + 1; characterIndex ++) {	
			randomRoom = Random.Range (1, NUMBER_OF_ROOMS); //rooms 1-7 (i.e. all, not including the crime scene  (room 0)
			while(charactersInRoom.ContainsKey(randomRoom)){	//while there isnt already a character there
				randomRoom = Random.Range (1, NUMBER_OF_ROOMS);
			}

			charactersInRoom.Add (randomRoom, new List<int>{characterIndex}); //CharactersInRoom[1] = 2 | char 2 is in room 1 //using lists so it is adaptable to multiople chars in one room

		}

		foreach (KeyValuePair<int,List<int>> room in charactersInRoom) {
			Debug.Log ("charactersInRoom["+room.Key.ToString()+"] = " + room.Value[0].ToString());

		}
	}
	/// <summary>
	/// Get list of the characters positioned within room index
	/// </summary>
	/// <returns>List of character GameObjects that are within room index</returns>
	/// <param name="room">Index of the room of choice</param>
	public List<GameObject> getCharactersInRoom(int room){
		List<GameObject> charactersInRoom = new List<GameObject> (); 
		if (this.charactersInRoom.ContainsKey (room)) {	//protects againnst invalid index exception
			foreach (int characterIndex in this.charactersInRoom[room]) {
				charactersInRoom.Add (this.aliveCharacters [characterIndex]);
			}
		}
		return charactersInRoom;
	}
	/// <summary>
	/// Checks if a particular character is the murderer
	/// </summary>
	/// <returns><c>true</c>, if accused character GameObject is the murderer, <c>false</c> otherwise.</returns>
	/// <param name="accused">character GameObject</param>
	public bool isMurderer(GameObject accused){
		return this.murderer == accused;
	}
	/// <summary>
	/// Gets the victim character GameObject
	/// </summary>
	/// <returns>The victim character GameObject</returns>
	public Character getVictim(){
		return this.victim.GetComponent<Character>();
	}
	/// <summary>
	/// Sets the location of each of the clues.
	/// </summary>
	/// <param name="clueNames">List of names of all clues in game</param>
	private void setClueLocations(List<string> clueNames){
		cluesInRoom [0] = new List<string>{ clueNames [0] }; //body clue always in room 0

		int randomRoom;
		for(int clueIndex = 1; clueIndex < clueNames.Count; clueIndex ++) {	
			randomRoom = Random.Range (1, NUMBER_OF_ROOMS); //rooms 1-7 (i.e. all, not including the crime scene 
			while(cluesInRoom.ContainsKey(randomRoom)){	//while there is already a character here, keep finsing new room
				randomRoom = Random.Range (1, NUMBER_OF_ROOMS);
			}
			cluesInRoom.Add (randomRoom, new List<string>{clueNames[clueIndex]}); //cluesInRoom[0] = ["bodyClue"] | using lists so it is adaptable to multiople chars in one room
		}
	}
	/// <summary>
	/// Gets the information associated with a Clue, and where all Clues are initially defined
	/// </summary>
	/// <returns>The clue information as a GameObect with Clue component</returns>
	/// <param name="clueName">Name of the clue to lookup</param>
	public GameObject getClueInformation(string clueName){
		GameObject newClue = new GameObject ();
		newClue.AddComponent<Clue> ();
		switch (clueName) {
		case "bodyClue":
			newClue.GetComponent<Clue> ().initialise ("chalkOutline", "Chalk Outline", "A chalk outline of the body of " + this.getVictim ().longName + "!!");
			break;
		default:
			break;
		}
		return newClue;
	}
	/// <summary>
	/// Decides which clues will be used in this game and assigns them a room. Stored as a dictionary[RoomIndex] = [clue1name,clue2name...]
	/// </summary>
	private void setClues(){
		List<string> cluesList = new List<string> (); 
		cluesList.Add("bodyClue"); //Clue 0 will ALWAYS be the chalk outline.
		//setCharacterClues ();
		//setMotive();
		//setWeapon();
		setClueLocations(cluesList);
	}
	/// <summary>
	/// Sets the clue initialisation parameters for newClue, based on clueName.
	/// </summary>
	/// <param name="clueName">Name of clue to get information for.</param>
	/// <param name="newClue">Clue GameObject to initialise</param>
	private void setClueInformation(string clueName, GameObject newClue){
		Clue clueInfo = getClueInformation (clueName).GetComponent<Clue>(); //sets up temp Clue that stores all properties
		newClue.GetComponent<Clue>().initialise(clueInfo.name,clueInfo.longName, clueInfo.description, clueInfo.isWeapon, clueInfo.isMotive, clueInfo.disappearWhenClicked);
		GameObject.Destroy (clueInfo.gameObject); //destroys temp Clue GameObject to remove it from scene
	}
	/// <summary>
	/// Instanciates all the clues in a particular room
	/// </summary>
	/// <returns>The clue GameObjects in the room</returns>
	/// <param name="roomIndex">Index of the room to find clues for</param>
	public List<GameObject> getCluesInRoom(int roomIndex){
		List<GameObject> clues = new List<GameObject> ();
		foreach (string clueName in this.cluesInRoom[roomIndex]) {	//for each clueName in room
			GameObject newClue = Instantiate (Resources.Load ("Clue"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
			setClueInformation (clueName, newClue); // calls the initialisation method with the relevent details
			newClue.GetComponent<SpriteRenderer> ().sprite = newClue.GetComponent<Clue>().sprite;	// Add Clue's sprite to the Clue's GameObject sprite renderer
			clues.Add(newClue);
		}
		return clues;
	}

	/// <summary>
	/// Initialises story characters and clues
	/// </summary>
	public void setStory(){
		setCharacters ();
		setCharacterRooms();
		setClues();
	}
}