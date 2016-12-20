using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controlling the scene transitions as the detective moves around the Ron Cooke Hub
/// </summary>
public class SceneTransitions : MonoBehaviour {
	/// <summary>
	/// Will hold the transform position of Detective if the character tries to walk out of bounds
	/// </summary>
	private Vector3 pos;
	/// <summary>
	/// Fades to black and then loads a scene.
	/// </summary>
	/// <returns>Yield WaitForSeconds</returns>
	/// <param name="scene">Scene index to load</param>
	IEnumerator fadeLoadScene(int scene){
		GameObject overlayPanel = GameObject.Find ("OverlayPanel");
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 1f, false);
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene (scene);
	}



	private void stopDetective(bool directionIsRight, Collider detective){
		if (directionIsRight) {
			detective.GetComponent<Detective> ().canWalkRight = false;
			pos = detective.transform.position;
			pos.x = 6.8f;
			detective.transform.position = pos;
		} else {
			detective.GetComponent<Detective> ().canWalkLeft = false;
			pos = detective.transform.position;
			pos.x = -7.5f;
			detective.transform.position = pos;
		}
	}

	/// <summary>
	/// Raises the mouse down event for use when DoorQuads are clicked.
	/// </summary>
	public void OnMouseDown(){
		if (gameObject.name == "ExitDoorQuad") {
			GameObject.Find ("Detective").GetComponent<Detective> ().walkInDirectionIsLeft = false;
			StartCoroutine (fadeLoadScene (3));
		} else if (gameObject.name == "KitchenDoorQuad") {
			GameObject.Find ("Detective").GetComponent<Detective> ().walkInDirectionIsLeft = false;
			StartCoroutine (fadeLoadScene (7));
		}
	}
	/// <summary>
	/// Handles what happens when player walks into a transparent collider at the edge of either side of the scene.
	/// </summary>
	/// <param name="detective">Passed by the colliderm; the GameObject that has caused the collision.</param>
	void OnTriggerEnter(Collider detective){
		if (Time.timeSinceLevelLoad > 0.2){	//enough time for them to walk on screen
			switch (this.gameObject.name) {
			case "Room1L":
				stopDetective(false,detective);
				break;

			case "Room1R":
				if (GameObject.Find ("SceneController").GetComponent<Room1Controller> ().canProgress ()){
					detective.GetComponent<Detective> ().walkInDirectionIsLeft = true;
					StartCoroutine (fadeLoadScene (4));
				} else {
					stopDetective(true,detective);
				}
				break;

			case "Room2L": //room2 is lobby
				detective.GetComponent<Detective>().walkInDirectionIsLeft = false;
				StartCoroutine(fadeLoadScene(6)); // load cafe
				break;

			case "Room2R":
				detective.GetComponent<Detective>().walkInDirectionIsLeft = true;
				StartCoroutine(fadeLoadScene(5)); // load train station
				break;

			case "Room3L": //room3 is train station
				detective.GetComponent<Detective>().walkInDirectionIsLeft = false;
				StartCoroutine(fadeLoadScene(4)); // load lobby
				break;

			case "Room3R":
				detective.GetComponent<Detective>().walkInDirectionIsLeft = true;
				StartCoroutine(fadeLoadScene(5)); // load ...
				break;

			case "Room4L": //room4 is cafe
				detective.GetComponent<Detective>().walkInDirectionIsLeft = false;
				StartCoroutine(fadeLoadScene(9)); // load ...
				break;

			case "Room4R":
				detective.GetComponent<Detective>().walkInDirectionIsLeft = true;
				StartCoroutine(fadeLoadScene(4)); // load lobby
				break;
			case "Room5L": //room 5 is kitchen
				stopDetective(false,detective);
				break;

			case "Room5R":
				detective.GetComponent<Detective>().walkInDirectionIsLeft = true;
				StartCoroutine(fadeLoadScene(6)); // load cafe
				break;

			default:
				break;
			}
		}
	}
}