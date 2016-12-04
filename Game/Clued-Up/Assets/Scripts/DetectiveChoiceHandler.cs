using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Controller for detective selection screen.
/// </summary>
public class DetectiveChoiceHandler : MonoBehaviour {
	/// <summary>
	/// The panel containing the character descriptions.
	/// </summary>
	public GameObject characterDescriptionsObject;
	/// <summary>
	/// The main story object.
	/// </summary>
	private Story story;
	/// <summary>
	/// Stores which description/ detective is currently visible
	/// </summary>
	private int currentlyActive;


	/// <summary>
	/// // Sets detective in the main story to the one the user confirmed after viewing their description.
	/// </summary>
	public void chooseDetective(){
		story.setDetective(currentlyActive);
		SceneManager.LoadScene (3); //loads room1/ lakeside
	}
	/// <summary>
	/// // Sets information in description panel and makes the panel visible
	/// </summary>
	/// <param name="detective">Detective index</param>
	public void displayInformation(int detective){
		
		//TODO replace image with image depending on detective GameObject.Find("LargerIcon").GetComponent<Renderer>().material = 
		//TODO replace name with name depending on detective GameObject.Find("Name").GetComponent<Text>().text = 
		//TODO replace description with text depending on detective GameObject.Find("Description").GetComponent<Text>().text = 

		characterDescriptionsObject.SetActive (true); //makes description panel visible
		currentlyActive = detective;
	}
	/// <summary>
	/// Start this instance and build list of detectives.
	/// </summary>
	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story

		//TODO READ CHARACTERS FROM FILE
	}

}
