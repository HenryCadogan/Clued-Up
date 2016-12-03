using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>//handles what happens when player walks into a transparent collider at the edge of each side of the scene
/// Controlling the scene transitions as the detective moves around the Ron Cooke Hub
/// </summary>
public class SceneTransitions : MonoBehaviour {
	/// <summary>
	/// The overlay panel.
	/// </summary>
	public GameObject overlayPanel;
	/// <summary>
	/// The detective.
	/// </summary>
	private GameObject detective;

	/// <summary>
	/// Fades the load scene.
	/// </summary>
	/// <returns>The load scene.</returns>
	/// <param name="scene">Scene.</param>
	IEnumerator fadeLoadScene(int scene){
		//Fades overlay panel to black and loads new scene
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 1f, false);
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene (scene);	//loads lobby scene
	}

	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	public void OnMouseDown(){
		//for when buttons are used to go to a scene
		if (gameObject.name == "DoorQuad") {
			GameObject.Find ("Detective").GetComponent<Detective> ().walkInDirectionIsLeft = false;
			//this.detective.GetComponent<PlayerController>().walkInDirectionIsLeft = true;
			StartCoroutine (fadeLoadScene (3));
		}
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="detective">Detective.</param>
	void OnTriggerEnter(Collider detective){
		//for when collisions are used to detect movement
		if (Time.timeSinceLevelLoad > 0.2){	//enough time for them to walk on screen
			switch (this.gameObject.name) {

			case "Room1L":
				//will force location of detective to not go past collider
				Vector3 pos = detective.transform.position;
				pos.x = -7f;
				detective.transform.position = pos;
				break;

			case "Room1R":
				bool canProgress = true; //set conditions for progressing to room 2
				if (canProgress){
					detective.GetComponent<Detective>().walkInDirectionIsLeft = true;
					StartCoroutine(fadeLoadScene(4));
				}
				break;

			case "Room2L":
				detective.GetComponent<Detective>().walkInDirectionIsLeft = false;
				StartCoroutine(fadeLoadScene(5));
				break;

			case "Room2R":
				detective.GetComponent<Detective>().walkInDirectionIsLeft = true;
				StartCoroutine(fadeLoadScene(6));
				break;

			default:
				break;
			}
		}
	}
}