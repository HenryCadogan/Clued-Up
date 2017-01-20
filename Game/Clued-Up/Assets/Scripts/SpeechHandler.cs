using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// The Speech Handler, which takes the input from the UI and passes relevent bits
/// to the character (and it's ImportSpeech).
/// </summary>
public class SpeechHandler : MonoBehaviour {

	/// <summary>
	/// The Unity Text UI object that the speech should appear on..
	/// </summary>
	public GameObject TextObject;
	/// <summary>
	/// The canvas of the main HUD (i.e. not the one the speech UI is attatched to).
	/// </summary>
	public HUDController HUDCanvas;
	/// <summary>
	/// The world position that the text should appear in when the detective is talking.
	/// </summary>
	public Vector3 DetectivePos;
	/// <summary>
	/// The world position that the text should appear in when the suspect is talking.
	/// </summary>
	public Vector3 SuspectPos;
	/// <summary>
	/// An array of buttons to be linked to the detective's traits.
	/// </summary>
	public Button[] TraitButtonArray;
	/// <summary>
	/// The active detective.
	/// </summary>
	public Detective ActiveDet;
	/// <summary>
	/// The active story.
	/// </summary>
	public Story ActiveStory;
	/// <summary>
	/// An array containing any UI elements that should be hidden when the speech UI is up.
	/// </summary>
	public GameObject[] OtherUIElements;
	/// <summary>
	/// The text object that shows the currently selected clue.
	/// </summary>
	public Text ClueText;
	/// <summary>
	/// Any speech UI elements that should be hidden during the actual conversatsion.
	/// </summary>
	public GameObject[] SpeechUIElements;

	/// <summary>
	/// The button that this script is attatched to.
	/// </summary>
	private Button ParentButton;
	/// <summary>
	/// The text of the text object that shows the speech.
	/// </summary>
	private Text ActualText;
	/// <summary>
	/// The actual speech files attatched to the character in the scene.
	/// </summary>
	private ImportSpeech SpeechRef;
	/// <summary>
	/// The name of the currently/last active branch of discussion.
	/// </summary>
	private string BranchName;
	/// <summary>
	/// A reference to the inventory of the detective.
	/// </summary>
	private Inventory PlayerInv;
	/// <summary>
	/// The character active in the current scene.
	/// </summary>
	private Character CharInScene;
	/// <summary>
	/// The canvas of the Speech UI.
	/// </summary>
	private GameObject ParentCanvas;
	/// <summary>
	/// The inventory index of the currently selected item.
	/// </summary>
	private int ClueIndex = 0;
	private Clue ActiveClue;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start (){
		Waiting ();
		// Assign various variables from objects in scene.
		ActiveDet = FindObjectOfType<Detective> ();
		ActiveStory = FindObjectOfType<Story> ();
		PlayerInv = FindObjectOfType<Inventory> ();
		HUDCanvas = FindObjectOfType<HUDController> ();
		string SceneName = SceneManager.GetActiveScene ().name;
		int RoomNo = (SceneName[SceneName.Length-1]) - '1';
		Debug.Log (RoomNo);
		List<GameObject> ListOfCharsInRoom = ActiveStory.getCharactersInRoom (RoomNo);
		ActualText = TextObject.GetComponentInChildren<Text> ();

		// Try to access the data from the character in the scene.
		try{
			CharInScene = ListOfCharsInRoom [0].GetComponent<Character> ();
			SpeechRef = CharInScene.GetComponentInChildren<ImportSpeech> ();
			CharInScene.SpeechUI = this;

		// This will fail if there is no character in scene,
		// but that's fine because there's no access to the UI then.
		} catch {}

		// A bit more object fetching...
		ParentCanvas = GameObject.Find ("SpeechUI");
		ParentCanvas.SetActive (false);
		gameObject.SetActive(false);
		string[] StrTraits = new string[ActiveDet.traits.Length];

		// Set the values on the trait buttons from the detective traits.
		for (int i = 0; i < StrTraits.Length; i++) {
			StrTraits [i] = ActiveStory.getTraitString (ActiveDet.traits [i]);
			Debug.Log (TraitButtonArray [i].name);
			Text ButtonText = TraitButtonArray [i].GetComponentInChildren<Text> ();
			ButtonText.text = StrTraits [i];
			TraitButtonArray [i].name = StrTraits [i];
		}
	}

	// This just delays the setup for an update to make sure everything is initialised.
	// You could _probably_ get rid of it but it's here for safety.
	IEnumerator Waiting(){
		yield return new WaitForFixedUpdate ();
	}

	/// <summary>
	/// Turns on the Speech UI, pauses the game.
	/// </summary>
	public void TurnOnSpeechUI(){
		if (ParentCanvas.gameObject.activeSelf == false) {
			// If the character is in a state where he can be talked to...
			if (CharInScene.CanBeTalkedTo == true) {
				// Turn off all of the non-speech UI elements...
				foreach (GameObject ForObj in OtherUIElements) {
					ForObj.gameObject.SetActive (false);
				}

				// Then turn on all the speech UI stuff.
				UpdateClueName (PlayerInv.collectedClueNames [ClueIndex]);
				ParentCanvas.SetActive (true);
				HUDCanvas.loadPanelAndPause (ParentCanvas);
				if (CharInScene.HasBeenTalkedTo == false) {
					CharInScene.HasBeenTalkedTo = true;
					StartBranch ("INTRO");
				}

				// Get the collider of the character and turn it off.
				BoxCollider BoxCol = CharInScene.GetComponent<BoxCollider> ();
				BoxCol.enabled = false;
			// If the character can't be talked to, the player will need to find another clue.
			} else {
				Debug.Log ("You need to find another clue");
			}
		// And if the speech UI is already on, we can't turn it doubly on, so do nothing.
		} else {
			Debug.Log ("Tried to turn on speech UI, but was already on");
		}
	}

