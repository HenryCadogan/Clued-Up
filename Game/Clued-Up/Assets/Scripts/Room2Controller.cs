using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Room 2 Controller
/// </summary>

public class Room2Controller : MonoBehaviour {
	/// <summary>
	/// The overlay panel.
	/// </summary>
	public GameObject overlayPanel;
	/// <summary>
	/// The door quad.
	/// </summary>
	public GameObject doorQuad;
	/// <summary>
	/// The story.
	/// </summary>
	private Story story;

	/// <summary>
	/// Sets the overlay.
	/// </summary>
	private void setOverlay(){
		//turns on overlay panel, fades it out
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image>().CrossFadeAlpha(0f,3f,false);
	}

	/// <summary>
	/// Sets the clues.
	/// </summary>
	private void setClues(){
		//instanciates new clue prefab with location & rotation, scales for perspective calls its initialisation method
		GameObject bodyClue = Instantiate (Resources.Load ("Clue"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		bodyClue.GetComponent<Transform> ().localScale = new Vector3 (1f, 4.5f, 1f);
		bodyClue.GetComponent<BoxCollider> ().size = new Vector3 (4.5f, 1.75f, 0f);	//manually set box collider as object is on floor & fixed position
		bodyClue.GetComponent<Clue>().initialise("chalkOutline", "Chalk Outline", "A chalk outline of the deceased.");
	}
	/// <summary>
	/// Sets the door.
	/// </summary>
	private void setDoor(){
		Material[] materialArray = new Material[8];
		//maretialArray for DoorQuad materials
		materialArray [0] = (Material)Resources.Load ("Room2DSunny", typeof(Material)); //finds material located in the resources folder
		materialArray [1] = (Material)Resources.Load ("Room2DRain", typeof(Material));
		materialArray [2] = (Material)Resources.Load ("Room2DSunset", typeof(Material));
		materialArray [3] = (Material)Resources.Load ("Room2DSnow", typeof(Material));

		doorQuad.GetComponent<Renderer> ().material = materialArray [story.getWeather ()];
	}


	/// <summary>
	/// Gets the characters.
	/// </summary>
	private void getCharacters(){
		List<GameObject> charactersInRoom = story.getCharactersInRoom (2);
		switch (charactersInRoom.Count) {
		case 1:
			//do stuff to make one character active
			Debug.Log (charactersInRoom [0].GetComponent<Character>().longName + "is in the room!");
			break;
		case 2:
			//do stuff to make two characters active in the same room
			break;
		default:
			Debug.Log ("No characters this time");
			break;
		}			
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		setOverlay ();
		GameObject detective = GameObject.Find ("Detective");
		detective.GetComponent<Detective> ().walkIn();


		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story

		//Make detective slightly lower in screen
		Vector3 pos = detective.transform.position;
		pos.y = -6.5f;
		detective.transform.position = pos;

		setDoor ();
		getCharacters ();
		//setClues ();
		//GameObject.Find("Detective").GetComponent<PlayerController> ().walkInFrom (true);
	}
}
