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
		Debug.Log ("UPDATE BUTTON TEXT:");
        inventory = GameObject.Find("Detective").GetComponent<Inventory>();
        Debug.Log("size of collected clues:" + inventory.collectedClueNames.Count);
        for (int i = 0; i < inventory.collectedClueNames.Count; i++){
			if (i < 4) {
				Debug.Log ("LESS THAN 4, i = " + i);
				ClueButtons.transform.GetChild (0).GetChild (i).GetChild (0).GetComponent<Text> ().text = story.getClueInformation (inventory.collectedClueNames [i]).GetComponent<Clue> ().longName;
			}else{
				ClueButtons.transform.GetChild(0).GetChild(i-4).GetChild(0).GetComponent<Text>().text = story.getClueInformation(inventory.collectedClueNames[i]).GetComponent<Clue>().longName;
            }
        }  
    }
}