	/// <summary>
	/// Turns off the speech UI, unpauses the game.
	/// </summary>
	public void TurnOffSpeechUI(){
		if (ParentCanvas.gameObject.activeSelf == true) {
			// Turn on all the non-speech UI elements first.
			foreach (GameObject ForObj in OtherUIElements) {
				ForObj.gameObject.SetActive (true);
			}

			//Then turn off all of the speech UI elements.
			ClueIndex = 0;
			ParentCanvas.SetActive (false);
			HUDCanvas.hidePanelAndResume (ParentCanvas);

			// Re-enable the collider of the character..
			BoxCollider BoxCol = CharInScene.GetComponent<BoxCollider> ();
			BoxCol.enabled = true;
			// If we accused them, then turn it back off.
			try{
				if(BranchName.Substring(0,6) == "Accuse"){
					BoxCol.enabled=false;
				}
			}
			catch{}
		//Obviously, you can't turn off a UI if it's already off, so do nothing if so.
		} else {
			Debug.Log ("Tried to turn off speech UI but it was already off.");
		}
	}

	//Starts a branch from a trait button.
	public void QuestionBranch(Button SendingButton){
		// Set the branch name to be the trait combined with the item dev name.
		BranchName = SendingButton.name;
		SpeechRef.ItemIn = ActiveClue;
		string LongName = BranchName + "-" + PlayerInv.collectedClueNames [ClueIndex];
		// See if the character has a branch for the combination of trait and item.
		try{
			SpeechRef.SetBranch (LongName);
			BranchName = LongName;
		}
		// If they don't just go with the trait-only branch.
		catch{
			SpeechRef.SetBranch(BranchName);
		}
		// Let the character make any required changes.
		CharInScene.PreSpeech (BranchName);
		// Turn on the text and continue button...
		gameObject.SetActive(true);
		TextObject.SetActive(true);
		// And turn off everything else.
		foreach (Button ForButton in TraitButtonArray) {
			ForButton.gameObject.SetActive (false);
		}
		foreach (GameObject ForButton in SpeechUIElements) {
			ForButton.gameObject.SetActive (false);
		}
		// Then display the first line of the speech.
		OnClick ();
	}

	public void StartBranch(string BranchIn){
		BranchName = BranchIn;
		SpeechRef.ItemIn = ActiveClue;
		SpeechRef.SetBranch(BranchName);
		CharInScene.PreSpeech (BranchName);
		gameObject.SetActive(true);
		TextObject.SetActive(true);
		foreach (Button ForButton in TraitButtonArray) {
			ForButton.gameObject.SetActive (false);
		}
		foreach (GameObject ForButton in SpeechUIElements) {
			ForButton.gameObject.SetActive (false);
		}
		OnClick ();
	}

	/// <summary>
	/// Increments the clue index, then updates the displayed clue name.
	/// </summary>
	public void NextClue (){
		ClueIndex += 1;
		if (ClueIndex >= PlayerInv.collectedClueNames.Count) {
			ClueIndex = 0;
		}
		UpdateClueName (PlayerInv.collectedClueNames [ClueIndex]);
	}

	/// <summary>
	/// Decrements the clue index, then updates the displayed clue name
	/// </summary>
	public void PrevClue (){
		ClueIndex -= 1;
		if (ClueIndex < 0) {
			ClueIndex = PlayerInv.collectedClueNames.Count - 1;
		}
		UpdateClueName (PlayerInv.collectedClueNames [ClueIndex]);
	}

	// Updates the displayed clue name.
	void UpdateClueName (string ClueName){
		ActiveClue = ActiveStory.getClueInformation (ClueName).GetComponent<Clue> ();
		ClueText.text = ActiveClue.longName;
	}

	public void Accuse (){
		bool HasMotiveClue = false;
		bool HasMurderWeapon = false;
		bool IsActuallyMurderer = CharInScene.isMurderer;
		string BranchName = "";
		foreach (string TestClue in PlayerInv.collectedClueNames) {
			if (TestClue == ActiveStory.MurderWeapon) {
				HasMurderWeapon = true;
			}
			if (TestClue == ActiveStory.MotiveClue) {
				HasMotiveClue = true;
			}
		}
		if (HasMotiveClue == false && HasMurderWeapon == false) {
			BranchName = "Accuse-NoItems";
		} else if (IsActuallyMurderer == false) {
			BranchName = "Accuse-WrongChar";
		} else if (HasMotiveClue == false) {
			BranchName = "Accuse-Motive";
		} else if (HasMurderWeapon == false) {
			BranchName = "Accuse-Weapon";
		} else {BranchName = "Accuse-Right";}
		StartBranch(BranchName);
	}

	public void OnClick (){
		string NextLine = SpeechRef.NextLine ();
		if (NextLine != null) {
			ActualText.text = NextLine.Substring(1, NextLine.Length - 1);
			if (NextLine [0].ToString () == "$") {
				TextObject.transform.localPosition = DetectivePos;
			} else {
				TextObject.transform.localPosition = SuspectPos;
			}
		} else {
			Character CharInScene = FindObjectOfType<Character> ();
			CharInScene.PostSpeech (BranchName);
			gameObject.SetActive(false);
			TextObject.SetActive(false);
			foreach (Button ForButton in TraitButtonArray) {
				ForButton.gameObject.SetActive (true);
			}
			foreach (GameObject ForButton in SpeechUIElements) {
				ForButton.gameObject.SetActive (true);
			}
			try{
				if(BranchName.Substring(0,6) == "Accuse"){
					TurnOffSpeechUI();
				}
			}
			catch{}
		}
	}
}
