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
	/// The HUD canvas.
	/// </summary>
	public HUDController HUDCanvas;
	public Vector3 DetectivePos;
	public Vector3 SuspectPos;
	public Button[] TraitButtonArray;
	public Detective ActiveDet;
	public Story ActiveStory;
	public GameObject[] OtherUIElements;
	public Text ClueText;
	public GameObject[] SpeechUIElements;

	private Button ParentButton;
	private Text ActualText;
	private ImportSpeech SpeechRef;
	private string BranchName;
	private Inventory PlayerInv;
	private Character CharInScene;
	private GameObject ParentCanvas;
	private int ClueIndex = 0;

	void Start (){
		Waiting ();
		ActiveDet = FindObjectOfType<Detective> ();
		ActiveStory = FindObjectOfType<Story> ();
		PlayerInv = FindObjectOfType<Inventory> ();
		HUDCanvas = FindObjectOfType<HUDController> ();
		string SceneName = SceneManager.GetActiveScene ().name;
		int RoomNo = (SceneName[SceneName.Length-1]) - '1';
		Debug.Log (RoomNo);
		List<GameObject> ListOfCharsInRoom = ActiveStory.getCharactersInRoom (RoomNo);

		try{
			CharInScene = ListOfCharsInRoom [0].GetComponent<Character> ();
			SpeechRef = CharInScene.GetComponentInChildren<ImportSpeech> ();
			ActualText = TextObject.GetComponentInChildren<Text> ();
			CharInScene.SpeechUI = this;
		} catch {}
		ParentCanvas = GameObject.Find ("SpeechUI");
		ParentCanvas.SetActive (false);
		gameObject.SetActive(false);
		string[] StrTraits = new string[ActiveDet.traits.Length];
		//string [] StrTraits = new string[3] {"UNO", "DOS", "TRES"};
		for (int i = 0; i < StrTraits.Length; i++) {
			StrTraits [i] = ActiveStory.getTraitString (ActiveDet.traits [i]);
			Debug.Log (TraitButtonArray [i].name);
			Text ButtonText = TraitButtonArray [i].GetComponentInChildren<Text> ();
			ButtonText.text = StrTraits [i];
			TraitButtonArray [i].name = StrTraits [i];
		}
	}

	IEnumerator Waiting(){
		yield return new WaitForFixedUpdate ();
	}

	public void TurnOnSpeechUI(){
		//TODO: Maybe some intro speech here?
		if (ParentCanvas.gameObject.activeSelf == false) {
			if (CharInScene.CanBeTalkedTo == true) {
				foreach (GameObject ForObj in OtherUIElements) {
					ForObj.gameObject.SetActive (false);
				}
				UpdateClueName (PlayerInv.collectedClueNames [ClueIndex]);
				ParentCanvas.SetActive (true);
				HUDCanvas.loadPanelAndPause (ParentCanvas);
				if (CharInScene.HasBeenTalkedTo == false) {
					CharInScene.HasBeenTalkedTo = true;
					StartBranch ("INTRO");
				}
			} else {
				Debug.Log ("You need to find another clue");
			}
		} else {
			Debug.Log ("Tried to turn on speech UI, but was already on");
		}
	}

	public void TurnOffSpeechUI(){
		if (ParentCanvas.gameObject.activeSelf == true) {
			foreach (GameObject ForObj in OtherUIElements) {
				ForObj.gameObject.SetActive (true);
			}
			ClueIndex = 0;
			ParentCanvas.SetActive (false);
			HUDCanvas.hidePanelAndResume (ParentCanvas);
		} else {
			Debug.Log ("Tried to turn off speech UI but it was already off.");
		}
	}

	public void QuestionBranch(Button SendingButton){
		BranchName = SendingButton.name;
		string LongName = BranchName + "-" + PlayerInv.collectedClueNames [ClueIndex];
		try{
			SpeechRef.SetBranch (LongName);
			BranchName = LongName;
		}
		catch{
			SpeechRef.SetBranch(BranchName);
		}
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

	public void StartBranch(string BranchIn){
		BranchName = BranchIn;
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

	public void NextClue (){
		ClueIndex += 1;
		if (ClueIndex >= PlayerInv.collectedClueNames.Count) {
			ClueIndex = 0;
		}
		UpdateClueName (PlayerInv.collectedClueNames [ClueIndex]);
	}

	public void PrevClue (){
		ClueIndex -= 1;
		if (ClueIndex < 0) {
			ClueIndex = PlayerInv.collectedClueNames.Count - 1;
		}
		UpdateClueName (PlayerInv.collectedClueNames [ClueIndex]);
	}

	void UpdateClueName (string ClueName){
		ClueText.text = ClueName;
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
				TextObject.transform.position = DetectivePos;
			} else {
				TextObject.transform.position = SuspectPos;
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
