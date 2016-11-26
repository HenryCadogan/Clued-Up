using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {
	
	public void loadPanelAndPause(GameObject panelToLoad){
		panelToLoad.SetActive (true);
		Time.timeScale = 0; //makes game time pause
	}

	public void hidePanelAndResume(GameObject panelToHide){
		panelToHide.SetActive (false);
		Time.timeScale = 1; //makes game time resume
	}
}
