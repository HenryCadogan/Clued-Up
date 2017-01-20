using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// Character class
/// </summary>
public class Character : MonoBehaviour {
	/// <summary>
	/// Name to be used when displaying a name (i.e. contains spaces)
	/// </summary>
	public string longName;
	/// <summary>
	/// Description to be used by the notebook.
	/// </summary>
	public string description;
	/// <summary>
	/// Image to be used by notebook.
	/// </summary>
	public Sprite image;
	/// <summary>
	/// If character is the murderer. Set by Story.
	/// </summary>
	public bool isMurderer;
	/// <summary>
	/// If character is the victim. Set by Story.
	/// </summary>
	public bool isVictim;
	/// <summary>
	/// List of Clues attatched to this character instance
	/// </summary>
	public List<Clue> characterClues;
	/// <summary>
	/// Makes the object persistant throughout scenes
	/// </summary>
	public SpeechHandler SpeechUI;
	public string CritBranch;
	public TextAsset SpeechFile;
	public bool HasBeenTalkedTo = false;
	public bool CanBeTalkedTo = true;
	private ImportSpeech SpeechRef;
	private BoxCollider BoxCol;

	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}

	void OnMouseDown(){
		SpeechUI.TurnOnSpeechUI ();
	}

	public void PreSpeech (string BranchName){
		Story ActiveStory = FindObjectOfType<Story> ();
		if (BranchName == "INTRO") {
			SpeechRef.CharIn = ActiveStory.getVictim ();
		}
		BoxCol.enabled = false;
	}

	public void PostSpeech (string BranchName){
		if (BranchName == CritBranch) {
			//TODO: Give item
		} else if (BranchName == "Accuse-NoItems" || BranchName == "Accuse-WrongChar"
		           || BranchName == "Accuse-Motive" || BranchName == "Accuse-Weapon") {
			CanBeTalkedTo = false;
		} else if (BranchName == "Accuse-Right") {
			Story ActiveStory = FindObjectOfType<Story> ();
			ActiveStory.EndGame ();
		}
		BoxCol.enabled = true;
	}
	/// <summary>
	/// Loads model at specific location as child of character and adds animation controller. If name is Kanye append either 0 or 1 to end, as there are different annimation options.
	/// </summary>
	/// <param name="position">Position.</param>
	public void display(Vector3 position){
		string modelName = this.name;
		if (modelName == "Kanye")
			modelName += Random.Range (0, 2).ToString ();
		
		GameObject model = Instantiate (Resources.Load<GameObject> ("Models/" + modelName));
		model.transform.parent = gameObject.transform;
		Vector3 pos = new Vector3();
		pos.x = position.x;
		pos.y = position.y;
		pos.z = position.z; //moves onto ground
		model.transform.position = pos;
		model.transform.Rotate (new Vector3 (0f, 180f, 0f)); //rotate to face camera

		BoxCollider BoxCol = GetComponent<BoxCollider> ();
		BoxCol.center = model.transform.localPosition + new Vector3 (0, 0, -3);
		BoxCol.size = new Vector3 (3, 1, 6);
		BoxCol.enabled = true;


		if (modelName != "Reginald")
			model.GetComponent<Animator>().runtimeAnimatorController = (Resources.Load<RuntimeAnimatorController> ("Models/" + modelName + "Anim"));
	}

	/// <summary>
	/// Gets a certain amount of character clues.
	/// </summary>
	/// <returns>The random character clue names.</returns>
	/// <param name="count">Count; number of clues needed.</param>
	public List<string> getRandomCharacterClueNames(int count){
		List<string> randomCharacterClues = new List<string>();
		int randInt;
		while (count > 0) {
			randInt = Random.Range (0, this.characterClues.Count);
			if (!randomCharacterClues.Contains (this.characterClues [randInt].name)) {
				randomCharacterClues.Add (this.characterClues [randInt].name);
				count -= 1;
			}
		}
		return randomCharacterClues;
	}

	/// <summary>
	/// Uses character text file clueNames to validate clue, and then returns a list of all clues for the character. 
	/// </summary>
	/// <returns>The character clues.</returns>
	/// <param name="characterClues">Character clues.</param>
	/// <param name="lines">Lines from character file (after name and description) </param>
	private List<Clue> setCharacterClues(List<Clue> characterClues, List<string> lines){
		Story story = GameObject.Find ("Story").GetComponent<Story> ();
		List<Clue> characterClueList = new List<Clue> ();
		foreach (string clueName in lines) {
			characterClueList.Add (story.getClueInformation (clueName).GetComponent<Clue> ());
		}

		return characterClueList;
	}


	/// <summary>
	/// Initialise the specified Character with properties and CharacterClues
	/// </summary>
	///<param name="characterIndex">Index of character to be initialised</param>
	public void initialise(int characterIndex){
		StreamReader stream = new StreamReader("Assets/TextFiles/character" + characterIndex.ToString() + ".txt");
		List<string> lines = new List<string> ();
		while(!stream.EndOfStream){
			lines.Add(stream.ReadLine());
		}
		stream.Close( );

		this.gameObject.name = lines[1]; //file contains comment in line 0
		this.longName = lines [2];
		this.description = lines [3];
		this.isMurderer = false;
		this.isVictim = false;
		print ("CharacterImages/" + this.gameObject.name);
		this.image = Resources.Load<Sprite> ("CharacterImages/" + this.gameObject.name);

		gameObject.AddComponent<ImportSpeech> ();
		ImportSpeech SpeechHandler = GetComponent<ImportSpeech> ();
		TextAsset TestAsset =(TextAsset)Resources.Load (lines [1]);
		SpeechHandler.asset = TestAsset;
		SpeechHandler.ActualStart ();
		SpeechRef = GetComponent<ImportSpeech> ();

		gameObject.AddComponent<BoxCollider> ();
		BoxCol = GetComponent<BoxCollider> ();
		BoxCol.enabled = false;
		this.characterClues = setCharacterClues(characterClues, lines.GetRange(4,lines.Count-4)); //uses all lines from character text file after name/description to get character clues
}

	/// <summary>
	/// Destroys the model of the character if there is one, each time a new room is loaded
	/// </summary>
	///<param name="characterIndex">Index of character to be initialised</param>
	void OnLevelWasLoaded(){
		foreach (Transform child in gameObject.transform) {
			GameObject.Destroy (child.gameObject);
			BoxCol.enabled = false;
		}
	}
}
