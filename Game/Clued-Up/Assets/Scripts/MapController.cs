using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// A class which enables the enabling of buttons if the detective has been to their rooms.
/// </summary>
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
		Detective detective = GameObject.Find ("Detective").GetComponent<Detective>();
		foreach (GameObject button in GameObject.FindGameObjectsWithTag("MapButtons")) {
			if (detective.isVisited (button.GetComponent<MapController> ().buttonIsForRoomIndex)) {
				button.GetComponent<Button> ().interactable = true;
			}
		}
	}
	
	/// <summary>
	/// Hides map panel & loads the next scene (buildIndex = roomIndex + 3).
	/// </summary>
	/// <param name="roomIndex">Room index given by component.</param>
	public void loadScene(string sceneName){
		gameObject.GetComponent<CanvasGroup>().alpha = 0;
		Time.timeScale = 1;
		gameObject.GetComponent<SceneTransitions>().startSceneTransition(sceneName);
	}
}
