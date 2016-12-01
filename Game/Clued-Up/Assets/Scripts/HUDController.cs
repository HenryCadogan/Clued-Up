using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
	public GameObject HUDText;
	
	public void loadPanelAndPause(GameObject panelToLoad){
		panelToLoad.SetActive (true);
		Time.timeScale = 0; //makes game time pause
	}

	public void hidePanelAndResume(GameObject panelToHide){
		panelToHide.SetActive (false);
		Time.timeScale = 1; //makes game time resume
	}

	IEnumerator fadeHUDText(string text){
		//instantly fades out text in case it isn't already, changes HUDText, fades in, waits, fades out
		HUDText.GetComponent<Text> ().CrossFadeAlpha (0f, 0f, false);
		HUDText.GetComponent<Text> ().text = text;
		HUDText.GetComponent<Text> ().CrossFadeAlpha (1f, 3f, false);
		yield return new WaitForSeconds(3);
		HUDText.GetComponent<Text> ().CrossFadeAlpha (0f, 3f, false);
	}

	public void displayHUDText(string text){
		StartCoroutine(fadeHUDText(text));
	}



	public void testInventoryOnClickOfNotebookButton(){
		GameObject.Find ("Detective").GetComponent<Inventory> ().collectedClues.ForEach(Debug.Log);	//print list of all collected clues
	}
}
