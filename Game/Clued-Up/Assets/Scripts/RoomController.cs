using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/// <summary>
/// Room Controller
/// </summary>

public class RoomController : MonoBehaviour {
	/// <summary>
	/// The index of the room.
	/// </summary>
	public int roomIndex;
	/// <summary>
	/// The overlay panel GameObject.
	/// </summary>
	public GameObject overlayPanel;
	/// <summary>
	/// The door quad GameObject.
	/// </summary>
	public GameObject doorQuad;

	/// <summary>
	/// The main story.
	/// </summary>
	private Story story;

	/// <summary>
	/// Prepares the overlay by turning on and immediately fading out, giving a fade from black effect.
	/// </summary>
	private void setOverlay(){
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image> ().CrossFadeAlpha (0f, 3f, false);
	}
	/// <summary>
	/// Sets the door quad GameObjects image depending on weather condition of story.
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
	/// Gets the characters present in this room from story.
	/// </summary>
	private void getCharacters(){
		List<GameObject> charactersInRoom = new List<GameObject> ();
		Debug.Log (roomIndex);
		if (story.getCharactersInRoom (roomIndex).Count > 0) {
			charactersInRoom = story.getCharactersInRoom (roomIndex);
		} else {
			Debug.Log ("No characters this time");
		}

		switch (charactersInRoom.Count) {
		case 1:
			//do stuff to make one character active
			Debug.Log (charactersInRoom [0].GetComponent<Character>().longName + "is in the room!");
			break;
		case 2:
			//do stuff to make two characters active in the same room
			break;
		default:
			break;
		}			
	}

	/// <summary>
	/// Walk character in, get reference to story and do initialisation for scene
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

		switch (roomIndex) {
		case 1:
			setDoor ();
			break;
		default:
			break;
		}

		getCharacters ();
		//setClues ();
		//GameObject.Find("Detective").GetComponent<PlayerController> ().walkInFrom (true);
	}
}
