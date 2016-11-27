using UnityEngine;
using System.Collections;

public class Clue : MonoBehaviour {

	public string longName; //name used in notebook etc. not to be confused with this.name which is the GameObject name
	public string description;
	public bool isWeapon;
	public bool isMotive;

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
