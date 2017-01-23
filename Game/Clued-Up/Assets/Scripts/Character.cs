using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
	public List<string> characterClueNames;
	/// <summary>
	/// The Speech UI handler in the current scene.
	/// </summary>
	public SpeechHandler speechUI;
	public bool entered;


	//TODO NEEDS SUMMARIES.
	/// <summary>
	/// The name of the branch that the will result in the detective recieveing an item.
	/// </summary>
	public string critBranch;
	/// <summary>
	/// The text asset that contains the speech of the character
	/// </summary>
	public TextAsset speechFile;
	/// <summary>
	/// A flag that triggers when the character has been talked to.
	/// </summary>
	public bool hasBeenTalkedTo = false;
	/// <summary>
	/// The state of whether the character can be talked to or not.
	/// </summary>
	public bool canBeTalkedTo = true;
	/// <summary>
	/// A reference to the ImportSpeech component of the character.
	/// </summary>
	private ImportSpeech speechRef;
	/// <summary>
	/// A reference to the BoxCollider component of the character.
	/// </summary>
	private BoxCollider boxCol;

	/// <summary>
	/// Makes the object persistant throughout scenes
	/// </summary>
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}

	void OnMouseDown(){
		Inventory inventory = GameObject.Find ("Detective").GetComponent<Inventory> ();
		inventory.encounter (this);
		speechUI.turnOnSpeechUI ();
	}

	public void preSpeech (string branchName){
		Story activeStory = FindObjectOfType<Story> ();
		if (branchName == "INTRO") {
			speechRef.charIn = activeStory.getVictim ();
		}
	}

	public void postSpeech (string branchName){
		if (branchName == critBranch) {
			//TODO: Give item
		} else if (branchName == "Accuse-NoItems" || branchName == "Accuse-WrongChar"
		           || branchName == "Accuse-Motive" || branchName == "Accuse-Weapon") {
			canBeTalkedTo = false;
		} else if (branchName == "Accuse-Right") {
			Story activeStory = FindObjectOfType<Story> ();
			activeStory.endGame ();
		}
	}
	/// <summary>
	/// Loads model at specific location as child of character and adds animation controller. If name is Kanye append either 0 or 1 to end, as there are different annimation options.
	/// </summary>
	/// <param name="position">Position.</param>
	public void display(Vector3 position){
		string modelName = this.name.Trim();
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

		BoxCollider boxCol = GetComponent<BoxCollider> ();
		boxCol.center = model.transform.localPosition + new Vector3 (0, 0, -3);
		boxCol.size = new Vector3 (3, 1, 6);
		boxCol.enabled = true;


		if (modelName != "Reginald")
			model.GetComponent<Animator>().runtimeAnimatorController = (Resources.Load<RuntimeAnimatorController> ("Models/" + modelName + "Anim"));
	}

	/// <summary>
	/// Gets a certain amount of character clues.
	/// 
	/// </summary>
	/// <returns>The random character clue names.</returns>
	/// <param name="count">Count; number of clues needed.</param>
	public List<string> getRandomCharacterClueNames(int count, List<string> clueNames){
		List<string> randomCharacterClues = new List<string>();
		int randInt;
		while (count > 0) {
			randInt = Random.Range (0, this.characterClueNames.Count);
			if ((!randomCharacterClues.Contains (this.characterClueNames [randInt])) && (!clueNames.Contains(this.characterClueNames[randInt]))) {
				//If that index hasnt already been selected, and the clue is not already in the total clues list
				randomCharacterClues.Add (this.characterClueNames [randInt]);
				count -= 1;
			}
		}
		return randomCharacterClues;
	}
	/// <summary>
	/// Initialise the specified Character with properties and CharacterClues
	/// </summary>
	///<param name="characterIndex">Index of character to be initialised</param>
	public void initialise(int characterIndex){
		string[] linesArray = Regex.Split(Resources.Load<TextAsset>("TextFiles/character" + characterIndex.ToString()).text, "\n");
		List<string> lines = new List<string> (linesArray);

		this.gameObject.name = lines[1]; //file contains comment in line 0
		this.longName = lines [2];
		this.description = lines [3];
		this.isMurderer = false;
		this.isVictim = false;
		this.image = Resources.Load<Sprite> ("CharacterImages/" + this.gameObject.name.Trim());

		gameObject.AddComponent<ImportSpeech> ();
		ImportSpeech speechHandler = GetComponent<ImportSpeech> ();
		TextAsset testAsset =(TextAsset)Resources.Load (lines [1].Trim());
		print("Asset = " + lines [1].Trim());
		speechHandler.asset = testAsset;
		speechHandler.actualStart ();
		speechRef = GetComponent<ImportSpeech> ();

		gameObject.AddComponent<BoxCollider> ();
		boxCol = GetComponent<BoxCollider> ();
		boxCol.enabled = false;
		this.characterClueNames = lines.GetRange(4,lines.Count-4); //all lines after the initial properties are the names of character clues.
	}

	/// <summary>
	/// Destroys the model of the character if there is one, each time a new room is loaded
	/// </summary>
	///<param name="characterIndex">Index of character to be initialised</param>
	void OnLevelWasLoaded(){
		foreach (Transform child in gameObject.transform) {
			GameObject.Destroy (child.gameObject);
			boxCol.enabled = false;
		}
	}





	void OnMouseEnter(){
		Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		entered = true;
	}
	void OnMouseExit(){
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		entered = false;
	}

	void Update(){
		if (entered) {
			Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		}
	}
}
