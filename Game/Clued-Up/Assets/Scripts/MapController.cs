using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {

	/// <summary>
	/// Makes a call to SceneTransitions to load the scene
	/// </summary>
	/// <param name="buildIndex">Build index of the scene to load</param>
	public Story story;

	public void showMap(){
		foreach (GameObject button in GameObject.FindGameObjectsWithTag("MapButtons")){
			button.SetActive(false);
		}

		//set the respective buttons to active if the room has been visited
		foreach (int index in story.getVisitedRooms()){
			switch(index){
			case 3: 
				GameObject.Find("OutsideButton").SetActive(true);
				break;
			case 4:
				GameObject.Find("AtriumButton").SetActive(true);
				break;
			case 5:
				GameObject.Find("TrainButton").SetActive(true);
				break;
			case 6:
				GameObject.Find("CafeButton").SetActive(true);
				break;
			case 7:
				GameObject.Find("KitchenButton").SetActive(true);
				break;
			case 8:
				GameObject.Find("BarButton").SetActive(true);
				break;
			case 9:
				GameObject.Find("RecStudioButton").SetActive(true);
				break;
			case 10:
				GameObject.Find("ToiletButton").SetActive(true);
				break;
			}
		}
	}
	

	public void loadScene(int buildIndex){
		//fade out the map panel to make it look nice
		gameObject.GetComponent<CanvasGroup>().alpha = 0;
		//set the timescale back to 1 to run the game again
		Time.timeScale = 1;
		//call the scene transition
		gameObject.GetComponent<SceneTransitions>().startSceneTransition(buildIndex);
		
	}


}
