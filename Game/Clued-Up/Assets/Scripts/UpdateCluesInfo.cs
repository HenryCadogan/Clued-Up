using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateCluesInfo : MonoBehaviour {
	private Inventory inventory;
	private Story story;

    public Text clueName;
    public Text clueDescription;
    public Image clueImage;
    //public GameObject buttonPressed;
    

	// Use this for initialization
	void Start () {
		story = GameObject.Find ("Story").GetComponent<Story> ();
        //inventory = GameObject.Find("Detective").GetComponent<Inventory>();
    }

    public void UpdateInformation(GameObject buttonPressed)
    {
        inventory = GameObject.Find("Detective").GetComponent<Inventory>();
        //Index is the index of the button on the panel.
        int index = buttonPressed.transform.GetSiblingIndex();
        int buttonsInColumn = 4;
        Debug.Log("index: " + index);
        int parent = buttonPressed.transform.parent.GetSiblingIndex();
        //The clue buttons are split into two panels on the clue button panels. The LHS (col1) and RHS (col2)
        //As the top buttons initially would have the same index, we add on the parent index (0 or 1) * (how many buttons in each column)
        index = index + (parent*buttonsInColumn);
        //this index is used to search in the inventory.
        if (0 < inventory.collectedClueNames.Count && inventory.collectedClueNames.Count-1 <= index)
        {   
            if (index < inventory.collectedClueNames.Count)
            {
				Clue activeClue = story.getClueInformation(inventory.collectedClueNames[index]).GetComponent<Clue>();
                clueName.text = activeClue.longName;
                clueDescription.text = activeClue.description;
                clueImage.sprite = activeClue.sprite;
            }
            else
            {
                clueName.text = "Undiscovered";
                clueDescription.text = "This clue is yet to be discovered";   
            }
        }   
    }
}
