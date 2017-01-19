using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MapController : MonoBehaviour {
	
	Story story;
	List<int> visitedRooms;
	List<Button> mapButtons;
	SceneTransitions sceneTransitions;
	Dictionary<int,string> RoomBuildLUT;

	void start(){
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
		
		story = GameObject.Find("Story").GetComponent<Story>();
		visitedRooms = story.getVisitedRooms();


		sceneTransitions = GameObject.Find("MapPanel").GetComponent<SceneTransitions>();
		Time.timeScale = 1;

		sceneTransitions.fadeAndLoadScene(buildIndex);
	}

}

	
