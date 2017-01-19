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

	void Awake ()
	{
		SpeechUI = FindObjectOfType<SpeechHandler> ();
		SpeechRef = GetComponent<ImportSpeech> ();
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
		this.isMurderer = false;
		this.isVictim = false;

		//gameObject.GetComponent<Renderer> ().enabled = false; //dont draw any characters yet

		//TODO set up character clues
}
}
