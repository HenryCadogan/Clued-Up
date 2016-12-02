using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public List<Clue> collectedClues;

	public bool isCollected(Clue clue){
		return collectedClues.Contains (clue);
	}

	public void collect(Clue clue){
		//adds name of clue to clue array
		Debug.Log (clue.name + " COLLECTED");
		collectedClues.Add (clue);
	}

	void start(){
		//initialises inventory list
		collectedClues = new List<Clue>();
	}
}

