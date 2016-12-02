using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	public string longName;
	public bool isMurderer;
	public bool isVictim;
	public int location;
	public List<Clue> characterClues;

	public void initialise(string objectName, string name, bool isMurderer=false, bool isVictim=false){
		this.gameObject.name = objectName;
		this.longName = name;
		this.isMurderer = isMurderer;
		this.isVictim = isVictim;

		//TODO set up character clues
}
}
