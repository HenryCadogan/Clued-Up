using UnityEngine;
using System.Collections;
/// <summary>
/// The controller for the clues NOT SURE IF THIS IS BEING USED. IF READING THIS, PLEASE CHECK AND OR DELETE
/// </summary>
public class ClueController : MonoBehaviour {
	/// <summary>
	/// //name used in notebook etc. not to be confused with this.name which is the GameObject name
	/// </summary>
	public string longName;
	/// <summary>
	/// The description.
	/// </summary>
	public string description;
	/// <summary>
	/// Boolean if this is a weapon
	/// </summary>
	public bool isWeapon;
	/// <summary>
	/// Boolean if this is the motive clue
	/// </summary>
	public bool isMotive;

	/// <summary>
	/// Initialise the specified objectName, name, description, isWeapon and isMotive.
	/// </summary>
	/// <param name="objectName">Object name.</param>
	/// <param name="name">Name.</param>
	/// <param name="description">Description.</param>
	/// <param name="isWeapon">If set to <c>true</c> is weapon.</param>
	/// <param name="isMotive">If set to <c>true</c> is motive.</param>
	public void initialise(string objectName, string name, string description, bool isWeapon=false, bool isMotive=false){
		//sets parameters and finds sprite
		this.name = objectName;
		this.longName = name;
		this.description = description;
		this.isWeapon = isWeapon;
		this.isMotive = isMotive;

		this.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Clues/" + objectName);
	}
}
