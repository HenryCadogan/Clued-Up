using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImportSpeech : MonoBehaviour {

	public TextAsset asset;  // POINT THIS AT THE TEXT FILE YOU WANT
	public Vector3 LeftPos;  // THIS CO-ORDINATE WANTS TO BE WHERE YOU WANT ( TO APPEAR
	public Vector3 RightPos;   // SAME BUT FOR )
	public Vector3 NeutralPos; // SAME BUT FOR WHEN YOU'RE DONE
	public string ItemName;
	public string OtherChar;
	private int Pos = 0;
	private ArrayList BranchList;
	private ArrayList SpeechList;
	private Dictionary<string, ArrayList> SpeechDict;

	// Use this for initialization
	private void Start () {
		SpeechDict = new Dictionary<string, ArrayList>();
		SpeechList = new ArrayList ();
		string AssetText = asset.text; 		// Read the file
		AssetText = AssetText.Replace("\r", "").Replace("\n", "");	// Purge all newline chars
		AssetText = AssetText.Replace("[",ItemName);
		AssetText = AssetText.Replace("]",OtherChar);
		while (AssetText.Length > 0) {
			string TestString = AssetText;
			TestString = TestString.Substring (1, TestString.Length - 1);// Ignore the first symbol
			Debug.Log(TestString);
			int LIndex = TestString.IndexOf ("(");
			int RIndex = TestString.IndexOf (")");	//Figure out whether a (, ) or # is closest to the start
			int HIndex = TestString.IndexOf("#");
			if (LIndex == -1 && RIndex == -1 && HIndex == -1) {
				string Substring = AssetText;
				SpeechList.Add (Substring);		// The rest of the string gets added to the arraylist
				Debug.Log(Substring);
				AssetText = "";		// Then clear out the text
			} else {
				if (LIndex == -1) {LIndex = 9999999;}
				if (RIndex == -1) {RIndex = 9999999;}
				if (HIndex == -1) {HIndex = 9999999;}
				int Index = Mathf.Min (LIndex, RIndex, HIndex);
				string Substring = AssetText.Substring (0, Index + 1);	// Get the substring from it
				SpeechList.Add (Substring); 						// Add to the arraylist
				Debug.Log(Substring);
				string TempText = AssetText.Substring (Index + 1, AssetText.Length - Index - 1);
				AssetText = TempText;					// Repeat with the rest of the string
			}
		}
		Array2Dict ();

		SetBranch ("INTRO"); //TODO: DELETE THIS LINE IT IS FOR TEST PURPOSES ONLY
	}

	private void Array2Dict(){
		ArrayList CurrentBranch = new ArrayList();
		string BranchName = "";
		for (int i = 0; i < SpeechList.Count; i++) {
			string TestLine = SpeechList[i].ToString();
			string TestChar = TestLine[0].ToString();
			if (TestChar == "#" && CurrentBranch.Count != 0) {
				SpeechDict.Add(BranchName, CurrentBranch);
				CurrentBranch = new ArrayList();
				BranchName = TestLine.Substring(1, TestLine.Length - 1);
			} else if (TestChar == "#") {
				BranchName = TestLine.Substring(1, TestLine.Length - 1);
			} else {
				CurrentBranch.Add(TestLine);
			}
		}
		SpeechDict.Add(BranchName, CurrentBranch);
	}

	public void SetBranch (string BranchName) {
		if (Pos != 0) {
			Debug.LogWarning ("SetBranch called in middle of branch? Going through with it, resetting branch.");
			Pos = 0;
		}
		BranchList = SpeechDict [BranchName];
	}

	// tl;dr: Everytime you call Ping() it will return the next line of the current branch
	// When it's done you get a null string, which can be interpreted as required
	public string Ping () {
		if (0 <= Pos && Pos < BranchList.Count) {
			string ReturnString = BranchList [Pos].ToString ();
			Pos += 1;
			return ReturnString;
		} else if (Pos >= BranchList.Count) {
			Pos = 0;
			string NullString = null;
			return NullString;
		} else {
			throw new UnityException ("ERROR: Ping Position is underneath SpeechList?");
		}
	}

	//This bit is gonna be rewritten into something relevent it's just here for demonstration
	public void TestScript(){
		string InString = Ping ();
		if (InString == null) {
			transform.position = NeutralPos;
			GetComponentInChildren<Text>().text = "~~END OF MESSAGE~~";
			SetBranch ("QUACK");
		} else if (InString [0].ToString() == "(") {
			transform.position = LeftPos;
			string MemeString = InString.Substring (1, InString.Length - 1);
			GetComponentInChildren<Text>().text = MemeString;
		} else if (InString [0].ToString() == ")") {
			transform.position = RightPos;
			string MemeString = InString.Substring (1, InString.Length - 1);
			GetComponentInChildren<Text>().text = MemeString;
		} else {
			throw new UnityException ("ERROR: What in the fuck is " + InString);
		}
	}
}


