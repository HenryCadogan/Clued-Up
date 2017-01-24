using UnityEngine;
using System.Collections;
/// <summary>
/// A class for the lockers that are used in the lobby of the RCH.
/// </summary>
public class Lockers : MonoBehaviour {

	/// <summary>
	/// Used in Lockers component of the GameObject children of Locker prefab to distinguish door colliders.
	/// </summary>
	public int doorID;
	/// <summary>
	/// The AudioSource component of the lockers.
	/// </summary>
	public AudioSource sfxSource;
	/// <summary>
	/// 0 means locker is closed, 1 = locker is open
	/// </summary>
	private int[] openLockers = {0,0,0,0};
	/// <summary>
	/// The clue is in locker number... (-1 = no clue)
	/// </summary>
	private int clueIsInLocker=-1;
	/// <summary>
	/// For outputting to the hud text
	/// </summary>
	private HUDController HUDC;
	/// <summary>
	/// The clue contained in the lockers.
	/// </summary>
	private GameObject containedClue;
	/// <summary>
	/// A state storing whether or not the mouse cursor is placed over the beer pump or not.
	/// </summary>
	private bool entered;

	/// <summary>
	/// Changes the locker sprite when one is opened.
	/// </summary>
	private void changeLockerSprite(){
		string imageName = "Furniture/lockers" + openLockers[0].ToString() + openLockers[1].ToString() + openLockers[2].ToString() + openLockers[3].ToString();
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(imageName);
	}
	/// <summary>
	/// Opens the locker to view contents & output to HUD text.
	/// </summary>
	/// <param name="locker">Locker.</param>
	private void openLocker (int locker){
		if (openLockers [locker] == 0) { //if locker is not open 
			this.openLockers [locker] = 1;
			changeLockerSprite ();
			sfxSource.Play ();
			if ((clueIsInLocker == locker) && (!containedClue.GetComponent<Clue> ().isCollected)) {
				HUDC.displayHUDText ("There is something in the locker...");
				containedClue.SetActive (true); //make clue visible
			} else {
				HUDC.displayHUDText ("The locker is empty.");
			}
		} else if ((clueIsInLocker == locker) && (!containedClue.GetComponent<Clue> ().isCollected)) {
			HUDC.displayHUDText ("There is something in the locker...");
		} else {
			HUDC.displayHUDText ("The locker is empty.");
		}
	}
	/// <summary>
	/// Resizes the door colliders.
	/// </summary>
	/// <param name="spriteSize">Sprite size.</param>
	private void resizeDoorColliders(Vector2 spriteSize){
		Vector3 pos = new Vector3();
		pos.x = -0.5f;
		pos.y = 0.6f;
		gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().size = spriteSize; //change size of boxcollider to match
		gameObject.transform.GetChild(0).localPosition = pos;
		pos.x = 0.55f;
		pos.y = 0.6f;
		gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>().size = spriteSize; //change size of boxcollider to match
		gameObject.transform.GetChild(1).localPosition = pos;
		pos.x = -0.5f;
		pos.y = -0.63f;
		gameObject.transform.GetChild(2).GetComponent<BoxCollider2D>().size = spriteSize; //change size of boxcollider to match
		gameObject.transform.GetChild(2).localPosition = pos;
		pos.x = 0.55f;
		pos.y = -0.63f;
		gameObject.transform.GetChild(3).GetComponent<BoxCollider2D>().size = spriteSize; //change size of boxcollider to match
		gameObject.transform.GetChild(3).localPosition = pos;

	}

	/// <summary>
	/// Initialise this instance.
	/// </summary>
	public void Initialise(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/lockers");
		Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size; //get size of sprite
		spriteSize.x *= 0.3f;
		spriteSize.y *= 0.4f;
		resizeDoorColliders (spriteSize);

		this.name = "lockers";
		this.HUDC = GameObject.Find("HUD").GetComponent<HUDController>();
}
	/// <summary>
	/// Adds clue to one of the lockers at random.
	/// </summary>
	/// <param name="clueObject">Clue object.</param>
	public void addClue(GameObject clueObject){
		this.clueIsInLocker = Random.Range (0, 4);
		switch (this.clueIsInLocker) {
		case 0:
			clueObject.transform.position = new Vector3(2.5f, -3.3f, 1f);
			break;
		case 1:
			clueObject.transform.position = new Vector3(3.95f, -3.5f, 1f);
			break;
		case 2:
			clueObject.transform.position = new Vector3(2.5f, -5f, 1f);
			break;
		case 3:
			clueObject.transform.position = new Vector3(3.95f, -5f, 1f);
			break;
		default:
			break;
		}
		clueObject.SetActive (false); // hides clue behind door
		this.containedClue = clueObject;

	}


	/// <summary>
	/// OnMouseDown of a child (door) collider, the parent method of openLocker is called with the child (door)'s ID.
	/// </summary>
	void OnMouseDown(){
		this.gameObject.transform.parent.GetComponent<Lockers> ().openLocker (this.doorID);
		Destroy (this.gameObject);
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
