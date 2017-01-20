using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Imports the speech elements into the game.
/// </summary>
public class ImportSpeech : MonoBehaviour {

		// QUICK BIT ON THE TEXT FILE FORMATTING
		// Any line prefaced by a # is a branch title and will split branches
		// Anything after a ( is said by the detective
		// Anything after a ) is said by the suspect
		// [ and ] are replaced by the name of the incoming Item and Character respectively
		// Set those by changing ItemName and OtherChar as appropriate.
	/// <summary>
	/// The text from file
	/// </summary>
	public TextAsset asset;  // POINT THIS AT THE TEXT FILE YOU WANT

	/// <summary>
	/// The item in.
	/// </summary>
	public Clue ItemIn;		// TODO: Update these to be object pointers
	/// <summary>
	/// The char in.
	/// </summary>
	public Character CharIn;
	/// <summary>
	/// Pos is the current position in the Branch
	/// </summary>
	private int Pos = 0;
	/// <summary>
	/// The arraylist of the current branch
	/// </summary>
	private ArrayList BranchList;
	/// <summary>
	/// arraylist of the whole file
	/// </summary>
	private ArrayList SpeechList;
	/// <summary>
	/// SpeechDict is a dictionary of the form BranchName -> The relevent BranchList
	/// </summary>
	private Dictionary<string, ArrayList> SpeechDict;
									
	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start () {
	}

	public void ActualStart(){
		SpeechDict = new Dictionary<string, ArrayList> ();	//Set up the dictionary and the list for input
		SpeechList = new ArrayList ();
		string AssetText = asset.text; 		// Read the file
		AssetText = AssetText.Replace ("\r", "").Replace ("\n", "");	// Purge all newline chars
		while (AssetText.Length > 0) {		// While we still have text to parse...
			string TestString = AssetText;	// Make a copy of it
			TestString = TestString.Substring (1, TestString.Length - 1);// Ignore the first symbol
			int LIndex = TestString.IndexOf ("£");
			int RIndex = TestString.IndexOf ("$");	//Figure out whether a (, ) or # is closest to the start
			int HIndex = TestString.IndexOf ("#");
			if (LIndex == -1 && RIndex == -1 && HIndex == -1) {	// If none of the above could be found
				string Substring = AssetText;
				SpeechList.Add (Substring);		// The rest of the string gets added to the arraylist
				AssetText = "";		// Then clear out the text
			} else {
				if (LIndex == -1) {
					LIndex = 9999999;
				}	// If we couldn't find one of them we still care about the smallest of the others
				if (RIndex == -1) {
					RIndex = 9999999;
				}	// So set them to an arbitrarily larbe value
				if (HIndex == -1) {
					HIndex = 9999999;
				}	// If you need 9,999,999 characters, maybe consider breaking them up into smaller lines
				int Index = Mathf.Min (LIndex, RIndex, HIndex);	// Take the smallest value of them
				string Substring = AssetText.Substring (0, Index + 1);	// Take the string up to the nearest character
				SpeechList.Add (Substring); 						// Add to the arraylist
				string TempText = AssetText.Substring (Index + 1, AssetText.Length - Index - 1);	// Remove the added bit from the string
				AssetText = TempText;					// Repeat with the rest of the string
			}
		}
		Array2Dict ();	// Once we've read the entire file we can add it to the dictionary
	}

	/// <summary>
	/// Convert the Array into a dictionary
	/// </summary>
	private void Array2Dict(){
		ArrayList CurrentBranch = new ArrayList();	//First we set up a w	orking ArrayList
		string BranchName = "INIT";		// We want to name our branch so we set up a string to do so
		for (int i = 0; i < SpeechList.Count; i++) {	// For every line...
			string TestLine = SpeechList[i].ToString();			// Store the current line in a variable for ease of access...
			string TestChar = TestLine[0].ToString();			// Take the first character and cast to a string.
			if (TestChar == "#" && CurrentBranch.Count != 0) {	// If it's a # (but the not the first one)
				SpeechDict.Add(BranchName, CurrentBranch);	// The branch we were working on is done, so we can store it in the dict
				CurrentBranch = new ArrayList();			// Then we can start a new branch
				BranchName = TestLine.Substring(1, TestLine.Length - 1);	// And we can name is appropriately.
			} else if (TestChar == "#") {									// Obviously, the first hash won't have anything before it
				BranchName = TestLine.Substring(1, TestLine.Length - 1);	// So we just set the name and carry on
			} else {
				CurrentBranch.Add(TestLine);	// Otherwise we just add the line to the current branch.
			}
		}
		SpeechDict.Add(BranchName, CurrentBranch);	// At the EoA, we can just put what we had into the dict.
	}

	/// <summary>
	/// Sets the branch.
	/// </summary>
	/// <param name="BranchName">Branch name.</param>
	public void SetBranch (string BranchName) {	// This subroutine, suprisingly, sets the branch to read from
		if (Pos != 0) {		// If the Pos isn't 0 when you do this, that means you were in the middle of the branch when you did it...
			Debug.LogWarning ("SetBranch called in middle of branch? Resetting branch position...");
			Pos = 0;		// Which is odd, but Pos will be reset for safety.
		}
		BranchList = SpeechDict [BranchName];	// At any rate, BranchList gets updated from the SpeechDict appropriately.
	}


	// tl;dr: Everytime you call Ping() it will return the next line of the current branch
	/// <summary>
	/// Gets the next line in the branch
	/// </summary>
	/// <returns>The line.</returns>
	public string NextLine () {
		if (0 <= Pos && Pos < BranchList.Count) {				// Assuming we're still in bounds of the arraylist...
			string ReturnString = BranchList [Pos].ToString();	// Get the current line
			string ItemName = "ERR_NO_ITEM";
			string CharName = "ERR_NO_CHAR";
			if (ItemIn != null) {
				ItemName = ItemIn.GetComponent<ClueController> ().longName;
			}
			if (CharIn != null) {
				CharName = CharIn.longName;
			}
			ReturnString = ReturnString.Replace("[",ItemName);	// Replace token characters
			ReturnString = ReturnString.Replace("]",CharName);
			Pos += 1;											// Increment the position marker
			return ReturnString;								// And throw the string back
		} else if (Pos >= BranchList.Count) {		// If we've gone past the length of the list, then we're done with the branch.
			Pos = 0;								// Reset the position
			string NullString = null;				// Set up and return a null string.
			return NullString;						// The recieving object can use this as a cue that the branch is done.
		} else {		// The only remaining alternative is that Pos is somehow negative.
			throw new UnityException ("ERROR: Pos is negative");
						// Considering that Pos only gets set to 0 and incremented, this is VERY uninteded behaviour.
		}
	}
}


