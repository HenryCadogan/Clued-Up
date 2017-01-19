using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateCluesInfo : MonoBehaviour {
	private Inventory inventory;
	private Story story;

    public Text nameText;
    public Text descriptionText;
    public Image notebookImage;
    //public GameObject buttonPressed;
    

	// Use this for initialization
	void Start () {
		story = GameObject.Find ("Story").GetComponent<Story> ();
        //inventory = GameObject.Find("Detective").GetComponent<Inventory>();
    }

    public void UpdateClueInformation(GameObject buttonPressed)
    {
		inventory = GameObject.Find ("Detective").GetComponent<Inventory> ();
		int index = buttonPressed.transform.GetSiblingIndex ();
		int buttonsInColumn = 4;
		int parent = buttonPressed.transform.parent.GetSiblingIndex ();
		//The clue buttons are split into two panels on the clue button panels. The LHS (col1) and RHS (col2)
		//As the top buttons initially would have the same index, we add on the parent index (0 or 1) * (how many buttons in each column)
		index = index + (parent * buttonsInColumn);
		Clue activeClue = story.getClueInformation (inventory.collectedClueNames [index]).GetComponent<Clue> ();
		nameText.text = activeClue.longName;
		descriptionText.text = activeClue.description;
		notebookImage.sprite = activeClue.sprite;
	}

	public void UpdateCharacterInformation(GameObject buttonPressed)
	{
		inventory = GameObject.Find ("Detective").GetComponent<Inventory> ();
		int index = buttonPressed.transform.GetSiblingIndex ();
		int buttonsInColumn = 4;
		int parent = buttonPressed.transform.parent.GetSiblingIndex ();
		//The clue buttons are split into two panels on the clue button panels. The LHS (col1) and RHS (col2)
		//As the top buttons initially would have the same index, we add on the parent index (0 or 1) * (how many buttons in each column)
		index = index + (parent * buttonsInColumn);
		Character activeChar = story.getCharacterInformation (inventory.encounteredCharacterNames [index]);
		nameText.text = activeChar.longName;
		descriptionText.text = activeChar.description;
		notebookImage.sprite = activeChar.image; 
	}
}
