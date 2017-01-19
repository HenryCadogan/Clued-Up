using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Story GameStory = FindObjectOfType<Story> ();
		Character Murderer = GameStory.murderer.GetComponent<Character> ();
		Character Victim = GameStory.victim.GetComponent<Character> ();
		Text TextObject = GetComponent<Text> ();
		string GameOverText = "On this day, " + Murderer.longName + " was arrested for the murder of "
		                      + Victim.longName + " at the Ron Cooke Hub.\r\nThe murder was carried out using a " + GameStory.MurderWeapon
		                      + ".\r\nDuring the interrogation, it was revealed that the murder had something to do with a "
		                      + ".\r\nYour work being done, you headed home to your planet.\r\nIt needs you.";
		TextObject.text = GameOverText;
	}

	public void RTMM(){
		SceneManager.LoadScene (0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
