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
	public GameObject textObject;
	/// <summary>
	/// The canvas of the main HUD (i.e. not the one the speech UI is attatched to).
	/// </summary>
	public HUDController hudCanvas;
	/// <summary>
	/// The world position that the text should appear in when the detective is talking.
	/// </summary>
	public Vector3 detectivePos;
	/// <summary>
	/// The world position that the text should appear in when the suspect is talking.
	/// </summary>
	public Vector3 suspectPos;
	/// <summary>
	/// An array of buttons to be linked to the detective's traits.
	/// </summary>
	public Button[] traitButtonArray;
	/// <summary>
	/// The active detective.
	/// </summary>
	public Detective activeDet;
	/// <summary>
	/// The active story.
	/// </summary>
	public Story activeStory;
	/// <summary>
	/// An array containing any UI elements that should be hidden when the speech UI is up.
	/// </summary>
	public GameObject[] otherUIElements;
	/// <summary>
	/// The panel object that holds the
	/// </summary>
	public GameObject speechPanel;
	/// <summary>
	/// The text object that shows the currently selected clue.
	/// </summary>
	public Text clueText;
	/// <summary>
	/// Any speech UI elements that should be hidden during the actual conversatsion.
	/// </summary>
	public GameObject[] speechUIElements;

	/// <summary>
	/// The button that this script is attatched to.
	/// </summary>
	private Button parentButton;
	/// <summary>
	/// The text of the text object that shows the speech.
	/// </summary>
	private Text actualText;
	/// <summary>
	/// The actual speech files attatched to the character in the scene.
	/// </summary>
	private ImportSpeech speechRef;
	/// <summary>
	/// The name of the currently/last active branch of discussion.
	/// </summary>
	private string branchName;
	/// <summary>
	/// A reference to the inventory of the detective.
	/// </summary>
	private Inventory playerInv;
	/// <summary>
	/// The character active in the current scene.
	/// </summary>
	private Character charInScene;
	/// <summary>
	/// The canvas of the Speech UI.
	/// </summary>
	private GameObject parentCanvas;
	/// <summary>
	/// The inventory index of the currently selected item.
	/// </summary>
	private int clueIndex = 0;
	private Clue activeClue;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start (){
		waiting ();
		//TODO: REMOVE STUPID CAPITALIZATION
		// Assign various variables from objects in scene.
		activeDet = FindObjectOfType<Detective> ();
		activeStory = FindObjectOfType<Story> ();
		playerInv = FindObjectOfType<Inventory> ();
		hudCanvas = FindObjectOfType<HUDController> ();
		string sceneName = SceneManager.GetActiveScene ().name;
		int roomNo = (sceneName[sceneName.Length-1]) - '1';
		Debug.Log (roomNo);
		List<GameObject> listOfCharsInRoom = activeStory.getCharactersInRoom (roomNo);
		actualText = textObject.GetComponentInChildren<Text> ();

		// Try to access the data from the character in the scene.
		try{
			charInScene = listOfCharsInRoom [0].GetComponent<Character> ();
			speechRef = charInScene.GetComponentInChildren<ImportSpeech> ();
			charInScene.speechUI = this;

		// This will fail if there is no character in scene,
		// but that's fine because there's no access to the UI then.
		} catch {}

		// A bit more object fetching...
		parentCanvas = GameObject.Find ("SpeechUI");
		parentCanvas.SetActive (false);
		gameObject.SetActive(false);
		string[] strTraits = new string[activeDet.traits.Length];

		// Set the values on the trait buttons from the detective traits.
		for (int i = 0; i < strTraits.Length; i++) {
			strTraits [i] = activeStory.getTraitString (activeDet.traits [i]);
			Debug.Log (traitButtonArray [i].name);
			Text buttonText = traitButtonArray [i].GetComponentInChildren<Text> ();
			buttonText.text = strTraits [i];
			traitButtonArray [i].name = strTraits [i];
		}
	}

	// This just delays the setup for an update to make sure everything is initialised.
	// You could _probably_ get rid of it but it's here for safety.
	IEnumerator waiting(){
		yield return new WaitForFixedUpdate ();
	}

	/// <summary>
	/// Turns on the Speech UI, pauses the game.
	/// </summary>
	public void turnOnSpeechUI(){
		if (parentCanvas.gameObject.activeSelf == false) {
			// If the character is in a state where he can be talked to...
			if (charInScene.canBeTalkedTo == true) {
				// Turn off all of the non-speech UI elements...
				foreach (GameObject forObj in otherUIElements) {
					forObj.gameObject.SetActive (false);
				}
				speechPanel.SetActive (false);
				// Then turn on all the speech UI stuff.
				updateClueName (playerInv.collectedClueNames [clueIndex]);
				parentCanvas.SetActive (true);
				hudCanvas.loadPanelAndPause (parentCanvas);
				if (charInScene.hasBeenTalkedTo == false) {
					charInScene.hasBeenTalkedTo = true;
					startBranch ("INTRO");
				}

				// Get the collider of the character and turn it off.
				BoxCollider BoxCol = charInScene.GetComponent<BoxCollider> ();
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
	public void turnOffSpeechUI(){
		if (parentCanvas.gameObject.activeSelf == true) {
			// Turn on all the non-speech UI elements first.
			foreach (GameObject ForObj in otherUIElements) {
				ForObj.gameObject.SetActive (true);
			}

			//Then turn off all of the speech UI elements.
			clueIndex = 0;
			parentCanvas.SetActive (false);
			hudCanvas.hidePanelAndResume (parentCanvas);

			// Re-enable the collider of the character..
			BoxCollider boxCol = charInScene.GetComponent<BoxCollider> ();
			boxCol.enabled = true;
			// If we accused them, then turn it back off.
			try{
				if(branchName.Substring(0,6) == "Accuse"){
					boxCol.enabled=false;
				}
			}
			catch{}
		//Obviously, you can't turn off a UI if it's already off, so do nothing if so.
		} else {
			Debug.Log ("Tried to turn off speech UI but it was already off.");
		}
	}

	/// <summary>
	/// Starts a branch from a trait button.
	/// </summary>
	/// <param name="sendingButton">The button pressed to start the branch.</param>
	public void questionBranch(Button sendingButton){
		// Set the branch name to be the trait combined with the item dev name.
		branchName = sendingButton.name;
		speechRef.itemIn = activeClue;
		string longName = branchName + "-" + playerInv.collectedClueNames [clueIndex];
		// See if the character has a branch for the combination of trait and item.
		try{
			speechRef.setBranch (longName);
			branchName = longName;
		}
		// If they don't just go with the trait-only branch.
		catch{
			speechRef.setBranch(branchName);
		}
		// Let the character make any required changes.
		charInScene.preSpeech (branchName);
		// Turn on the text and continue button...
		gameObject.SetActive(true);
		textObject.SetActive(true);
		speechPanel.SetActive (true);
		// And turn off everything else.
		foreach (Button forButton in traitButtonArray) {
			forButton.gameObject.SetActive (false);
		}
		foreach (GameObject forButton in speechUIElements) {
			forButton.gameObject.SetActive (false);
		}
		// Then display the first line of the speech.
		OnClick ();
	}

	/// <summary>
	/// Starts the branch from a string in lieu of a button.
	/// </summary>
	/// <param name="branchIn">The name of the branch to be started.</param>
	public void startBranch(string branchIn){
		// Set the name of the branch.
		branchName = branchIn;
		// Initialise the branch.
		speechRef.itemIn = activeClue;
		speechRef.setBranch(branchName);
		// Let the character set up...
		charInScene.preSpeech (branchName);
		// Then turn on the text and continue button...
		gameObject.SetActive(true);
		textObject.SetActive(true);
		speechPanel.SetActive (true);
		// And turn off everything else.
		foreach (Button forButton in traitButtonArray) {
			forButton.gameObject.SetActive (false);
		}
		foreach (GameObject forButton in speechUIElements) {
			forButton.gameObject.SetActive (false);
		}
		// Then display the first line of text.
		OnClick ();
	}

	/// <summary>
	/// Increments the clue index, then updates the displayed clue name.
	/// </summary>
	public void nextClue (){
		clueIndex += 1;
		if (clueIndex >= playerInv.collectedClueNames.Count) {
			clueIndex = 0;
		}
		updateClueName (playerInv.collectedClueNames [clueIndex]);
	}

	/// <summary>
	/// Decrements the clue index, then updates the displayed clue name
	/// </summary>
	public void prevClue (){
		clueIndex -= 1;
		if (clueIndex < 0) {
			clueIndex = playerInv.collectedClueNames.Count - 1;
		}
		updateClueName (playerInv.collectedClueNames [clueIndex]);
	}

	/// <summary>
	/// Updates the name of the clue on the UI
	/// </summary>
	/// <param name="ClueName">The dev name of the clue to be shown.</param>
	void updateClueName (string ClueName){
		activeClue = activeStory.getClueInformation (ClueName).GetComponent<Clue> ();
		clueText.text = activeClue.longName;
	}
	/// <summary>
	/// Accuse the character in the scene of being a murderer.
	/// </summary>
	public void accuse (){
		// When accusing, we check if the detective has the motive clue and murder weapon...
		bool hasMotiveClue = false;
		bool hasMurderWeapon = false;
		// As well as if the character is actually the murderer.
		bool isMurderer = charInScene.isMurderer;
		string branchName = "";
		// Go through each clue to see if it's the murder weapon or motive clue.
		foreach (string testClue in playerInv.collectedClueNames) {
			if (testClue == activeStory.MurderWeapon) {
				hasMurderWeapon = true;
			}
			if (testClue == activeStory.MotiveClue) {
				hasMotiveClue = true;
			}
		}
		// Then select a branch accordingly.
		if (hasMotiveClue == false && hasMurderWeapon == false) {
			branchName = "Accuse-NoItems";
		} else if (hasMotiveClue == false) {
			branchName = "Accuse-Motive";
		} else if (hasMurderWeapon == false) {
			branchName = "Accuse-Weapon";
		} else if (isMurderer == false) {
			branchName = "Accuse-WrongChar";
		} else {branchName = "Accuse-Right";}
		// Whatever branch gets selected, we can then start it.
		startBranch(branchName);
	}

	/// <summary>
	/// Displays the next line of text in the branch, or returns to the main speech UI.
	/// </summary>
	public void OnClick (){
		// Get the next line of the branch.
		string nextLine = speechRef.nextLine ();
		if (nextLine != null) {
			// Display it and put it in position.
			actualText.text = nextLine.Substring(1, nextLine.Length - 1);
			if (nextLine [0].ToString () == "$") {
				speechPanel.transform.localPosition = detectivePos;
			} else {
				speechPanel.transform.localPosition = suspectPos;
			}
		// If the string was null then we have hit the end of the branch.
		} else {
			// Let the character update if needed.
			charInScene.postSpeech (branchName);
			// Disable the text and continue button, turn everything else on...
			gameObject.SetActive(false);
			speechPanel.SetActive (false);
			foreach (Button forButton in traitButtonArray) {
				forButton.gameObject.SetActive (true);
			}
			foreach (GameObject forButton in speechUIElements) {
				forButton.gameObject.SetActive (true);
			}
			// If the branch was an accuse branch, we then close the speech UI.
			try{
				if(branchName.Substring(0,6) == "Accuse"){
					turnOffSpeechUI();
				}
			}
			// This is in a try because if the branch name is smaller than 6 characters
			// it will raise an exception, but we know it's not Accuse so we can just
			// carry on our merry way.
			catch{}
		}
	}
}
