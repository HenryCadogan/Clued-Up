using UnityEngine;
using System.Collections;
public class Lockers : MonoBehaviour {
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
		if(openLockers[locker] == 0){ //if locker is not open 
			this.openLockers[locker] = 1;
			changeLockerSprite();
			if (clueIsInLocker == locker) {
				HUDC.displayHUDText ("There is something in the locker...");
			} else {
				HUDC.displayHUDText ("The locker is empty.");
			}
		}
	}
	/// <summary>
	/// Initialise this instance.
	/// </summary>
	public void Initialise(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/lockers");
		Vector2 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size; //get size of sprite
		gameObject.GetComponent<BoxCollider2D>().size = S; //change size of boxcollider to match
		this.name = "lockers";
		this.HUDC = GameObject.Find("HUD").GetComponent<HUDController>();
	}

	void OnMouseDown(){
		float mousex = Input.mousePosition.x;
		float mousey = Input.mousePosition.y;
		if ((mousex > 700f) && (mousex < 740f)) { // column1
			if ((mousey > 195f) && (mousey < 275f)) {	//column1 row 1
				openLocker(0);
			} else if ((mousey > 120f) && (mousey < 195f)) { //column1 row 2
				openLocker(2);
			}
		}else if ((mousex > 765f) && (mousex < 815f)) { // column2
			if ((mousey > 195f) && (mousey < 275f)) {	//column2 row 1
				openLocker(1);
			} else if ((mousey > 120f) && (mousey < 195f)) { //column2 row 2
				openLocker(3);
			}
		} 
	}

	public void addClue(GameObject clueObject){
		this.clueIsInLocker = Random.Range (0, 4);
		switch (this.clueIsInLocker) {
		case 0:
			///clueObject.transform.position.Set(
			break;
		default:
			break;
		}
	}
}
