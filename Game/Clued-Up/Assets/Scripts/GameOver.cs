using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour {
	public GameObject textPanel;
	public AudioSource audioSource;
	public SceneTransitions sceneController;
	private Story story;

	private void fadeText(GameObject textObject, float alpha, float time){ 
		textObject.GetComponent<Text>().CrossFadeAlpha(alpha,time,false);
	}

	IEnumerator endGameCutscene(Story story){
		audioSource.Play (); //play scream
		yield return new WaitForSeconds(1f); //wait 1 second after scream begins
		textPanel.transform.GetChild(0).GetComponent<Text>().text = "On this day, "+ story.murderer.GetComponent<Character>().longName +" was arrested for the murder of "+ story.victim.GetComponent<Character>().longName +" at the Ron Cooke Hub.";
		fadeText (textPanel.transform.GetChild (0).gameObject,1f, 2f);

		yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
		textPanel.transform.GetChild(1).GetComponent<Text>().text = "The murder was carried out using a " + story.murderWeapon +".";
		fadeText (textPanel.transform.GetChild (1).gameObject,1f, 2f);

		yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
		textPanel.transform.GetChild(2).GetComponent<Text>().text = "During the interrogation, it was revealed that the murder had something to do with a " + story.motiveClue+".";
		fadeText (textPanel.transform.GetChild (2).gameObject,1f, 2f);

		yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
		textPanel.transform.GetChild(3).GetComponent<Text>().text = "Your work being done, you headed home to your force.";
		fadeText (textPanel.transform.GetChild (3).gameObject,1f, 2f);

		yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
		textPanel.transform.GetChild(4).GetComponent<Text>().text = "It needs you.";
		fadeText (textPanel.transform.GetChild (4).gameObject,1f, 2f);

		yield return new WaitForSeconds(3f); // wait three secs for fade, and one second after the fade ends
		print("FADEOUT");
		fadeOutAllText (0f,2f);

		yield return new WaitForSeconds(3f); // wait three secs for fade, and one second after the fade ends

		SceneManager.LoadScene (12); //load credits
	}
	/// <summary>
	/// Simultaneously fades out all text boxes
	/// </summary>
	private void fadeOutAllText(float alpha, float time){
		fadeText (textPanel.transform.GetChild (0).gameObject, alpha, time);
		fadeText (textPanel.transform.GetChild (1).gameObject, alpha, time);
		fadeText (textPanel.transform.GetChild (2).gameObject, alpha, time);
		fadeText (textPanel.transform.GetChild (3).gameObject, alpha, time);
		fadeText (textPanel.transform.GetChild (4).gameObject, alpha, time);
		print ("DONE");
	}

	// Use this for initialization
	void Start () {
		Story story = FindObjectOfType<Story> ();
		fadeOutAllText (0f, 0f); //instantaneously fades out all text boxes
		StartCoroutine (endGameCutscene(story));
	}
}
