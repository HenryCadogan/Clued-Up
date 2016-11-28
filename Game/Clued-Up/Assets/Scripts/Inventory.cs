using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public List<string> collectedClueNames;

	public bool isCollected(string name){
		return collectedClueNames.Contains (name);
	}

	public void collect(string name){
		//adds name of clue to clue array
		Debug.Log (name + " COLLECTED");
		collectedClueNames.Add (name);
	}

	void start(){
		//initialises inventory list
		collectedClueNames = new List<string>();
	}
}

