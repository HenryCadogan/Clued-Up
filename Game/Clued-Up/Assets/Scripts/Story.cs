using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Story : MonoBehaviour {
	public static Story Instance;//Makes the object persistant throughout scenes

	private int weather = 2; // 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy. 0 set for debug
	private int detective; // int set by user in character selection
	private Dictionary<int, int> charactersInRoom = new Dictionary<int,int>();
	private int murderer;
	private int victim;
	private List<GameObject> aliveCharacters;

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

	public string getIntro2(){
		return randomLineFrom("Assets/TextFiles/intro2.txt");
	}

	public string getIntro3(){
		return randomLineFrom ("Assets/TextFiles/intro3.txt");
	}
		
	public int setWeather(Material[] ma){
		//sets weather condition to random int within range of array of all possible weather conditions, and returns this int.
		weather = Random.Range (0, ma.Length);
		return weather;
	}

	public int getWeather(){
		return weather;
	}

	public void setDetective(int detectiveInt){
		//sets detective to int chosen by user in Character Selection
		detective = detectiveInt;
		Debug.Log ("You have chosen dectective " + detective.ToString ());
	}

	public int getDetective(){
		return this.detective;
	}
		
	private void setCharacters(){
		List<GameObject> characters = new List<GameObject> ();

		//set up all characters
		//GameObject character0 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		//character0.GetComponent<Character>().initialise("character0", "Character 0");

		GameObject character1 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character1.GetComponent<Character>().initialise("character1", "Character 1");

		GameObject character2 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		character2.GetComponent<Character>().initialise("character2", "Character 2");

		Debug.Log (character2.name);

		//GameObject character3 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		//character3.GetComponent<Character>().initialise("character3", "Character 3");

		//GameObject character4 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		//character4.GetComponent<Character>().initialise("character4", "Character 4");

		//GameObject character5 = Instantiate (Resources.Load ("Character"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		//character5.GetComponent<Character>().initialise("character5", "Character 5");

		//characters.Add (character0);
		characters.Add (character1);
		characters.Add (character2);
		//characters.Add (character3);
		//characters.Add (character4);
		//characters.Add (character5);

		//this.victim = characters[Random.Range(0,characters.Count)];
		//characters.Remove(this.victim);
		//this.murderer = characters[Random.Range(0,characters.Count)];
		this.aliveCharacters = characters;
	}
		
	private void setCharacterRooms(){
		Debug.Log ("Setting Character Rooms");
		List<GameObject> unassignedCharacters = this.aliveCharacters;
		int randomRoom;
		int characterIndex = 0;
		for(characterIndex = 0; characterIndex < this.aliveCharacters.Count; characterIndex ++) {	
			randomRoom = Random.Range (2, 9); //rooms 2-8 (not including the crime scene which does not include 
			while(charactersInRoom.ContainsKey(randomRoom)){	//while there isnt already a character there
				randomRoom = Random.Range (1, 9);
			}
			charactersInRoom.Add (randomRoom, characterIndex);
			unassignedCharacters [characterIndex].GetComponent<Character>().location = randomRoom;
		}

		foreach (KeyValuePair<int,int> room in charactersInRoom) {
			Debug.Log ("charactersInRoom["+room.Key.ToString()+"] = " + room.Value.ToString());
		}

	}


	public void setStory(){
		setCharacters ();
		//setCharacterRooms();
	}
}