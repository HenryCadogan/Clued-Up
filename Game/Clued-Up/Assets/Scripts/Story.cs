using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
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
	private int weather = 1;
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
	public string MurderWeapon;
	/// <summary>
	/// The motive clue for this instance.
	/// </summary>
	public string MotiveClue;
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
	/// Keeps only 1 instance ever, therefore it can survive between scenes without having several scenes.
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
		string[] linesArray = Regex.Split (Resources.Load<TextAsset> (filename).text, "\n");
		List<string> lines = new List<string> (linesArray);
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

		string intro1 = "It was a " + weatherString + randomLineFrom("TextFiles/intro1");
		return intro1;
	}

	/// <summary>
	/// Selects a random introductory paragraph from relevent file
	/// </summary>
	/// <returns>2nd introductory sentence (string)</returns>
	/// <param name="introIndex">Index of the intro file to read a line from</param>
	public string getIntro(int introIndex){
		return randomLineFrom("TextFiles/intro"+introIndex.ToString());
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
		if ((detectiveInt < NUMBER_OF_DETECTIVES) || (detectiveInt >= 0)){
			detective = detectiveInt;
			Debug.Log ("You have chosen dectective " + detective.ToString ());
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

	public void EndGame(){
		Debug.Log ("Congratulations! You have beaten the game!");
		SceneManager.LoadScene (11);
		//TODO: End cutscene.
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
		int randomRoom;
		List<int> emptyRooms = new List<int>();

		for (int roomI = 1; roomI <  NUMBER_OF_ROOMS ; roomI++) { //initialise all rooms to have character index -1, and adding all rooms to emptyRooms
			this.charactersInRoom.Add (roomI, new List<int>{-1});
			emptyRooms.Add (roomI);
		}

		//for each character, assign a random room that isnt the crime scene & provided there isnt anyone in it, add it to characters in room
		for(int characterIndex = 0; characterIndex < this.aliveCharacters.Count; characterIndex ++) {	
			randomRoom = emptyRooms[Random.Range (0, emptyRooms.Count)];
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
		print ("Get characters in room " + room);
		List<GameObject> charactersInCurrentRoom = new List<GameObject> (); 
		if (this.charactersInRoom.ContainsKey (room)) {
			foreach (int characterIndex in this.charactersInRoom[room]) {
				if (characterIndex != -1) {	//where -1 is default
					charactersInCurrentRoom.Add (this.aliveCharacters [characterIndex]);
				}
			}
		} else if(room!= 0)
			throw new System.IndexOutOfRangeException ("Room out of range");
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
			cluesInRoom.Add (randomRoom, new List<string>{clueNames[clueIndex]}); //cluesInRoom[0] = ["chalkOutline"] | using lists so it is adaptable to multiople chars in one room
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
		clueName = clueName.Trim ();
		switch (clueName) {
		case "chalkOutline":
			newClue.GetComponent<Clue> ().initialise ("chalkOutline", "Chalk Outline", "A chalk outline of the body of " + this.getVictim ().longName + "!", disappearWhenClicked:false);
			break;
		case "microphone":
			newClue.GetComponent<Clue> ().initialise ("microphone", "Microphone", "Someone wants to make themselves heard");
			break;
		case "wizzardHat":
			newClue.GetComponent<Clue> ().initialise ("wizzardHat", "Wizzard's Hat", "Looks like part of a Halloween costume");
			break;
		case "moustache":
			newClue.GetComponent<Clue> ().initialise ("moustache", "Moustache", "Is someone trying to disguise themselves?");
			break;
		case "pen":
			newClue.GetComponent<Clue> ().initialise ("pen", "Pen", "A fancy fountain pen.");
			break;
		case "plunger":
			newClue.GetComponent<Clue> ().initialise ("plunger", "Plunger", "Oh wow. A plunger!");
			break;
		case "brownHair":
			newClue.GetComponent<Clue> ().initialise ("brownHair", "Brown Hair", "Whose hair is this colour?");
			break;
		case "sandwich":
			newClue.GetComponent<Clue> ().initialise ("sandwich", "Sandwich", " A cheese sandwich. It’s half eaten. Delicious.");
			break;
		case "stapler":
			newClue.GetComponent<Clue> ().initialise ("stapler", "Stapler", "Staples things.");
			break;
		case "suitcase":
			newClue.GetComponent<Clue> ().initialise ("suitcase", "Suitcase", "Looks like someone’s forgot their suitcase. Oh no.");
			break;
		case "wand":
			newClue.GetComponent<Clue> ().initialise ("wand", "Wand", "Casts spells and things.");
			break;
		case "sunglasses":
			newClue.GetComponent<Clue> ().initialise ("sunglasses", "Sunglasses", "Why would you need sunglasses in York? Unless someone is trying to conceal their identity...");
			break;
		case "whistle":
			newClue.GetComponent<Clue> ().initialise ("whistle", "Whistle", "It must belong to the train conductor...I wonder why it's here...");
			break;
		case "whiteHair":
				newClue.GetComponent<Clue> ().initialise ("whiteHair", "White Hair", "Whose hair is this colour?");
			break;
		case "money":
			newClue.GetComponent<Clue> ().initialise ("money", "Money", "Someone's been careless...or they've got too much money...", localScale:0.15f);
			break;
		case "tape":
			newClue.GetComponent<Clue> ().initialise ("tape", "Tape", "It's sticky.", localScale:0.1f);
			break;
		case "bling":
			newClue.GetComponent<Clue> ().initialise ("bling", "Bling", "What kind of person would wear this around their neck?");
			break;
		case "ladder":
			newClue.GetComponent<Clue> ().initialise ("ladder", "Ladder", "Oops. Someone dropped their ladder.");
			break;
		case "ticket":
			newClue.GetComponent<Clue> ().initialise ("ticket", "Ticket", "A train ticket for use on the train.", localScale:0.1f);
			break;
		case "coal":
			newClue.GetComponent<Clue> ().initialise ("coal", "Coal", "A lump of coal.");
			break;
		case "cloak":
			newClue.GetComponent<Clue> ().initialise ("cloak", "Cloak", "Someone is trying to conceal themselves...");
			break;
		case "blondHair":
			newClue.GetComponent<Clue> ().initialise ("blondHair", "Blond Hair", "Whose hair is this colour?");
			break;
		case "chefHat":
			newClue.GetComponent<Clue> ().initialise ("chefHat", "Chef's Hat", "I wonder who this belongs to...");
			break;
		case "comb":
			newClue.GetComponent<Clue> ().initialise ("comb", "Comb", "I wonder who would carry a comb with them...");
			break;
		case "monocle":
			newClue.GetComponent<Clue> ().initialise ("monocle", "Monocle", "This must belong to someone important.");
			break;
		case "feather":
			newClue.GetComponent<Clue> ().initialise ("feather", "Feather", "Where could this feather have come from?");
			break;
		case "lighter":
			newClue.GetComponent<Clue> ().initialise ("lighter", "Lighter", "Somoeone dropped their lighter. Oops.");
			break;
		case "blackHair":
			newClue.GetComponent<Clue> ().initialise ("blackHair", "Black Hair", "Whose hair is this colour?");
			break;
		
		
		
		case "hammer":
			newClue.GetComponent<Clue> ().initialise ("hammer", "Hammer", "A bloody hammer.", true);
			break;
		case "gun":
			newClue.GetComponent<Clue> ().initialise ("gun", "Gun", "A bloody gun.", true);
			break;
		case "knife":
			newClue.GetComponent<Clue> ().initialise ("knife", "Knife", "A bloody knife.", true);
			break;
		case "salmon":
			newClue.GetComponent<Clue> ().initialise ("salmon", "Salmon", "A bloody salmon that has been handled under suspicious circumstances.", true);
			break;



		case "polaroid":
			newClue.GetComponent<Clue> ().initialise ("polaroid", "Polaroid", "A dirty, crumpled polaroid. In the frame are " + this.murderer.GetComponent<Character>().longName + " and " +this.victim.GetComponent<Character>().longName+". They have their arms around one another, and they’re laughing. You remember seeing them both at the party, but they didn’t seem to talk to one another at all. In fact, they seemed to be at opposite sides of the room for the entirety of the evening. You can’t help but feel like something must have happened between them...but what?", isMotive:true);
			break;
		case "recorder":
			newClue.GetComponent<Clue> ().initialise ("recorder", "Recorder", "The quality is terrible, but you can just about make out the voices. It appears to be two people having a conversation. You recognise both of them. One of them is " +this.victim.GetComponent<Character>().longName + " the other is " + this.murderer.GetComponent<Character>().longName +". At first they are calm, and you can’t quite hear what they’re saying. But gradually it gets louder, their voices rising as their anger bubbles up. Someone starts shouting. There’s a loud bang. Everything goes quiet. There seems to be some muffled shuffling in the background. The recording cuts off. Everything is starting to make sense...", isMotive:true);
			break;
		case "diary":
			newClue.GetComponent<Clue> ().initialise ("diary", "Diary", this.victim.GetComponent<Character>().longName + "’s diary. You know you shouldn’t be reading it, but you find yourself flicking through the pages anyway. After all, it could be used for evidence, right? You turn to the most recent entry. The day before the party. You can’t help noticing how many times" + this.murderer.GetComponent<Character>().longName + "'s name has been mentioned. You read the entire page before slamming the book shut. Everything is starting to make sense...", isMotive:true, localScale:0.15f);
			break;
		case "letter":
			newClue.GetComponent<Clue> ().initialise ("letter", "Letter", "A crumpled letter. The writing is small and scrawling, but if you squint you can just make it out. It appears to have been written by " + this.murderer.GetComponent<Character>().longName + ", and it’s addressed to " + this.victim.GetComponent<Character>().longName + ". It’s messy and smudged, so you don’t attempt to read the entire thing, but you can’t help but notice the very last sentence... ‘Your days are numbered.’ Everything is starting to make sense...", isMotive:true, localScale:0.2f);
			break;



		default:
			throw new System.ArgumentException ("'"+ clueName + "' is not a valid Clue Name");
		}
		return newClue;
	}
	/// <summary>
	/// Decides which character 5 clues to use (2 from victim, 3 from murderer) 
	/// </summary>
	private void setCharacterClues(List<string> cluesList){
		List<string> victimClueNames = victim.GetComponent<Character>().getRandomCharacterClueNames (2);
		List<string> murdererClueNames = murderer.GetComponent<Character>().getRandomCharacterClueNames (3);
		foreach (string clueName in victimClueNames)
			cluesList.Add (clueName);
		foreach (string clueName in murdererClueNames)
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
		MotiveClue = motiveClueNames [Random.Range (0, motiveClueNames.Count)];
		cluesList.Add(MotiveClue);
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
		MurderWeapon = weaponClueNames [Random.Range (0, weaponClueNames.Count)];
		cluesList.Add(MurderWeapon);
	}


	/// <summary>
	/// Decides which clues will be used in this game and assigns them a room. Stored as a dictionary[RoomIndex] = [clue1name,clue2name...]
	/// Clue 0 = chalkOutline
	/// Clue 1,2,3,4,5 = characterClues
	/// Clue 6 = motiveClue
	/// Clue 7 = setWeapon
	/// </summary>
	private void setClues(){
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
	/// Sets the clue initialisation parameters for newClue, based on clueName.
	/// </summary>
	/// <param name="clueName">Name of clue to get information for.</param>
	/// <param name="newClue">Clue GameObject to initialise</param>
	private void setClueInformation(string clueName, GameObject newClue){
		Clue clueInfo = getClueInformation (clueName).GetComponent<Clue>(); //sets up temp Clue that stores all properties
		newClue.GetComponent<Clue>().initialise(clueInfo.name,clueInfo.longName, clueInfo.description, clueInfo.isWeapon, clueInfo.isMotive, clueInfo.disappearWhenClicked, clueInfo.transform.localScale.x);
		//GameObject.Destroy (clueInfo.gameObject); //destroys temp Clue GameObject to remove it from scene TODO: removed for debugging - toby
	}
	/// <summary>
	/// Instanciates all the clues in a particular room
	/// </summary>
	/// <returns>The clue GameObjects in the room</returns>
	/// <param name="roomIndex">Index of the room to find clues for</param>
	public List<GameObject> getCluesInRoom(int roomIndex){
		List<GameObject> clues = new List<GameObject> ();
		foreach (string clueName in this.cluesInRoom[roomIndex]) {	//for each clueName in room
			GameObject newClue = Instantiate (Resources.Load ("Clue"), new Vector3 (1f, 1f, 1f), Quaternion.Euler (0, 0, 0)) as GameObject;
			setClueInformation (clueName, newClue); // calls the initialisation method with the relevent details
			newClue.GetComponent<SpriteRenderer> ().sprite = newClue.GetComponent<Clue> ().sprite;	// Add Clue's sprite to the Clue's GameObject sprite renderer
			clues.Add (newClue);
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