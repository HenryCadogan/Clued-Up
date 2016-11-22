using UnityEngine;
using System.Collections;

public class ImportSpeech : MonoBehaviour {

	public TextAsset asset;  // POINT THIS AT THE TEXT FILE YOU WANT
	private string AssetText;
	private ArrayList SpeechList;
	private int Pos;
	private int Index;

	// Use this for initialization
	void Start () {
		Debug.Log("STARTED FROM THE BOTTOM");
		Pos = 0;
		Debug.Log("NOW WE HERE");
		SpeechList = new ArrayList ();	// Make sure speeck starts as an empty list
		Debug.Log("GOT THIS SWEET ARRAYLIST");
		AssetText = asset.text; 		// Read the file
		Debug.Log("GOT THIS SWEET ASSETTEXT");
		AssetText = asset.text.Replace ("\n", "");	// Purge all newline chars
		Debug.Log("FUCK THE FUCK OFF NEWLINES");
		//while (AssetText.Length > 0) {
		for (int i = 1; i <= 2; i++) {
			Index = -1;
			Debug.Log("I DID AN INDEX");
			Debug.Log (AssetText + AssetText.Length.ToString());
			string TestString = AssetText;
			TestString = TestString.Substring (1, TestString.Length - 1);// Ignore the first symbol
			Debug.Log(TestString);
			int PIndex = TestString.IndexOf ("(");
			int DIndex = TestString.IndexOf (")");	//Figure out whether a £ or $ is closest
			if (PIndex < DIndex) {
				Index = PIndex;
			} else {
				Index = DIndex;		// Store its position
			}
			Debug.Log (Index);
			string Substring = AssetText.Substring (0, DIndex);	// Get the substring from it
			Debug.Log(Substring);
			SpeechList.Add (Substring); 						// Add to the arraylist
			string TempText = AssetText.Substring (Index, AssetText.Length - Index);
			AssetText = TempText;
			Debug.Log (AssetText);
			//yield return new WaitForSeconds (1);
																// Remove parsed string
		}
	}

	string Ping () {
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

	public void TestScript(){
		string InString = Ping ();
		GUIText btnText = gameObject.GetComponentInChildren (typeof(GUIText)) as GUIText;
		if (InString == null) {
			transform.position = new Vector3 (0, 0, 0);
			btnText.text = "~~END OF MESSAGE~~";
		} else if (InString [0].ToString() == "(") {
			transform.position = new Vector3 (-50, 0, 0);
			btnText.text = InString.Substring (1, InString.Length - 1);
		} else if (InString [0].ToString() == ")") {
			transform.position = new Vector3 (50, 0, 0);
			btnText.text = InString.Substring (1, InString.Length - 1);
		} else {
			throw new UnityException ("ERROR: What in the fuck is " + InString);
		}
	}
}
