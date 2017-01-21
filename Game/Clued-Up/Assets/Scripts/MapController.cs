using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapController : MonoBehaviour {
	/// <summary>
	/// Property used in Button component to indicate which room it will load.
	/// </summary>
	public int buttonIsForRoomIndex;

	/// <summary>
	/// Called when map button is clicked. Iterates through each button, and enables it if it has already been visited. Also stops time.
	/// </summary>
	public void updateMapButtons(){
		Time.timeScale = 0;
		Story story = GameObject.Find ("Story").GetComponent<Story>();
		foreach (GameObject button in GameObject.FindGameObjectsWithTag("MapButtons")) {
			if (story.isVisited (button.GetComponent<MapController> ().buttonIsForRoomIndex)) {
				button.GetComponent<Button> ().interactable = true;
			}
		}
	}
	
	/// <summary>
	/// Hides map panel & loads the next scene (buildIndex = roomIndex + 3).
	/// </summary>
	/// <param name="roomIndex">Room index given by component.</param>
	public void loadScene(int roomIndex){
		gameObject.GetComponent<CanvasGroup>().alpha = 0;
		Time.timeScale = 1;
		gameObject.GetComponent<SceneTransitions>().startSceneTransition(roomIndex + 3);
	}
}
