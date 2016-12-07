using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// The Inventory class
/// </summary>

public class Inventory : MonoBehaviour {
	/// <summary>
	/// The list of collected clues.
	/// </summary>
	public List<string> collectedClueNames; //cannot store the clues themselves as they are Destroyed between scenes

	/// <summary>
	/// <true> if the specified clue has been collected
	/// </summary>
	/// <returns><c>true</c>, if specified clue is in the inventory, <c>false</c> otherwise.</returns>
	/// <param name="clue">Clue.</param>
	public bool isCollected(string clueName){
		return collectedClueNames.Contains (clueName);
	}
	/// <summary>
	/// Collect the specified clue by adding it to the inventory list
	/// </summary>
	/// <param name="clue">The clue to collect</param>
	public void collect(Clue clue){
		Debug.Log (clue.name + " COLLECTED");
		collectedClueNames.Add (clue.name);
	}

	/// <summary>
	/// Initialises inventory list
	/// </summary>
	void start(){
		collectedClueNames = new List<string>();
	}
}

