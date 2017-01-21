using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// The clue class
/// </summary>

public class Clue : MonoBehaviour {
	/// <summary>
	/// //Long name displayed in game (i.e with spaces). Not to be confused with this.name which is the GameObject name
	/// </summary>
	public string longName;
	/// <summary>
	/// The description of the clue
	/// </summary>
	public string description;
	/// <summary>
	/// The 2D sprite of the clue
	/// </summary>
	public Sprite sprite;
	/// <summary>
	/// <c>True<c>/ if clue is weapon
	/// </summary>
	public bool isWeapon;
	/// <summary>
	/// if this clue is the motive clue
	/// </summary>
	public bool isMotive;
	/// <summary>
	/// if this item should dissapear from the game when collected
	/// </summary>
	public bool disappearWhenClicked; 
	/// <summary>
	/// <c>True<c>/ Clue has been collected
	/// </summary>
	public bool isCollected = false;


	/// <summary>
	/// The clue description panel to be populated & shown when collected.
	/// </summary>
	private GameObject cluePanel;
	/// <summary>
	/// The clue title GameObject within the clue panel.
	/// </summary>
	private Text cluePanelName;
	/// <summary>
	/// The description GameObject within the clue panel.
	/// </summary>
	private Text cluePanelDescription;
	/// <summary>
	/// The image GameObject within CluePanel.
	/// </summary>
	private Image cluePanelImage;
	/// <summary>
	/// The overlay panel to be used for fading.
	/// </summary>
	private GameObject overlayPanel;
	/// <summary>
	/// The heads up display prefab
	/// </summary>
	private GameObject hud;
	private bool entered;
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Clue"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Clue"/>.</returns>
	public override string ToString ()
	{
		return this.longName;
	}

	/// <summary>
	/// Populates and displays the clue information on screen.
	/// </summary>
	public void displayClueInformation(){
		this.cluePanel = GameObject.Find ("CluePanel");
		this.cluePanelDescription = GameObject.Find ("Description").GetComponent<Text> ();
		this.cluePanelName = GameObject.Find ("ClueName").GetComponent<Text> ();
		this.cluePanelImage = GameObject.Find ("ClueImage").GetComponent<Image> ();
		this.overlayPanel = GameObject.Find ("OverlayPanel");

		this.cluePanelName.text = this.longName;
		this.cluePanelImage.sprite = this.sprite;
		this.cluePanelDescription.text = this.description;
		this.cluePanel.GetComponent<CanvasGroup>().alpha = 1f; //change opacity to 0.3 instantaneously

		this.overlayPanel.GetComponent<CanvasGroup> ().alpha = 0.5f; //make overlay canvas group visible
		this.overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 0f, false); //instantaneously fade overlay panel. This is needed becuse initial fade leaves <Image> alpha at 0
		this.overlayPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true; //make it so nothing else on screen can be interacted with while overlay panel is on

	}		
	/// <summary>
	/// Activated when GameObject clicked on. If it's the first time, send message to HUD, add it to inventory, display clue.
	/// </summary>
	void OnMouseDown(){
		Inventory inventory = GameObject.Find ("Detective").GetComponent<Inventory> ();
		if (Time.timeScale != 0) {	//if game isn't paused
			if (!(inventory.isCollected (this.name))) {
				inventory.collect (this);
				hud.GetComponent<HUDController> ().displayHUDText (this.longName + " added to inventory.");
				this.isCollected = true;

				if (this.name == "chalkOutline") { // add victim to character notebook
					inventory.encounter (GameObject.Find ("Story").GetComponent<Story> ().getVictim ());
				}				
				displayClueInformation ();

				if (this.disappearWhenClicked) {
					gameObject.GetComponent<Renderer> ().enabled = false;	//hides but does not destroy the clue
				}
			}
		}
	}
	/// <summary>
	/// Initialise the Clue with properties and finds sprite with same name as this clue which it also scales.
	/// </summary>
	/// <param name="objectName">Object name of the clue</param>
	/// <param name="name">Long name to be displayed in game</param>
	/// <param name="description">Description to be displayed in the game</param>
	/// <param name="isWeapon">If <c>true</c> then the clue is the murder weapon</param>
	/// <param name="isMotive">If <c>true</c> then this clue is motive.</param>
	/// <param name="disappearWhenClicked">If set to <c>true</c>, the clue will disappear when clicked.</param>
	public void initialise(string objectName, string name, string description, bool isWeapon=false, bool isMotive=false, bool disappearWhenClicked=true, float localScale = 0.25f){
		this.hud = GameObject.Find ("HUD");

		this.name = objectName;
		this.longName = name;
		this.description = description;
		this.isWeapon = isWeapon;
		this.isMotive = isMotive;
		this.disappearWhenClicked = disappearWhenClicked;
		this.sprite =  Resources.Load<Sprite> ("Clues/" + objectName); //finds image in Resources with the same name as the clue & sets

		this.transform.localScale = new Vector3(localScale,localScale,localScale);
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
