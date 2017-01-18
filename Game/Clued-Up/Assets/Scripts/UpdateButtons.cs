using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateButtons : MonoBehaviour {
    private Inventory inventory;
	private Story story;
    // Use this for initialization
    void Start () {
		story = GameObject.Find ("Story").GetComponent<Story> ();
    }
   
    public void updateClueButtonText(GameObject ClueButtons)
    {
        inventory = GameObject.Find("Detective").GetComponent<Inventory>();
		Debug.Log (inventory.encounteredCharacterNames [0]);
        for (int i = 0; i < inventory.collectedClueNames.Count; i++){
			if (i < 4) {
				ClueButtons.transform.GetChild (0).GetChild (i).GetChild (0).GetComponent<Text> ().text = story.getClueInformation (inventory.collectedClueNames [i]).GetComponent<Clue> ().longName;
				ClueButtons.transform.GetChild (0).GetChild (i).GetComponent<Button> ().interactable = true;

			}else{
				ClueButtons.transform.GetChild(1).GetChild(i-4).GetChild(0).GetComponent<Text>().text = story.getClueInformation(inventory.collectedClueNames[i]).GetComponent<Clue>().longName;
				ClueButtons.transform.GetChild (1).GetChild (i-4).GetComponent<Button> ().interactable = true;
			}
        }  
    }

	public void updateCharacterButtonText(GameObject ClueButtons)
	{
		inventory = GameObject.Find("Detective").GetComponent<Inventory>();
		for (int i = 0; i < story.encounteredCharacterNames.Count; i++){
			if (i < 4) {
				ClueButtons.transform.GetChild (0).GetChild (i).GetChild (0).GetComponent<Text> ().text = story.getCharacterInformation (inventory.encounteredCharacterNames [i]).longName;
				ClueButtons.transform.GetChild (0).GetChild (i).GetComponent<Button> ().interactable = true;

			}else{
				ClueButtons.transform.GetChild (0).GetChild (i).GetChild (0).GetComponent<Text> ().text = story.getCharacterInformation (inventory.encounteredCharacterNames [i]).longName;
				ClueButtons.transform.GetChild (1).GetChild (i-4).GetComponent<Button> ().interactable = true;
			}
		}  
	}
}
