﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// The clue class
/// </summary>

public class Clue : MonoBehaviour {
	/// <summary>
	/// //name used in notebook etc. not to be confused with this.name which is the GameObject name
	/// </summary>
	public string longName;
	/// <summary>
	/// The description of the clue
	/// </summary>
	public string description;
	/// <summary>
	/// The sprite of the clue
	/// </summary>
	public Sprite sprite;
	/// <summary>
	/// Boolean if this clue is a weapon
	/// </summary>
	public bool isWeapon;
	/// <summary>
	/// Boolean if this clue is the motive clue
	/// </summary>
	public bool isMotive;
	/// <summary>
	/// Boolean if this item should dissapear from the game when clicked on
	/// </summary>
	public bool disappearWhenClicked; //Clue will vanish when collected


	/// <summary>
	/// The clue panel.
	/// </summary>
	private GameObject cluePanel;
	/// <summary>
	/// The name of the clue panel.
	/// </summary>
	private Text cluePanelName;
	/// <summary>
	/// The clue panel description.
	/// </summary>
	private Text cluePanelDescription;
	/// <summary>
	/// The clue panel image.
	/// </summary>
	private Image cluePanelImage;
	/// <summary>
	/// The overlay panel.
	/// </summary>
	private GameObject overlayPanel;
	/// <summary>
	/// The inventory.
	/// </summary>
	private Inventory inventory;
	/// <summary>
	/// The heads up display
	/// </summary>
	private GameObject hud;

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Clue"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Clue"/>.</returns>
	public override string ToString ()
	{
		return this.longName;
	}

	/// <summary>
	/// Displays the clue information.
	/// </summary>
	public void displayClueInformation(){
		//Populates information panel & displays
		this.cluePanelName.text = this.longName;
		this.cluePanelImage.sprite = this.sprite;
		this.cluePanelDescription.text = this.description;
		this.cluePanel.GetComponent<CanvasGroup>().alpha = 1f; //change opacity to 0.3 instantaneously

		this.overlayPanel.GetComponent<CanvasGroup> ().alpha = 0.5f; //make overlay canvas group visible
		this.overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 0f, false); //instantaneously fade overlay panel. This is needed becuse initial fade leaves <Image> alpha at 0
		this.overlayPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true; //make it so nothing else on screen can be interacted with while overlay panel is on

	}		

	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	void OnMouseDown(){
		//when object is clicked for the first time, send message to HUD, add it to inventory, display clue. Destroys object afterwards if needed
		if (Time.timeScale != 0) {	//if game isn't paused
			if (!(inventory.isCollected (this))) {
				inventory.collect (this);
				hud.GetComponent<HUDController> ().displayHUDText (this.longName + " added to inventory.");
			}				
			displayClueInformation ();

			if (this.disappearWhenClicked) {
				gameObject.GetComponent<Renderer>().enabled = true;	//hides but does not destroy the clue
			}
		}
	}
		
	/// <summary>
	/// Initialise the specified objectName, name, description, isWeapon, isMotive and disappearWhenClicked.
	/// </summary>
	/// <param name="objectName">Object name.</param>
	/// <param name="name">Name.</param>
	/// <param name="description">Description.</param>
	/// <param name="isWeapon">If set to <c>true</c> is weapon.</param>
	/// <param name="isMotive">If set to <c>true</c> is motive.</param>
	/// <param name="disappearWhenClicked">If set to <c>true</c> disappear when clicked.</param>
	public void initialise(string objectName, string name, string description, bool isWeapon=false, bool isMotive=false, bool disappearWhenClicked=false){
		//sets parameters and finds sprite
		this.cluePanel = GameObject.Find ("CluePanel");
		this.cluePanelDescription = GameObject.Find ("Description").GetComponent<Text> ();
		this.cluePanelName = GameObject.Find ("ClueName").GetComponent<Text> ();
		this.cluePanelImage = GameObject.Find ("ClueImage").GetComponent<Image> ();
		this.overlayPanel = GameObject.Find ("OverlayPanel");
		this.inventory = GameObject.Find ("Detective").GetComponent<Inventory> ();
		this.hud = GameObject.Find ("HUD");


		this.name = objectName;
		this.longName = name;
		this.description = description;
		this.isWeapon = isWeapon;
		this.isMotive = isMotive;
		this.disappearWhenClicked = disappearWhenClicked;
		this.sprite =  Resources.Load<Sprite> ("Clues/" + objectName); //finds image in Resources with the same name as the clue & sets
		this.GetComponent<SpriteRenderer> ().sprite = this.sprite;
	}
}
