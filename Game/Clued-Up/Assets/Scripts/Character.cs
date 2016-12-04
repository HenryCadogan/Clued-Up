using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Character class
/// </summary>
public class Character : MonoBehaviour {
	/// <summary>
	/// Name to be used when displaying a name (i.e. contains spaces)
	/// </summary>
	public string longName;
	/// <summary>
	/// If character is the murderer. Set by Story.
	/// </summary>
	public bool isMurderer;
	/// <summary>
	/// If character is the victim. Set by Story.
	/// </summary>
	public bool isVictim;
	/// <summary>
	/// List of Clues attatched to this character instance
	/// </summary>
	public List<Clue> characterClues;
	/// <summary>
	/// Makes the object persistant throughout scenes
	/// </summary>
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);

	}



	/// <summary>
	/// Initialise the specified Character with properties and CharacterClues
	/// </summary>
	/// <param name="objectName">The name of the character GameObject</param>
	/// <param name="name">Long name to be displayed in game</param>
	/// <param name="isMurderer"></param><c>true</c> if character is murderer.</param>
	/// <param name="isVictim"></param><c>true</c> if character is victim.</param>
	public void initialise(string objectName, string name, bool isMurderer=false, bool isVictim=false){
		this.gameObject.name = objectName;
		this.longName = name;
		this.isMurderer = isMurderer;
		this.isVictim = isVictim;

		//gameObject.GetComponent<Renderer> ().enabled = false; //dont draw any characters yet

		//TODO set up character clues
}
}
