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
	/// Returns a boolean if the parameter clue is already in the inventory or not
	/// </summary>
	/// <returns><c>true</c>, if collected was ised, <c>false</c> otherwise.</returns>
	/// <param name="clue">Clue.</param>
	public bool isCollected(Clue clue){
		return collectedClues.Contains (clue);
	}

	/// <summary>
	/// Collect the specified clue into the inventory
	/// </summary>
	/// <param name="clue">Clue.</param>
	public void collect(Clue clue){
		//adds name of clue to clue array
		Debug.Log (clue.name + " COLLECTED");
		collectedClues.Add (clue);
	}


	/// <summary>
	//initialises inventory list
	/// </summary>
	void start(){
		collectedClues = new List<Clue>();
	}
}

