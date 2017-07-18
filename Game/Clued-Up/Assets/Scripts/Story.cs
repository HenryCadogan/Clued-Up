using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

/// <summary>
/// Persistant story class that stores all game/ global variables and initialises Clues and Character objects
/// </summary>
public class Story :  MonoBehaviour{
    /// <summary>
    /// Variable to make sure there is only one instance of this object
    /// </summary>
    public static Story Instance;
    /// <summary>
    /// Whether the story has been loaded yet.
    /// </summary>
    public static bool loaded = false;
	/// <summary>
	/// Index of weather conditions where 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy. Preset for debugging different condtitions
	/// </summary>
	private WeatherOption weather;
	/// <summary>
	/// The detective int set by user in character selection
	/// </summary>
	private int detective; 
	/// <summary>
	/// Dictionary to store which character Indexs are stored in each room index
	/// </summary>
	public Dictionary<int, List<int>> charactersInRoom = new Dictionary<int,List<int>>();
	/// <summary>
	/// The murder weapon for this instance of the story
	/// </summary>
	public string murderWeapon;
	/// <summary>
	/// The motive clue for this instance.
	/// </summary>
	public string motiveClue;
	/// <summary>
	/// The murderer in this instance..
	/// </summary>
	public GameObject murderer;
	/// <summary>
	/// The victim for this instance of the story
	/// </summary>
	public GameObject victim;
	/// <summary>
	/// Subset of all characters, ones that are not the victim
	/// </summary>
	private List<GameObject> aliveCharacters;
	/// <summary>
	/// Dictionary of list of clue names present in each room in this instance of the game.
	/// </summary>
	public Dictionary<int, List<string>> cluesInRoom = new Dictionary<int, List<string>>();
	/// <summary>
	/// The number of rooms that will be in the game
	/// </summary>
	private static int NUMBER_OF_ROOMS = 8;
	/// <summary>
	/// The number of detectives that will be in the game
	/// </summary>
	private static int NUMBER_OF_DETECTIVES = 3;
    /// <summary>
    /// The full list of characters for the game.
    /// </summary>
    private List<GameObject> characters = new List<GameObject> (); 
    /// <summary>
    /// The number of possible weather options (currently sunny, rainy, sunset, snow)
    /// </summary>
    public enum WeatherOption
    {
        SUN, RAIN, SUNSET, SNOW
    };
    /// <summary>
    /// The private variablr score for the detective. Increases for every conversation and by more 
    /// if you get information from them or find out where a clue is
    /// </summary>
    private int score;
    /// <summary>
    /// The public setter and getter for the score of the player
    /// </summary>
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            GameObject.Find("HUD").GetComponent<HUDController>().Scoretext.text = "Score: " + value;
        }
    }

	/// <summary>
	/// Keeps only 1 instance ever, therefore it can survive between scenes without having several scenes.
	/// </summary>
	void Awake () {
        Debug.Log("Story Awake called");
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		
		}
		else if (Instance != this)
		{
            return;
		}
	}

    /// <summary>
    /// Is the story loaded or not?
    /// </summary>
    /// <returns>true or false</returns>
    public static bool isLoaded()
    {
        Debug.Log("Story#isLoaded returning " + loaded);
        return loaded;
    }

	/// <summary>
	/// Gets a random line from a file
	/// </summary>
	/// <returns>A line from the file (string)</returns>
	/// <param name="filename">Path to file (from root)</param>
	private string randomLineFrom(string filename){
		string[] linesArray = Regex.Split (Resources.Load<TextAsset> (filename).text, "\n");
		//testing to ensure filename is correct
		if (linesArray.Length == 0) {
			throw new FileLoadException ("FileName \'" + filename + "\' is not a valid file to read from");
		}
		List<string> lines = new List<string> (linesArray);
		return lines [UnityEngine.Random.Range (0, lines.Count)];
	}

	/// <summary>
	/// Selects a random first introductory paragraph
	/// </summary>
	/// <returns>1st introductory sentence (string)</returns>
	public string getIntro1(){
		string weatherString;
		switch (weather) {
		case WeatherOption.SUN:
			weatherString = "beautiful, sunny afternoon";
			break;
		case WeatherOption.RAIN:
			weatherString = "wet stormy night";
			break;
		case WeatherOption.SUNSET:
			weatherString = "warm autumn's sunset";
			break;
		case WeatherOption.SNOW:
			weatherString = "cold, snowy evening";
			break;
		default:
			throw new System.ArgumentOutOfRangeException ("Weather out of range");
		}
		string intro1 = "It was a " + weatherString + randomLineFrom("TextFiles/intro1");
		return intro1;
	}

	/// <summary>
	/// Selects a random introductory paragraph from relevent file
	/// </summary>
	/// <returns>2nd introductory sentence (string)</returns>
	/// <param name="introIndex">Index of the intro file to read a line from</param>
	public string getIntro(int introIndex){
		return randomLineFrom("TextFiles/intro" + introIndex.ToString());
	}
	/// <summary>
	/// Sets the weather for the rest of the game
	/// </summary>
	/// <returns>Weather index</returns>
	/// <param name="materialArray">array of background materials; one each weather condition</param>
	public WeatherOption setWeather(){
        Array values = Enum.GetValues(typeof(WeatherOption));
        weather = (WeatherOption)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		return weather;
	}
	/// <summary>
	/// Gets weather type
	/// </summary>
	/// <returns>The weather type</returns>
	public WeatherOption getWeather(){
		return weather;
	}
	/// <summary>
	/// Sets the detective.
	/// </summary>
	/// <param name="detectiveInt">Index for the detective to set, passed from when the corresponding button is pressed in character selection</param>
	public void setDetective(int detectiveInt){
		if ((detectiveInt < NUMBER_OF_DETECTIVES) || (detectiveInt >= 0)){
			detective = detectiveInt;
			//Debug.Log ("You have chosen dectective " + detective.ToString ());
		} else
			throw new System.ArgumentOutOfRangeException ("Not enough detectives");
	}
	/// <summary>
	/// Gets the detective.
	/// </summary>
	/// <returns>The detective.</returns>
	public int getDetective(){
		return this.detective;
	}

	public void endGame(){
		Debug.Log ("Congratulations! You have beaten the game!");
		SceneManager.LoadScene ("GameOver");
	}
		

	/// <summary>
	/// Gets written translation of trait index.
	/// </summary>
	/// <returns>The trait string.</returns>
	/// <param name="traitIndex">Trait index.</param>
	public string getTraitString(int traitIndex){
		switch(traitIndex){
		case 0:
			return "Aggressive";
		case 1:
			return "Annoying";
		case 2:
			return "Calm";
		case 3:
			return "Comedic";
		case 4:
			return "Friendly";
		case 5:
			return "Manipulative";
		case 6:
			return "Rude";
		default:
			throw new System.IndexOutOfRangeException ("Trait index out of range");
		}
	}
	/// <summary>
	/// Initial definitions for all characters and selection of victim/murderer
	/// </summary>
	private void setCharacters(){
		

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

		GameObject character6 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character6.GetComponent<Character>().initialise(6);

		GameObject character7 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character7.GetComponent<Character>().initialise(7);

		characters.Add (character0);
		characters.Add (character1);
		characters.Add (character2);
		characters.Add (character3);
		characters.Add (character4);
		characters.Add (character5);
		characters.Add (character6);
		characters.Add (character7);

		this.victim = characters[UnityEngine.Random.Range(0,characters.Count)];
		characters.Remove(this.victim);
		this.murderer = characters[UnityEngine.Random.Range(0,characters.Count)];
		this.aliveCharacters = characters;

		this.victim.GetComponent<Character> ().isVictim = true;
		this.murderer.GetComponent<Character> ().isMurderer = true;
	}
	/// <summary>
	/// Gets information relevent to a character name.
	/// </summary>
	/// <returns>The character information in Character for characterName.</returns>
	/// <param name="name">Name of character object.</param>
	public Character getCharacterInformation(string name){
		if (victim.name == name) {
			return victim.GetComponent<Character> ();
		} else {
			foreach (GameObject character in aliveCharacters) {
				if (character.name == name) {
					return character.GetComponent<Character> ();
				}
			}
		}
		throw new System.ArgumentException ("No character has name " + name);
	}
	/// <summary>
	/// Decides which of the alive characters occupy each room, excluding the initial crime scene room.
	/// </summary>
	public void setCharacterRooms(){
        Debug.LogFormat("setCharacterRooms called. There are {0} alive characters.", aliveCharacters.Count);
		int randomRoom;
		List<int> emptyRooms = new List<int>();

		for (int roomI = 1; roomI <  NUMBER_OF_ROOMS ; roomI++) { //initialise all rooms to have character index -1, and adding all rooms to emptyRooms
			this.charactersInRoom.Add (roomI, new List<int>{-1});
			emptyRooms.Add (roomI);
		}

		//for each character, assign a random room that isnt the crime scene & provided there isnt anyone in it, add it to characters in room
		for(int characterIndex = 0; characterIndex < this.aliveCharacters.Count; characterIndex ++) {
			randomRoom = emptyRooms[UnityEngine.Random.Range (0, emptyRooms.Count)];
            Debug.LogFormat("setCharacterRooms: Placing character {0} in room {1}", characterIndex, randomRoom);
			emptyRooms.Remove (randomRoom);
			charactersInRoom[randomRoom] = new List<int>{characterIndex}; //CharactersInRoom[1] = 2 | char 2 is in room 1 //using lists so it is adaptable to multiple characters being in one room
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
		//print ("Get characters in room " + room);
		List<GameObject> charactersInCurrentRoom = new List<GameObject> (); 
        List<string> namesInCurrentRoom = new List<string>();
		if (this.charactersInRoom.ContainsKey (room)) {
			foreach (int characterIndex in this.charactersInRoom[room]) {
				if (characterIndex != -1) {	//where -1 is default
                    GameObject character = this.aliveCharacters[characterIndex];
					charactersInCurrentRoom.Add (character);
                    namesInCurrentRoom.Add(character.name);
				}
			}
		} else if(room!= 0)
			throw new System.IndexOutOfRangeException ("Room out of range");

        Debug.LogFormat("getCharactersInRoom({0}) returning: {1}", room, string.Join(",", namesInCurrentRoom.ToArray()));
		return charactersInCurrentRoom;
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
	/// Gets the murderer character game object
	/// </summary>
	/// <returns>The murderer object.</returns>
	public Character getMurderer(){
		return this.murderer.GetComponent<Character>();
	}

	/// <summary>
	/// Sets the location of each of the clues.
	/// </summary>
	/// <param name="clueNames">List of names of all clues in game</param>
	private void setClueLocations(List<string> clueNames){
		cluesInRoom [0] = new List<string>{ clueNames [0] }; //body clue always in room 0
        Debug.LogFormat("Spawning clue {0} in room {1}", clueNames[0], 0);
		int randomRoom;
		for(int clueIndex = 1; clueIndex < clueNames.Count; clueIndex ++) {	
			randomRoom = UnityEngine.Random.Range (1, NUMBER_OF_ROOMS); //rooms 1-7 (i.e. all, not including the crime scene 
			while(cluesInRoom.ContainsKey(randomRoom)){	//while there is already a character here, keep finsing new room
				randomRoom = UnityEngine.Random.Range (1, NUMBER_OF_ROOMS);
			}
            Debug.LogFormat("Spawning clue {0} in room {1}", clueNames[clueIndex], randomRoom);
			cluesInRoom.Add (randomRoom, new List<string>{clueNames[clueIndex]}); //cluesInRoom[0] = ["chalkOutline"] | using lists so it is adaptable to multiople chars in one room
		}
	}

    public Clue getClueByName(string name)
    {
        Debug.Log("getClueByName called for " + name);
        GameObject go = GameObject.Find(name);
        if (go == null)
        {
            throw new Exception("getClueByName: couldn't find game object with name: " + name);
        }

        Clue clue = go.GetComponent<Clue>();
        if (clue == null)
        {
            throw new Exception("getClueByName: found gameobject but it doesn't have a clue component.");
        }

        return clue;
    }

	/// <summary>
	/// Decides which character 5 clues to use (2 from victim, 3 from murderer), such that (VictimClues ∩ CharacterClues) = ∅
	/// </summary>
	private void setCharacterClues(List<string> cluesList){
		List<string> murdererClueNames = murderer.GetComponent<Character>().getRandomCharacterClueNames (3, cluesList);
		foreach (string clueName in murdererClueNames)
			cluesList.Add (clueName);
		List<string> victimClueNames = victim.GetComponent<Character>().getRandomCharacterClueNames (2, cluesList);
		foreach (string clueName in victimClueNames)
			cluesList.Add (clueName);
	}
	/// <summary>
	/// Selects one of many motive clues.
	/// </summary>
	/// <param name="cluesList">Clues list.</param>
	private void setMotiveClue(List<string> cluesList){
		List<string> motiveClueNames = new List<string> ();
		motiveClueNames.Add ("diary");
		motiveClueNames.Add ("recorder");
		motiveClueNames.Add ("polaroid");
		motiveClueNames.Add ("letter");
		motiveClue = motiveClueNames [UnityEngine.Random.Range (0, motiveClueNames.Count)];
		cluesList.Add(motiveClue);
	}
	/// <summary>
	/// Selects one of many weapons.
	/// </summary>
	/// <param name="cluesList">Clues list.</param>
	private void setWeaponClue(List<string> cluesList){
		List<string> weaponClueNames = new List<string> ();
		weaponClueNames.Add ("gun");
		weaponClueNames.Add ("salmon");
		weaponClueNames.Add ("hammer");
		weaponClueNames.Add ("knife");
		murderWeapon = weaponClueNames [UnityEngine.Random.Range (0, weaponClueNames.Count)];
		cluesList.Add(murderWeapon);
	}


	/// <summary>
	/// Decides which clues will be used in this game and assigns them a room. Stored as a dictionary[RoomIndex] = [clue1name,clue2name...]
	/// Clue 0 = chalkOutline
	/// Clue 1,2,3,4,5 = characterClues
	/// Clue 6 = motiveClue
	/// Clue 7 = setWeapon
	/// </summary>
	private void setClues(){
        Debug.Log("setClues called");
		List<string> cluesList = new List<string> (); 
		cluesList.Add("chalkOutline"); //Clue 0 will ALWAYS be the chalk outline.
		setCharacterClues (cluesList);
		setMotiveClue(cluesList);
		setWeaponClue(cluesList);
		setClueLocations(cluesList);

		foreach (string clueName in cluesList)
			print (clueName);
	}
	/// <summary>
	/// Instanciates all the clues in a particular room
	/// </summary>
	/// <returns>The clue GameObjects in the room</returns>
	/// <param name="roomIndex">Index of the room to find clues for</param>
	public List<GameObject> getCluesInRoom(int roomIndex){
        Debug.LogFormat("getCluesInRoom called for room {0}", roomIndex);
        if (!this.cluesInRoom.ContainsKey(roomIndex))
        {
            throw new Exception(String.Format("cluesInRoom does not contain key for room: {0}, there are {1} rooms in cluesInRoom",
                                              roomIndex, cluesInRoom.Count));
        }

        Debug.LogFormat("There are {0} clues in this room", this.cluesInRoom[roomIndex].Count);

		List<GameObject> clues = new List<GameObject> ();
		foreach (string clueName in this.cluesInRoom[roomIndex]) {	//for each clueName in room
            Debug.LogFormat("Instantiating clue: {0}", clueName);

			GameObject newClue = Instantiate (Resources.Load ("Clue"), new Vector3 (1f, 1f, 1f), Quaternion.Euler (0, 0, 0)) as GameObject;
            Clue clueInfo = newClue.GetComponent<Clue>(); //sets up temp Clue that stores all properties
            clueInfo.initialise(clueName);

            // Add Clue's sprite to the Clue's GameObject sprite renderer
			newClue.GetComponent<SpriteRenderer> ().sprite = newClue.GetComponent<Clue> ().sprite;	
			clues.Add (newClue);
		}
		return clues;
	}

	/// <summary>
	/// Initialises story characters and clues
	/// </summary>
	public void setStory(){
        setWeather();
		setCharacters ();
		setCharacterRooms();
		setClues();
        loaded = true;
	}

    /// <summary>
    /// Return s a list of gameobjects that are characters
    /// </summary>
    public List<GameObject> getFullCharacterList(){
		return characters;
	}

    /// <summary>
    /// Random alive characters
    /// </summary>
    public Character randomAliveCharacter(){
		Character tempCharacter;
		foreach (GameObject character in  aliveCharacters){
			print(character.name);
		}
		do {
			tempCharacter = aliveCharacters [UnityEngine.Random.Range (0, aliveCharacters.Count)].GetComponent<Character> ();
		
		} while(tempCharacter.isMurderer == true);
		return tempCharacter;
	}
					


}