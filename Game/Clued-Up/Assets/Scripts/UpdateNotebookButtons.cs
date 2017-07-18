﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateNotebookButtons : MonoBehaviour {
    private Inventory inventory;
	private Story story;
    // Use this for initialization
    void Start () {
		story = GameObject.Find ("Story").GetComponent<Story> ();
    }
   
    public void updateClueButtonText(GameObject ClueButtons){
		print ("BEGIN: updateClueButtonText");
        inventory = GameObject.Find("Detective").GetComponent<Inventory>();

        for (int i=0; i < inventory.collectedClueNames.Count; i++)
        {
            string clueShortName = inventory.collectedClueNames[i];
            print("INVENTORY: " + clueShortName);

            if (i < 4)
            {
                ClueButtons.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Text>().text = Clue.getClueLongName(clueShortName);
                ClueButtons.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                ClueButtons.transform.GetChild(1).GetChild(i - 4).GetChild(0).GetComponent<Text>().text = Clue.getClueLongName(clueShortName);
                ClueButtons.transform.GetChild(1).GetChild(i - 4).GetComponent<Button>().interactable = true;
            }
        }
    }

	public void updateCharacterButtonText(GameObject CharacterButtons){
		inventory = GameObject.Find("Detective").GetComponent<Inventory>();
		if (inventory.encounteredCharacterNames.Count > 0) {
			for (int i = 0; i < inventory.encounteredCharacterNames.Count; i++) {
				if (i < 4) {
					Debug.Log (inventory.encounteredCharacterNames [i]);
					CharacterButtons.transform.GetChild (0).GetChild (i).GetChild (0).GetComponent<Text> ().text = story.getCharacterInformation (inventory.encounteredCharacterNames [i]).longName;
					CharacterButtons.transform.GetChild (0).GetChild (i).GetComponent<Button> ().interactable = true;

				} else {
					CharacterButtons.transform.GetChild (1).GetChild (i - 4).GetChild (0).GetComponent<Text> ().text = story.getCharacterInformation (inventory.encounteredCharacterNames [i]).longName;
					CharacterButtons.transform.GetChild (1).GetChild (i - 4).GetComponent<Button> ().interactable = true;
				}
			} 
		}
	}
}
