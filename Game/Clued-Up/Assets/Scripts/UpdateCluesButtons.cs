using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateCluesButtons : MonoBehaviour {
    private Inventory inventory;
	private Story story;
    // Use this for initialization
    void Start () {
		story = GameObject.Find ("Story").GetComponent<Story> ();
    }
   
    public void updateButtonText(GameObject ClueButtons)
    {
        inventory = GameObject.Find("Detective").GetComponent<Inventory>();
        Debug.Log("size of collected clues:" + inventory.collectedClueNames.Count);
        if (inventory.collectedClueNames.Count < 4)
        {
            for (int i = 0; i < inventory.collectedClueNames.Count; i++)
            {
				ClueButtons.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Text>().text = story.getClueInformation(inventory.collectedClueNames[i]).GetComponent<Clue>().longName;
            }
            
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
				ClueButtons.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Text>().text = story.getClueInformation(inventory.collectedClueNames[i]).GetComponent<Clue>().longName;
            }
            for (int i = 4; i < inventory.collectedClueNames.Count; i++)
            {
				ClueButtons.transform.GetChild(0).GetChild(1).GetChild(i-4).GetComponent<Text>().text = story.getClueInformation(inventory.collectedClueNames[i]).GetComponent<Clue>().longName;
            }
        }  
    }
}
