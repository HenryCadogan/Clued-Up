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
	public List<Clue> collectedClues;

	/// <summary>
	/// <true> if the specified clue has been collected
	/// </summary>
	/// <returns><c>true</c>, if specified clue is in the inventory, <c>false</c> otherwise.</returns>
	/// <param name="clue">Clue.</param>
	public bool isCollected(Clue clue){
		return collectedClues.Contains (clue);
	}
	/// <summary>
	/// Collect the specified clue by adding it to the inventory list
	/// </summary>
	/// <param name="clue">The clue to collect</param>
	public void collect(Clue clue){
		Debug.Log (clue.name + " COLLECTED");
		collectedClues.Add (clue);
	}

	/// <summary>
	/// Initialises inventory list
	/// </summary>
	void start(){
		collectedClues = new List<Clue>();
	}
}

