using UnityEngine;
using System.Collections;
public class Bin : MonoBehaviour {
	/// <summary>
	/// <c>T</c> if clue is in the sofa
	/// </summary>
	private bool hasClue = false;
	private HUDController HUDC;
	private GameObject containedClue;
	private bool entered;

	/// <summary>
	/// Opens the cupboard to view contents & output to HUD text.
	/// </summary>
	private void showClue (){
		HUDC.displayHUDText ("You managed to pull something out of the bin!");
		containedClue.SetActive (true); //make clue visible
	}
	/// <summary>
	/// Initialise this instance.
	/// </summary>
	public void Initialise(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/bin");
		Vector2 S = this.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size; //get size of sprite
		gameObject.GetComponent<BoxCollider2D>().size = S; //change size of boxcollider to match
		this.HUDC = GameObject.Find("HUD").GetComponent<HUDController>();
	}
	/// <summary>
	/// Raises the mouse down event. If sofa has clue then show.
	/// </summary>
	void OnMouseDown(){
		if ((this.hasClue)){
			this.showClue ();
			this.gameObject.GetComponent<AudioSource> ().Play ();
			this.hasClue = false;
		} else {
			HUDC.displayHUDText ("Nothing worth collecting in here. Unless you have a crisp packet collection.");
		}
	}

	public void addClue(GameObject clueObject){
		this.hasClue = true;
		clueObject.transform.position = this.gameObject.transform.position + new Vector3(0f, 1.2f, 0f);
		clueObject.SetActive (false);
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
