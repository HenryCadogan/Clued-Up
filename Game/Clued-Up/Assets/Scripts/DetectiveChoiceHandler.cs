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
	public GameObject detectiveButtons;
	public GameObject detectives;
	/// <summary>
	/// The main story object.
	/// </summary>
	private Story story;


	/// <summary>
	/// Stores which description/ detective is currently visible
	/// </summary>
	public int currentlyActive;


	/// <summary>
	/// // Sets detective in the main story to the one the user confirmed after viewing their description.
	/// </summary>
	public void chooseDetective(){
		Debug.Log (currentlyActive);
		story.setDetective(currentlyActive);
		detectives.transform.GetChild(story.getDetective()).gameObject.SetActive(true); //only activates the chosen detective by using the detective int as an index of children of the Detectives object
		SceneManager.LoadScene (3); //loads room1/ lakeside
	}
	/// <summary>
	/// // Sets information in description panel and makes the panel visible
	/// </summary>
	/// <param name="detective">Detective index</param>
	public void displayInformation(int selectedDetectiveIndex){
		Detective selectedDetective = detectives.transform.GetChild (selectedDetectiveIndex).GetComponent<Detective> ();
		characterDescriptionsObject.transform.FindChild ("DescriptionsName").GetComponent<Text> ().text = selectedDetective.longName;
		characterDescriptionsObject.transform.FindChild ("Description").GetComponent<Text> ().text = selectedDetective.description;
		characterDescriptionsObject.transform.FindChild ("LargerIcon").GetComponent<Image>().sprite = Resources.Load<Sprite> ("detectiveIconSmall" + selectedDetectiveIndex.ToString());
		characterDescriptionsObject.SetActive (true); //makes description panel visible
		currentlyActive = selectedDetectiveIndex;
	}
	/// <summary>
	/// Start this instance and build list of detectives.
	/// </summary>
	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story
		int currentlySettingDetectiveButton = 0 ;
		foreach (Transform button in detectiveButtons.transform) {
			button.transform.FindChild ("Name").GetComponent<Text> ().text = detectives.transform.GetChild(currentlySettingDetectiveButton).GetComponent<Detective>().longName;
			button.transform.FindChild ("Trait1").GetComponent<Text> ().text = "• " + story.getTraitString(detectives.transform.GetChild(currentlySettingDetectiveButton).GetComponent<Detective>().traits[0]);
			button.transform.FindChild ("Trait2").GetComponent<Text> ().text = "• " + story.getTraitString(detectives.transform.GetChild(currentlySettingDetectiveButton).GetComponent<Detective>().traits[1]);
			button.transform.FindChild ("Trait3").GetComponent<Text> ().text = "• " + story.getTraitString(detectives.transform.GetChild(currentlySettingDetectiveButton).GetComponent<Detective>().traits[2]);
			button.transform.FindChild ("Icon").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("detectiveIconSmall" + currentlySettingDetectiveButton.ToString ());
			currentlySettingDetectiveButton ++;
		}
		//TODO READ CHARACTERS FROM FILE
	}

}
