using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImportSpeech : MonoBehaviour {

	public TextAsset asset;  // POINT THIS AT THE TEXT FILE YOU WANT
	public Vector3 LeftPos;  // THIS CO-ORDINATE WANTS TO BE WHERE YOU WANT ( TO APPEAR
	public Vector3 RightPos;   // SAME BUT FOR )
	public Vector3 NeutralPos; // SAME BUT FOR WHEN YOU'RE DONE
	private string AssetText;
	private ArrayList SpeechList;
	private int Pos;
	private int Index;

	// Use this for initialization
	void Start () {
		Pos = 0;
		SpeechList = new ArrayList ();	// Make sure speeck starts as an empty list
		AssetText = asset.text; 		// Read the file
		AssetText = AssetText.Replace("\r", "").Replace("\n", "");	// Purge all newline chars
		while (AssetText.Length > 0) {
		//for (int i = 1; i <= 6; i++) {    //IGNORE THIS LINE IT'S FOR TEST PURPOSES
			Index = -1;
			string TestString = AssetText;
			TestString = TestString.Substring (1, TestString.Length - 1);// Ignore the first symbol
			int PIndex = TestString.IndexOf ("(");
			int DIndex = TestString.IndexOf (")");	//Figure out whether a ( or ) is closest
			if (PIndex < DIndex && PIndex > 0) {
				Index = PIndex;
			} else if (DIndex > 0) { 
				Index = DIndex;		// Store its position
			}
			if (Index != -1) {		// If we found a ( or )
				string Substring = AssetText.Substring (0, Index + 1);	// Get the substring from it
				SpeechList.Add (Substring); 						// Add to the arraylist
				string TempText = AssetText.Substring (Index + 1, AssetText.Length - Index - 1);
				AssetText = TempText;					// Repeat with the rest of the string
			} else {							// If there isn't one...
				string Substring = AssetText;
				SpeechList.Add (Substring);		// The rest of the string gets added to the arraylist
				AssetText = "";		// Then clear out the text
			}
		}
	}


	// tl;cbacommenting: Everytime you call Ping() it will return the next line of the arraylist
	// When it's done you get a null string, which can be interpreted as required
	public string Ping () {
		if (0 <= Pos && Pos < SpeechList.Count) {
			string ReturnString = SpeechList [Pos].ToString ();
			Pos += 1;
			return ReturnString;
		} else if (Pos >= SpeechList.Count) {
			Pos = 0;
			string NullString = null;
			return NullString;
		} else {
			throw new UnityException ("ERROR: Ping Position is underneath SpeechList?");
		}
	}

	//This shit is gonna be rewritten into something relevent it's just here for demonstration
	public void TestScript(){
		string InString = Ping ();
		if (InString == null) {
			transform.position = NeutralPos;
			GetComponentInChildren<Text>().text = "~~END OF MESSAGE~~";
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
