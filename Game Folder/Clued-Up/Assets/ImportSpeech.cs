using UnityEngine;
using System.Collections;

public class ImportSpeech : MonoBehaviour {

	public TextAsset asset;  // POINT THIS AT THE TEXT FILE YOU WANT
	private string AssetText;
	private ArrayList SpeechList;
	private int Pos;

	// Use this for initialization
	void Start () {
		SpeechList = new ArrayList ();	// Make sure speeck starts as an empty list
		AssetText = asset.text; 		// Read the file
		AssetText = asset.text.Replace ("\n", "");	// Purge all newline chars
		while (AssetText.Length > 0) {
			int Index = -1;
			string TestString = AssetText.Substring (1, AssetText.Length - 1); // Ignore the first symbol
			int PIndex = TestString.IndexOf ("£");
			int DIndex = TestString.IndexOf ("$");	//Figure out whether a £ or $ is closest
			if (PIndex < DIndex) {
				Index = PIndex;
			} else {
				Index = DIndex;		// Store its position
			}
			string Substring = AssetText.Substring (0, DIndex);	// Get the substring from it
			SpeechList.Add (Substring); 						// Add to the arraylist
			AssetText = AssetText.Substring (Index, AssetText.Length - Index);
																// Remove parsed string
		}
	}

	string Ping () {
		Pos += 1; // TODO: HANDLE POS > LEN
		return SpeechList[Pos - 1].ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
