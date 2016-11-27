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
	private Image overlayPanel;
	private GameObject detectiveObject; //used to access playercontroller
	private Inventory inventory;



	public void displayClueInformation(){
		//Populates information panel & displays
		this.cluePanelName.text = this.longName;
		this.cluePanelImage.sprite = this.sprite;
		this.cluePanelDescription.text = this.description;
		this.cluePanel.SetActive (true);
		this.overlayPanel.CrossFadeAlpha (0.3f, 0f, false); //change opacity to 0.3 instantaneously
	}		

	void OnMouseDown(){
		//when object is clicked
		Debug.Log ("CLICK YOU MOTHERFUCKER!");
		if (!(inventory.isCollected (this.name))) { //if clue not already collected
			inventory.collect (this.name);
		}				

		displayClueInformation ();
	}
		

	public void initialise(string objectName, string name, string description, bool isWeapon=false, bool isMotive=false, bool disappearWhenClicked=true){
		//sets parameters and finds sprite
		this.cluePanel = GameObject.Find ("CluePanel");
		this.detectiveObject = GameObject.Find ("Detective");
		this.cluePanelDescription = GameObject.Find ("Description").GetComponent<Text> ();
		this.cluePanelName = GameObject.Find ("ClueName").GetComponent<Text> ();
		this.cluePanelImage = GameObject.Find ("ClueImage").GetComponent<Image> ();
		this.overlayPanel = GameObject.Find ("OverlayPanel").GetComponent<Image> ();
		this.inventory = GameObject.Find ("Detective").GetComponent<Inventory> ();


		this.name = objectName;
		this.longName = name;
		this.description = description;
		this.isWeapon = isWeapon;
		this.isMotive = isMotive;
		this.sprite =  Resources.Load<Sprite> ("Clues/" + objectName); //finds image in Resources with the sane name as the clue & sets
		this.GetComponent<SpriteRenderer> ().sprite = this.sprite;
	}
}
