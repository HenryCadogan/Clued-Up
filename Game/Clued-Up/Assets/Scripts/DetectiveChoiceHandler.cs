using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Detective choice handler.
/// </summary>
public class DetectiveChoiceHandler : MonoBehaviour {
	/// <summary>
	/// The character descriptions object.
	/// </summary>
	public GameObject characterDescriptionsObject;
	/// <summary>
	/// The story object.
	/// </summary>
	private Story story;
	/// <summary>
	///stores which description is currently visible
	/// </summary>
	private int currentlyActive;


	/// <summary>
	/// //Sets detective in story to the one the player is viewing the description of after they click the confirm button
	/// </summary>
	public void chooseDetective(){
		
		story.setDetective(currentlyActive);
		SceneManager.LoadScene (3); //loads room1/ lakeside
	}

	/// <summary>
	/// //Sets information in description panel and makes the panel visible
	/// </summary>
	/// <param name="detective">Detective.</param>
	public void displayInformation(int detective){
		
		//TODO replace image with image depending on detective GameObject.Find("LargerIcon").GetComponent<Renderer>().material = 
		//TODO replace name with name depending on detective GameObject.Find("Name").GetComponent<Text>().text = 
		//TODO replace description with text depending on detective GameObject.Find("Description").GetComponent<Text>().text = 

		characterDescriptionsObject.SetActive (true); //makes description panel visible
		currentlyActive = detective;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story

		//TODO READ CHARACTERS FROM FILE
	}

}
