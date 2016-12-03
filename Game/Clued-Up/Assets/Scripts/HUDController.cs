using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Heads up display controller
/// </summary>
public class HUDController : MonoBehaviour {
	/// <summary>
	/// The HUD text.
	/// </summary>
	public GameObject HUDText;

	/// <summary>
	/// Loads the panel and pause.
	/// </summary>
	/// <param name="panelToLoad">Panel to load.</param>
	public void loadPanelAndPause(GameObject panelToLoad){
		panelToLoad.SetActive (true);
		Time.timeScale = 0; //makes game time pause
	}

	/// <summary>
	/// Hides the panel and resume.
	/// </summary>
	/// <param name="panelToHide">Panel to hide.</param>
	public void hidePanelAndResume(GameObject panelToHide){
		panelToHide.SetActive (false);
		Time.timeScale = 1; //makes game time resume
	}

	/// <summary>
	/// Fades the HUD text.
	/// </summary>
	/// <returns>The HUD text.</returns>
	/// <param name="text">Text.</param>
	IEnumerator fadeHUDText(string text){
		//instantly fades out text in case it isn't already, changes HUDText, fades in, waits, fades out
		HUDText.GetComponent<Text> ().CrossFadeAlpha (0f, 0f, false);
		HUDText.GetComponent<Text> ().text = text;
		HUDText.GetComponent<Text> ().CrossFadeAlpha (1f, 3f, false);
		yield return new WaitForSeconds(3);
		HUDText.GetComponent<Text> ().CrossFadeAlpha (0f, 3f, false);
	}

	/// <summary>
	/// Displays the HUD text.
	/// </summary>
	/// <param name="text">Text.</param>
	public void displayHUDText(string text){
		StartCoroutine(fadeHUDText(text));
	}


	/// <summary>
	/// Tests the inventory on click of notebook button.
	/// </summary>
	public void testInventoryOnClickOfNotebookButton(){
		GameObject.Find ("Detective").GetComponent<Inventory> ().collectedClues.ForEach(Debug.Log);	//print list of all collected clues
	}
}
