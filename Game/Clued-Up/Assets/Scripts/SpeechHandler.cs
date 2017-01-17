using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class SpeechHandler : MonoBehaviour {

	public Character TargetChar;
	public GameObject TextObject;
	public HUDController HUDCanvas;
	public Vector3 DetectivePos;
	public Vector3 SuspectPos;
	public Button[] TraitButtonArray;
	public Detective ActiveDet;
	public Story ActiveStory;
	public Button LeaveButton;
	public GameObject[] OtherUIElements;
	public Text ClueText;

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
		CharInScene = FindObjectOfType<Character> ();
		HUDCanvas = FindObjectOfType<HUDController> ();
		string SceneName = SceneManager.GetActiveScene ().name;
		int RoomNo = (SceneName[SceneName.Length-1]) - '0';
		List<GameObject> ListOfCharsInRoom = ActiveStory.getCharactersInRoom (RoomNo);
		//TargetChar = ListOfCharsInRoom [0].GetComponent<Character> ();
		SpeechRef = TargetChar.GetComponentInChildren<ImportSpeech> ();
		ActualText = TextObject.GetComponentInChildren<Text> ();
		ParentCanvas = GameObject.Find ("SpeechUI");
		ParentCanvas.SetActive (false);
		gameObject.SetActive(false);
	}

	void Awake(){
		ActiveDet = FindObjectOfType<Detective> ();
		ActiveStory = FindObjectOfType<Story> ();
		PlayerInv = FindObjectOfType<Inventory> ();
		CharInScene = FindObjectOfType<Character> ();
		HUDCanvas = FindObjectOfType<HUDController> ();
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
			foreach (GameObject ForObj in OtherUIElements) {
				ForObj.gameObject.SetActive (false);
			}
			UpdateClueName (PlayerInv.collectedClueNames [ClueIndex]);
			ParentCanvas.SetActive (true);
			HUDCanvas.loadPanelAndPause (ParentCanvas);
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
		SpeechRef.SetBranch (SendingButton.name);
		BranchName = SendingButton.name;
		BranchName = BranchName + PlayerInv.collectedClueNames [ClueIndex];
		CharInScene.PreSpeech (BranchName);
		gameObject.SetActive(true);
		TextObject.SetActive(true);
		foreach (Button ForButton in TraitButtonArray) {
			ForButton.gameObject.SetActive (false);
		}
		LeaveButton.gameObject.SetActive (false);
		OnClick ();
	}

	public void StartBranch(string BranchIn){
		BranchName = BranchIn;
		CharInScene.PreSpeech (BranchName);
		gameObject.SetActive(true);
		TextObject.SetActive(true);
		foreach (Button ForButton in TraitButtonArray) {
			ForButton.gameObject.SetActive (false);
		}
		LeaveButton.gameObject.SetActive (false);
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
			if (NextLine [0].ToString () == "(") {
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
			LeaveButton.gameObject.SetActive (true);
		}
	}
}
