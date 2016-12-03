using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Character class
/// </summary>
public class Character : MonoBehaviour {
	/// <summary>
	/// The long name.
	/// </summary>
	public string longName;
	/// <summary>
	/// Boolean if the character is the murderer
	/// </summary>
	public bool isMurderer;
	/// <summary>
	/// Boolean if this character is the victim
	/// </summary>
	public bool isVictim;
	/// <summary>
	/// List of clues based on the characters
	/// </summary>
	public List<Clue> characterClues;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	//Makes the object persistant throughout scenes
	{
		DontDestroyOnLoad(gameObject);

	}

	/// <summary>
	/// Initialise the specified objectName, name, isMurderer and isVictim.
	/// </summary>
	/// <param name="objectName">Object name.</param>
	/// <param name="name">Name.</param>
	/// <param name="isMurderer">If set to <c>true</c> is murderer.</param>
	/// <param name="isVictim">If set to <c>true</c> is victim.</param>

	public void initialise(string objectName, string name, bool isMurderer=false, bool isVictim=false){
		this.gameObject.name = objectName;
		this.longName = name;
		this.isMurderer = isMurderer;
		this.isVictim = isVictim;

		//gameObject.GetComponent<Renderer> ().enabled = false; //dont draw any characters yet

		//TODO set up character clues
}
}
