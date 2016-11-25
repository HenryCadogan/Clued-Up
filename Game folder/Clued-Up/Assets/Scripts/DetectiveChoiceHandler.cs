using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DetectiveChoiceHandler : MonoBehaviour {
	public GameObject characterDescriptionsObject;

	private Story story;
	private int currentlyActive; //stores which description is currently visible

	public void chooseDetective(){
		//Sets detective in story to the one the player is viewing the description of after they click the confirm button
		story.setDetective(currentlyActive);
		SceneManager.LoadScene (3); //loads room1/ lakeside
	}

	public void displayInformation(int detective){
		//Sets information in description panel and makes the panel visible
		//TODO replace image with image depending on detective GameObject.Find("LargerIcon").GetComponent<Renderer>().material = 
		//TODO replace name with name depending on detective GameObject.Find("Name").GetComponent<Text>().text = 
		//TODO replace description with text depending on detective GameObject.Find("Description").GetComponent<Text>().text = 

		characterDescriptionsObject.SetActive (true); //makes description panel visible
		currentlyActive = detective;
	}

	// Use this for initialization
	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story

		//TODO READ CHARACTERS FROM FILE
	}

}
