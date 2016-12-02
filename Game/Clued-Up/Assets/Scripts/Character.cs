using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	public string longName;
	public bool isMurderer;
	public bool isVictim;
	public List<Clue> characterClues;

	void Awake ()
	//Makes the object persistant throughout scenes
	{
		DontDestroyOnLoad(gameObject);

	}

	public void initialise(string objectName, string name, bool isMurderer=false, bool isVictim=false){
		this.gameObject.name = objectName;
		this.longName = name;
		this.isMurderer = isMurderer;
		this.isVictim = isVictim;

		//gameObject.GetComponent<Renderer> ().enabled = false; //dont draw any characters yet

		//TODO set up character clues
}
}
