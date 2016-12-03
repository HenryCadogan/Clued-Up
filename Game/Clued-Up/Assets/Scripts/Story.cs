using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// The stroy object for the game to run around
/// </summary>
public class Story : MonoBehaviour {
	/// <summary>
	/// Makes the object persistant throughout scenes
	/// </summary>
	public static Story Instance;
	/// <summary>
	/// The weather index // 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy. 0 set for debug
	/// </summary>
	private int weather = 2;
	/// <summary>
	/// The detective.
	/// int set by user in character selection
	/// </summary>
	private int detective; 

	/// <summary>
	/// The list of characters in room.
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
	/// The alive characters.
	/// </summary>
	private List<GameObject> aliveCharacters;
	/// <summary>
	/// The list of clues.
	/// </summary>
	private List<GameObject> clues;


	/// <summary>
	/// The number of room that will be in the instance of the game.
	/// </summary>
	private static int NUMBER_OF_ROOMS = 8;
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()  
	//Makes the object persistant throughout scenes
	{
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
	/// <returns>The line from.</returns>
	/// <param name="filename">Filename.</param>
	private string randomLineFrom(string filename){
		//returns a random line from specified file in resources
		StreamReader stream = new StreamReader(filename);
		List<string> lines = new List<string> ();

		while(!stream.EndOfStream){
			lines.Add(stream.ReadLine());
		}
		stream.Close( );
		return lines [Random.Range (0, lines.Count)];
	}

	/// <summary>
	/// Gets the intro 1.
	/// </summary>
	/// <returns>The intro1.</returns>
	public string getIntro1(){
		//returns first sentence of introduction
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
	/// Gets the intro 2.
	/// </summary>
	/// <returns>The intro2.</returns>
	public string getIntro2(){
		return randomLineFrom("Assets/TextFiles/intro2.txt");
	}
	/// <summary>
	/// Gets the intro 3.
	/// </summary>
	/// <returns>The intro3.</returns>
	public string getIntro3(){
		return randomLineFrom ("Assets/TextFiles/intro3.txt");
	}
	/// <summary>
	/// Sets the weather.
	/// </summary>
	/// <returns>The weather.</returns>
	/// <param name="ma">Ma.</param>
	public int setWeather(Material[] ma){
		//sets weather condition to random int within range of array of all possible weather conditions, and returns this int.
		weather = Random.Range (0, ma.Length);
		return weather;
	}

	/// <summary>
	/// Gets the weather.
	/// </summary>
	/// <returns>The weather.</returns>
	public int getWeather(){
		return weather;
	}
	/// <summary>
	/// Sets the detective.
	/// </summary>
	/// <param name="detectiveInt">Detective int.</param>
	public void setDetective(int detectiveInt){
		//sets detective to int chosen by user in Character Selection
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
	/// Sets the characters.
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
	/// Sets the character rooms.
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
	/// Gets the characters in room.
	/// </summary>
	/// <returns>The characters in room.</returns>
	/// <param name="room">Room.</param>
	public List<GameObject> getCharactersInRoom(int room){
		//returns list of all characters in given room
		List<GameObject> charactersInRoom = new List<GameObject> (); 
		if (this.charactersInRoom.ContainsKey (room)) {	//protects againnst invalid index exception
			foreach (int characterIndex in this.charactersInRoom[room]) {
				charactersInRoom.Add (this.aliveCharacters [characterIndex]);
			}
		}
		return charactersInRoom;

	}

	/// <summary>
	/// Checks if the accused character is the murderer
	/// </summary>
	/// <returns><c>true</c>, if murderer was ised, <c>false</c> otherwise.</returns>
	/// <param name="accused">Accused.</param>
	public bool isMurderer(GameObject accused){
		return this.murderer == accused;
	}

	/// <summary>
	/// Gets the victim.
	/// </summary>
	/// <returns>The victim.</returns>
	public Character getVictim(){
		return this.victim.GetComponent<Character>();
	}

	/// <summary>
	/// Sets the story.
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