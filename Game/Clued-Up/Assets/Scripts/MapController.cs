using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MapController : MonoBehaviour {
	
	List<int> visitedRooms;
	List<Button> mapButtons;

	public Story story;

	void start(){
		//getting a copy of the story
	}

	void showPanel(){
		//set only the visited room buttons to enabled
		//foreach (GameObject button in GameObject.FindGameObjectsWithTag("MapMenuButtons")){
		//	mapButtons.Add(button.GetComponent<Button>);
		//}
		//show the mapPanel
	}
		

	public void loadScene(int buildIndex){
		story = gameObject.GetComponent<Story>();
		visitedRooms = story.getVisitedRooms();
		print (visitedRooms.ToString ());
		SceneTransitions sceneTransitions = gameObject.GetComponent<SceneTransitions>();
		//print (sceneTransitions.name);
		//making the panel dissapear so the game looks nice when fading out to a new scene
		Time.timeScale = 1;
		print("Fading panel");
		gameObject.GetComponent<CanvasGroup> ().alpha = 0;
		//call method to start loading scene coroutine
		sceneTransitions.fadeAndLoadScene(buildIndex);

	}

}

	
