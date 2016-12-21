using UnityEngine;
using System.Collections;
using UnityEditor;
public class Cupboard : MonoBehaviour {
	/// <summary>
	/// <c>T<c>/ if cupboard is open
	/// </summary>
	private bool isOpen= false;
	/// <summary>
	/// <c>T</c> if clue is in behind this door
	/// </summary>
	private bool hasClue = false;
	/// <summary>
	/// For example "oven"; name of the cupboard
	/// </summary>
	private string longName;
	private HUDController HUDC;
	private GameObject containedClue;

	/// <summary>
	/// Changes the sprite when opened.
	/// </summary>
	private void changeSprite(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/" + this.name + "-open");
	}
	/// <summary>
	/// Opens the cupboard to view contents & output to HUD text.
	/// </summary>
	private void open (){
		this.isOpen = true;
		changeSprite ();
		if(hasClue){
			HUDC.displayHUDText ("There is something in the " + this.longName + "...");
			containedClue.SetActive (true); //make clue visible
		} else {
			HUDC.displayHUDText ("The " + this.longName + " is empty.");
		}
	}
	/// <summary>
	/// Initialise this instance.
	/// </summary>
	public void Initialise(string name, string longName){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/" + name);
		Debug.Log ("trying to load image: " + "Furniture/" + name);
		Vector2 S = this.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size; //get size of sprite
		gameObject.GetComponent<BoxCollider2D>().size = S; //change size of boxcollider to match
		this.name = name;
		this.longName = longName;
		this.HUDC = GameObject.Find("HUD").GetComponent<HUDController>();
	}
	/// <summary>
	/// Raises the mouse down event. Opens cupboard if it isnt open already
	/// </summary>
	void OnMouseDown(){
		if (!this.isOpen) { //if cupboard is not ope
			this.open ();
		}else if ((hasClue) && (!containedClue.GetComponent<Clue> ().isCollected)) {
			HUDC.displayHUDText ("There is something in the " + this.longName + "...");
		} else {
			HUDC.displayHUDText ("The " + this.longName + " is empty.");
		}
	}

	public void addClue(GameObject clueObject){
		this.hasClue = true;
		clueObject.transform.position = this.gameObject.transform.position + new Vector3(-0.2f, 0f, 0f);
		clueObject.SetActive (false); // hides clue behind door
		this.containedClue = clueObject;

	}
}
