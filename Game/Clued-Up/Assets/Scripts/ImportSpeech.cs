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
		// Anything after a $ is said by the detective
		// Anything after a £ is said by the suspect
		// [ and ] are replaced by the name of the incoming Item and Character respectively
		// Set those by changing ItemName and OtherChar as appropriate.
	/// <summary>
	/// The text from file
	/// </summary>
	public TextAsset asset;  // POINT THIS AT THE TEXT FILE YOU WANT

	/// <summary>
	/// The item in.
	/// </summary>
	public Clue itemIn;		// TODO: Update these to be object pointers
	/// <summary>
	/// The char in.
	/// </summary>
	public Character charIn;
	/// <summary>
	/// Pos is the current position in the Branch
	/// </summary>
	private int pos = 0;
	/// <summary>
	/// The arraylist of the current branch
	/// </summary>
	private ArrayList branchList;
	/// <summary>
	/// arraylist of the whole file
	/// </summary>
	private ArrayList speechList;
	/// <summary>
	/// SpeechDict is a dictionary of the form BranchName -> The relevent BranchList
	/// </summary>
	private Dictionary<string, ArrayList> speechDict;

	/// <summary>
	/// Runs initialisation, but after everything else is initialised for safety.
	/// </summary>
	public void actualStart(){
		speechDict = new Dictionary<string, ArrayList> ();	//Set up the dictionary and the list for input
		speechList = new ArrayList ();
		string assetText = asset.text; 		// Read the file
		assetText = assetText.Replace ("\r", "").Replace ("\n", "");	// Purge all newline chars
		while (assetText.Length > 0) {		// While we still have text to parse...
			string testString = assetText;	// Make a copy of it
			testString = testString.Substring (1, testString.Length - 1);// Ignore the first symbol
			int lIndex = testString.IndexOf ("£");
			int rIndex = testString.IndexOf ("$");	//Figure out whether a £, $ or # is closest to the start
			int hIndex = testString.IndexOf ("#");
			if (lIndex == -1 && rIndex == -1 && hIndex == -1) {	// If none of the above could be found
				string substring = assetText;
				speechList.Add (substring);		// The rest of the string gets added to the arraylist
				assetText = "";		// Then clear out the text
			} else {
				if (lIndex == -1) {
					lIndex = 9999999;
				}	// If we couldn't find one of them we still care about the smallest of the others
				if (rIndex == -1) {
					rIndex = 9999999;
				}	// So set them to an arbitrarily large value
				if (hIndex == -1) {
					hIndex = 9999999;
				}	// If you need 9,999,999 characters, maybe consider breaking them up into smaller lines
				int index = Mathf.Min (lIndex, rIndex, hIndex);	// Take the smallest value of them
				string substring = assetText.Substring (0, index + 1);	// Take the string up to the nearest character
				speechList.Add (substring); 						// Add to the arraylist
				string tempText = assetText.Substring (index + 1, assetText.Length - index - 1);	// Remove the added bit from the string
				assetText = tempText;					// Repeat with the rest of the string
			}
		}
		array2Dict ();	// Once we've read the entire file we can add it to the dictionary
	}

	/// <summary>
	/// Convert the speech arraylist into a dictionary.
	/// </summary>
	private void array2Dict(){
		ArrayList currentBranch = new ArrayList();	//First we set up a working ArrayList
		string branchName = "INIT";		// We want to name our branch so we set up a string to do so
		for (int i = 0; i < speechList.Count; i++) {	// For every line...
			string testLine = speechList[i].ToString();			// Store the current line in a variable for ease of access...
			string testChar = testLine[0].ToString();			// Take the first character and cast to a string.
			if (testChar == "#" && currentBranch.Count != 0) {	// If it's a # (but the not the first one)
				speechDict.Add(branchName, currentBranch);	// The branch we were working on is done, so we can store it in the dict
				currentBranch = new ArrayList();			// Then we can start a new branch
				branchName = testLine.Substring(1, testLine.Length - 1);	// And we can name is appropriately.
			} else if (testChar == "#") {									// Obviously, the first hash won't have anything before it
				branchName = testLine.Substring(1, testLine.Length - 1);	// So we just set the name and carry on
			} else {
				currentBranch.Add(testLine);	// Otherwise we just add the line to the current branch.
			}
		}
		speechDict.Add(branchName, currentBranch);	// At the EoA, we can just put what we had into the dict.
	}

	/// <summary>
	/// Sets the active branch.
	/// </summary>
	/// <param name="BranchName">Branch name.</param>
	public void setBranch (string branchName) {	// This subroutine, suprisingly, sets the branch to read from
		if (pos != 0) {		// If the Pos isn't 0 when you do this, that means you were in the middle of the branch when you did it...
			Debug.LogWarning ("SetBranch called in middle of branch? Resetting branch position...");
			pos = 0;		// Which is odd, but Pos will be reset for safety.
		}
		branchList = speechDict [branchName];	// At any rate, BranchList gets updated from the SpeechDict appropriately.
	}


	// tl;dr: Everytime you call NextLine() it will return the next line of the current branch
	/// <summary>
	/// Gets the next line in the branch
	/// </summary>
	/// <returns>The line.</returns>
	public string nextLine () {
		if (0 <= pos && pos < branchList.Count) {				// Assuming we're still in bounds of the arraylist...
			string returnString = branchList [pos].ToString();	// Get the current line
			string itemName = "ERR_NO_ITEM";
			string charName = "ERR_NO_CHAR";
			if (itemIn != null) {
				itemName = itemIn.longName;
			}
			if (charIn != null) {
				charName = charIn.longName;
			}
			returnString = returnString.Replace("[",itemName);	// Replace token characters
			returnString = returnString.Replace("]",charName);
			pos += 1;											// Increment the position marker
			return returnString;								// And throw the string back
		} else if (pos >= branchList.Count) {		// If we've gone past the length of the list, then we're done with the branch.
			pos = 0;								// Reset the position
			string nullString = null;				// Set up and return a null string.
			return nullString;						// The recieving object can use this as a cue that the branch is done.
		} else {		// The only remaining alternative is that Pos is somehow negative.
			throw new UnityException ("ERROR: Pos is negative");
						// Considering that Pos only gets set to 0 and incremented, this is VERY uninteded behaviour.
		}
	}
}


