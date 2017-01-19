using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MapController : MonoBehaviour {
	
	Story story;
	List<int> visitedRooms;
	List<Button> mapButtons;
	Dictionary<int,string> RoomBuildLUT;

	void start(){
		//getting a copy of the story
		story = GameObject.Find("Story").GetComponent<Story>();
		//add all scenes into the LUT
		for(int x = 3; x < SceneManager.sceneCount - 3; x++) {
			RoomBuildLUT.Add(x, SceneManager.GetSceneAt(x).name);
		}
		print(RoomBuildLUT.ToString());
	}

	void showPanel(){
		//set onyl the visited room buttons to enabled
		//foreach (GameObject button in GameObject.FindGameObjectsWithTag("MapMenuButtons")){
		//	mapButtons.Add(button.GetComponent<Button>);
		//}
		//show the mapPanel
	}
		

	public void loadScene(int buildIndex){
		//visitedRooms = story.getVisitedRooms();
		SceneTransitions sceneTransitions = gameObject.GetComponent<SceneTransitions>();
		print (sceneTransitions.name);
		//making the panel dissapear so the game looks nice when fading out to a new scene
		Time.timeScale = 1;
		print("Fading panel");
		gameObject.GetComponent<CanvasGroup> ().alpha = 0;
		//call method to start loading scene coroutine
		sceneTransitions.fadeAndLoadScene(buildIndex);

	}

}

	
