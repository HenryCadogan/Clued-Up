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
	private int weather = 2;
	/// <summary>
	/// The detective int set by user in character selection
	/// </summary>
	private int detective; 
	/// <summary>
	/// Dictionary to store which character Indexs are stored in each room index
	/// </summary>
	private Dictionary<int, List<int>> charactersInRoom = new Dictionary<int,List<int>>(); //characters in rooms stored as list of indexes for each roomID
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
	/// The list of clues availabe in this instance of the game.
	/// </summary>
	private List<GameObject> clues;
	/// <summary>
	/// The number of room that will be in the game.
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
	/// Selects a random second introductory paragraph
	/// </summary>
	/// <returns>2nd introductory sentence (string)</returns>
	public string getIntro2(){
		return randomLineFrom("Assets/TextFiles/intro2.txt");
	}
	/// <summary>
	/// Selects a random third introductory paragraph
	/// </summary>
	/// <returns>3rd introductory sentence (string)</returns>
	public string getIntro3(){
		return randomLineFrom ("Assets/TextFiles/intro3.txt");
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
		character0.GetComponent<Character>().initialise("character0", "Character 0");

		GameObject character1 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character1.GetComponent<Character>().initialise("character1", "Character 1");

		GameObject character2 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character2.GetComponent<Character>().initialise("character2", "Character 2");

		GameObject character3 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character3.GetComponent<Character>().initialise("character3", "Character 3");

		GameObject character4 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character4.GetComponent<Character>().initialise("character4", "Character 4");

		GameObject character5 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character5.GetComponent<Character>().initialise("character5", "Character 5");

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
		for(int characterIndex = 0; characterIndex < this.aliveCharacters.Count; characterIndex ++) {	
			randomRoom = Random.Range (2, NUMBER_OF_ROOMS + 1); //rooms 2-8 (i.e. all, not including the crime scene 
			while(charactersInRoom.ContainsKey(randomRoom)){	//while there isnt already a character there
				randomRoom = Random.Range (2, NUMBER_OF_ROOMS + 1);
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
	/// Initialises story characters and clues
	/// </summary>
	public void setStory(){
		setCharacters ();
		setCharacterRooms();
		//setCharacterClues ();
		//setMotiveClue();
		//setWeapon();
		//setClueLocations();
	}
}