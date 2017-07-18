﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// The Inventory class
/// </summary>

public class Inventory : MonoBehaviour {
	/// <summary>
	/// The list of collected clues.
	/// </summary>
	public List<string> collectedClueNames = new List<string>(); //cannot store the clues themselves as they are Destroyed between scenes
	/// <summary>
	/// The list of encountered characters.
	/// </summary>
	public List<string> encounteredCharacterNames = new List<string>();

	/// <summary>
	/// <true> if the specified clue has been collected
	/// </summary>
	/// <returns><c>true</c>, if specified clue is in the inventory, <c>false</c> otherwise.</returns>
	/// <param name="clue">Clue.</param>
	public bool isCollected(string clueName){
		return collectedClueNames.Contains (clueName);
	}
	/// <summary>
	/// Collect the specified clue by adding it to the inventory list, if not there already.
	/// </summary>
	/// <param name="clue">The clue to collect</param>
	public void collect(string clueName){
		if (!this.isCollected (clueName)) {
			// Add the clue to the inventory...
			Debug.Log (clueName + " COLLECTED");
			collectedClueNames.Add (clueName);
			// And set all characters so we can talk to them again.
			Character[] listOfChars = FindObjectsOfType<Character> ();
			foreach (Character forChar in listOfChars) {
				forChar.canBeTalkedTo = true;
			}
		}else
			throw new System.ArgumentException ("Clue " + clueName + " has already been collected. This is strictly not allowed.");
	}

	/// <summary>
	/// Encounter the specified character by adding name to encounteredCharacterNames if not there already.
	/// </summary>
	/// <param name="character">Character.</param>
	public void encounter(Character character){
		if(!this.encounteredCharacterNames.Contains(character.name)){
			Debug.Log (character.name + " ENCOUNTERED");
			encounteredCharacterNames.Add (character.name);
		}
	}
}

