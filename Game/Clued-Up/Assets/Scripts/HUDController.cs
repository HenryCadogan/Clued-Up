using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// Heads up display controller
/// </summary>
public class HUDController : MonoBehaviour {
	/// <summary>
	/// Text GameObject from within the HUD prefab
	/// </summary>
	public GameObject HUDText;

	/// <summary>
	/// Makes a panel active and pauses game time.
	/// </summary>
	/// <param name="panelToLoad">Panel GameObject that will become visible</param>
	public void loadPanelAndPause(GameObject panelToLoad){
		panelToLoad.SetActive (true);
		Time.timeScale = 0;
	}
	/// <summary>
	/// Hides a panel and resumes game time.
	/// </summary>
	/// <param name="panelToHide">Panel GameObject that will be hidden.</param>
	public void hidePanelAndResume(GameObject panelToHide){
		panelToHide.SetActive (false);
		Time.timeScale = 1;
	}

	/// <summary>
	/// Fades new message in and out of HUD
	/// </summary>
	/// <returns>Yield WaitForSeconds</returns>
	/// <param name="text">Text string to be displayed in HUD</param>
	IEnumerator fadeHUDText(string text){
		//instantly fades out text in case it isn't already, changes HUDText, fades in, waits, fades out
		HUDText.GetComponent<Text> ().CrossFadeAlpha (0f, 0f, false);
		HUDText.GetComponent<Text> ().text = text;
		HUDText.GetComponent<Text> ().CrossFadeAlpha (1f, 1.5f, false);
		yield return new WaitForSeconds(3);
		HUDText.GetComponent<Text> ().CrossFadeAlpha (0f, 1.5f, false);
	}
	/// <summary>
	/// Calls coroutine to fade message in/out of HUD
	/// </summary>
	/// <param name="text">Text string to be displayed in the HUD</param>
	public void displayHUDText(string text){
		StartCoroutine(fadeHUDText(text));
	}
}
