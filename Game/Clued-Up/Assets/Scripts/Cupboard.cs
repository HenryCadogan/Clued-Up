﻿using UnityEngine;
using System.Collections;
/// <summary>
/// A class for the cupboard object in the kitchen.
/// </summary>
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
	/// <summary>
	/// A reference to the main HUD of the scene of the cupboard.
	/// </summary>
	private HUDController HUDC;
	/// <summary>
	/// The GameObject of the clue held inside the cupboard.
	/// </summary>
	private GameObject containedClue;
	/// <summary>
	/// The offset from which the clue should spawn in from.
	/// </summary>
	private Vector3 cluePosOffset;
	/// <summary>
	/// A state storing whether or not the mouse cursor is placed over the cupboard or not.
	/// </summary>
	private bool entered;

	/// <summary>
	/// Changes the sprite when opened.
	/// </summary>
	private void changeSprite(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/" + this.name + "-open");
	}
	/// <summary>
	/// Plays the sound of the AudioSource component of the cupboard.
	/// </summary>
	private void playSound(){
		gameObject.GetComponent<AudioSource> ().clip = Resources.Load<AudioClip> ("Sounds/" + this.name);
		gameObject.GetComponent<AudioSource> ().Play ();
	}
	/// <summary>
	/// Opens the cupboard to view contents & output to HUD text.
	/// </summary>
	private void open (){
		this.isOpen = true;
		playSound ();
		changeSprite ();
		if(hasClue){
			HUDC.displayHUDText ("There is something in the " + this.longName + "...");
            containedClue.GetComponent<SpriteRenderer>().enabled = true;
            containedClue.GetComponent<BoxCollider>().enabled = true;
		} else {
			HUDC.displayHUDText ("The " + this.longName + " is empty.");
		}
	}
	/// <summary>
	/// Initialise this instance.
	/// </summary>
	public void Initialise(string name, string longName, float clueOffsetx = -0.2f, float clueOffsety = 0f, float clueOffsetz =  0f){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/" + name);
		Vector2 S = this.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size; //get size of sprite
		gameObject.GetComponent<BoxCollider2D>().size = S; //change size of boxcollider to match
		this.name = name;
		this.longName = longName;
		this.HUDC = GameObject.Find("HUD").GetComponent<HUDController>();
		this.cluePosOffset = new Vector3(clueOffsetx,clueOffsety,clueOffsetz);
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
	/// <summary>
	/// Adds clue to be revealed when furniture clicked.
	/// </summary>
	/// <param name="clueObject">Clue object.</param>
	public void addClue(GameObject clueObject){
		this.hasClue = true;
		clueObject.transform.position = this.gameObject.transform.position + cluePosOffset;
        clueObject.GetComponent<SpriteRenderer>().enabled = false; // hides clue behind door
        clueObject.GetComponent<BoxCollider>().enabled = false; // so you can't click it
		this.containedClue = clueObject;

	}




	void OnMouseEnter(){
		Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		entered = true;
	}
	void OnMouseExit(){
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		entered = false;
	}

	void Update(){
		if (entered) {
			Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		}
	}
}
