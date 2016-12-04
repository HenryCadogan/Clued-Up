using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeechHandler : MonoBehaviour {

	public GameObject TargetChar;
	public GameObject TextObject;
	public Vector3 DetectivePos;
	public Vector3 SuspectPos;
	public Button[] ButtonArray;

	private Button ParentButton;
	private Text ActualText;
	private ImportSpeech SpeechRef;

	void Start (){
		SpeechRef = TargetChar.GetComponentInChildren<ImportSpeech> ();
		ActualText = TextObject.GetComponentInChildren<Text> ();
		gameObject.SetActive(false);
	}

	public void StartBranch(string BranchName){
		SpeechRef.SetBranch (BranchName);
		gameObject.SetActive(true);
		TextObject.SetActive(true);
		foreach (Button ForButton in ButtonArray) {
			ForButton.gameObject.SetActive (false);
		}
		OnClick ();
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
			gameObject.SetActive(false);
			TextObject.SetActive(false);
			foreach (Button ForButton in ButtonArray) {
				ForButton.gameObject.SetActive (true);
			}
		}
	}
}
