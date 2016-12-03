using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Clue : MonoBehaviour {

	public string longName; //name used in notebook etc. not to be confused with this.name which is the GameObject name
	public string description;
	public Sprite sprite;
	public bool isWeapon;
	public bool isMotive;
	public bool disappearWhenClicked; //Clue will vanish when collected

	private GameObject cluePanel;
	private Text cluePanelName;
	private Text cluePanelDescription;
	private Image cluePanelImage;
	private GameObject overlayPanel;
	private Inventory inventory;
	private GameObject hud;
		
	public override string ToString ()
	{
		return this.longName;
	}

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
