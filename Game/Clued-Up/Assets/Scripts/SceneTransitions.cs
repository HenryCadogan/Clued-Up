using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controlling the scene transitions as the detective moves around the Ron Cooke Hub
/// Info:
///     Room1 = Lakehouse
///     Room2 = Lobby
///     Room3 = TrainStation
///     Room4 = Cafe
///     Room5 = Kitchen
///     Room6 = Bar
///     Room7 = Studio
///     Room8 = Toilets
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
	IEnumerator fadeLoadScene(string scene){
		GameObject overlayPanel = GameObject.Find ("OverlayPanel");
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 1f, false);
		yield return new WaitForSeconds (1);
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto); //resets curser if stuck on magnifying glass
		SceneManager.LoadScene (scene);
		yield return null;
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
		if (gameObject.name == "LobbyDoorQuad") {
			GameObject.Find ("Detective").GetComponent<Detective> ().walkInDirection = 2;
			StartCoroutine (fadeLoadScene ("Room1"));
		} else if (gameObject.name == "KitchenDoorQuad") {
			GameObject.Find ("Detective").GetComponent<Detective> ().walkInDirection = 2;
			StartCoroutine (fadeLoadScene ("Room5"));
		} else if (gameObject.name == "StudioDoorQuad") {
			GameObject.Find ("Detective").GetComponent<Detective> ().walkInDirection = 2;
			StartCoroutine (fadeLoadScene ("Room7"));
		} else if (gameObject.name == "StudioExitDoorQuad") {
			GameObject.Find ("Detective").GetComponent<Detective> ().walkInDirection = 0;
			StartCoroutine (fadeLoadScene ("Room6"));
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
				if (GameObject.Find ("SceneController").GetComponent<RoomController> ().canProgress ()){
					detective.GetComponent<Detective> ().walkInDirection = 1;
					StartCoroutine (fadeLoadScene ("Room2"));
				} else {
					stopDetective(true,detective);
				}
				break;

			case "Room2L": //room2 is lobby
				detective.GetComponent<Detective>().walkInDirection = 2;
				StartCoroutine(fadeLoadScene("Room4")); // load cafe
				break;

			case "Room2R":
				detective.GetComponent<Detective>().walkInDirection = 0;
				StartCoroutine(fadeLoadScene("Room3")); // load train station
				break;

			case "Room3L": //room3 is train station
				detective.GetComponent<Detective>().walkInDirection = 2;
				StartCoroutine(fadeLoadScene("Room2")); // load lobby
				break;

			case "Room3R":
				detective.GetComponent<Detective>().walkInDirection = 0;
				StartCoroutine(fadeLoadScene("Room8")); // load toilets
				break;

			case "Room4L": //room4 is cafe
				detective.GetComponent<Detective>().walkInDirection = 2;
				StartCoroutine(fadeLoadScene("Room6")); // load bar
				break;

			case "Room4R":
				detective.GetComponent<Detective>().walkInDirection = 0;
				StartCoroutine(fadeLoadScene("Room2")); // load lobby
				break;

			case "Room5L": //room 5 is kitchen
				stopDetective(false,detective);
				break;

			case "Room5R":
				detective.GetComponent<Detective>().walkInDirection = 0;
				StartCoroutine(fadeLoadScene("Room4")); // load cafe
				break;

			case "Room6L": //room 6 is bar
				detective.GetComponent<Detective>().walkInDirection = 2;
				StartCoroutine(fadeLoadScene("Room8")); // load toilets
				break;

			case "Room6R":
				detective.GetComponent<Detective>().walkInDirection = 0;
				StartCoroutine(fadeLoadScene("Room4")); // load cafe
				break;

			case "Room7L": //room 7 is studio
				stopDetective(false, detective);
				break;

			case "Room7R":
				stopDetective (true, detective);
				break;

			case "Room8L": //room 8 is toilets
				detective.GetComponent<Detective>().walkInDirection = 2;
				StartCoroutine(fadeLoadScene("Room3")); // load station
				break;

			case "Room8R":
				detective.GetComponent<Detective>().walkInDirection = 0;
				StartCoroutine(fadeLoadScene("Room6")); // load bar
				break;

			default:
				break;
			}
		}
	}
	/// <summary>
	/// Starts the scene transition coroutine. This is used from outside this class to load a new scene
	/// </summary>
	/// <param name="buildIndex">Build index.</param>
	public void startSceneTransition(string sceneName){
		StartCoroutine(fadeLoadScene(sceneName));
	}


	public void returnToMainMenu(){
		Time.timeScale = 1; //so that future coroustines work
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);

		if (GameObject.Find ("Detectives") != null) {
			Destroy (GameObject.Find ("Detectives"));
		}

		if (GameObject.Find ("Story") != null) {
			foreach (GameObject character in GameObject.Find("Story").GetComponent<Story>().getFullCharacterList()) {
				Destroy (character);
			}
			Destroy (GameObject.Find ("Story").GetComponent<Story> ().victim);
			Destroy (GameObject.Find ("Story"));
		}
		SceneManager.LoadScene ("MainMenu");
	}
}