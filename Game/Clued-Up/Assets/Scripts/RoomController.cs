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
	/// List of furniture gameobjects in the room
	/// </summary>
	private List<GameObject> furnitureInRoom = new List<GameObject>();
	/// <summary>
	/// List of Clue GameObjects in the room used to assign locations to them.
	/// </summary>
	private List<GameObject> cluesInRoom = new List<GameObject>();
	/// <summary>
	/// The main story.
	/// </summary>
	private Story story;
	/// <summary>
	/// Size of detective in each room so it can be adjusted for realistic scaling
	/// </summary>
	private float[] detectiveSizeByRoom = {1f,1f,1.2f,1f,1.2f,1.2f,1.2f,1f};
	/// <summary>
	/// Character pos for each room (characterPositionByRoom[roomIndex]).
	/// </summary>
	private Vector3[] characterPositionsByRoom = {new Vector3 (), 
		new Vector3 (-3.3f, -7.2f, 4.2f), //lobby
		new Vector3 (-2.2f, -6.5f, 4.2f), //train
		new Vector3 (0, -6.7f, 4.2f), //cafe
		new Vector3 (3.55f, -7.2f, 4.2f), //kitchen
		new Vector3 (-2f, -6.5f, 4.2f), //bar
		new Vector3 (-0.8f, -7.3f, 4.2f), //studio
		new Vector3 (5.2f, -7.3f, 4.2f) //toilets
	};

	/// <summary>
	/// Scales the detective.
	/// </summary>
	/// <param name="detective">Detective GameObject.</param>
	/// <param name="scaler">Scaler (can be negative).</param>
	private void scaleDetective(GameObject detective, float scaler){
		detective.transform.localScale = new Vector3 (scaler, scaler, scaler);
	}

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
	private void setLobbyMusic(){
		string filepath = "Sounds/Ambience-Lobby" + UnityEngine.Random.Range (0, 2).ToString ();
		GameObject.Find ("Audio Source").GetComponent<AudioSource> ().clip = Resources.Load<AudioClip> (filepath);
		GameObject.Find ("Audio Source").GetComponent<AudioSource> ().Play ();
	}
	/// <summary>
	/// Gets the characters present in this room from story.
	/// </summary>
	private void getCharacters(){
		List<GameObject> charactersInRoom = new List<GameObject> ();
		if (story.getCharactersInRoom (roomIndex).Count > 0) {
			charactersInRoom = story.getCharactersInRoom (roomIndex);
		} else {
			Debug.Log ("No characters this time");
		}

		switch (charactersInRoom.Count) {
		case 1:
			//do stuff to make one character active
			//Debug.Log (charactersInRoom [0].GetComponent<Character> ().longName + " is in the room!");
			charactersInRoom [0].GetComponent<Character> ().display (characterPositionsByRoom[roomIndex]);
			break;
		case 2:
			//do stuff to make two characters active in the same room
			break;
		default:
			break;
		}			
	}
	/// <summary>
	/// Fengs the shui.
	/// Instanciates the furniture in each room, setting its position & initialising.
	/// </summary>
	private void fengShui(){
		switch (roomIndex) {
		case 1: //lobby
			GameObject lockers = Instantiate (Resources.Load ("Lockers"), new Vector3 (3.22f, -4f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			lockers.GetComponent<Lockers> ().Initialise ();
			furnitureInRoom.Add (lockers);
			break;
		case 2: //train station
			GameObject bin = Instantiate (Resources.Load ("Bin"), new Vector3 (7f, -3.7f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			bin.transform.localScale = new Vector3 (0.7f, 0.7f, 1f);
			bin.GetComponent<Bin> ().Initialise ();
			furnitureInRoom.Add (bin);
			break;
		case 4: //kitchen
			GameObject cupboardL = Instantiate (Resources.Load ("Cupboard"), new Vector3 (6.11f, -4.88f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			cupboardL.transform.localScale = new Vector3 (0.95f, 1f, 1f);
			cupboardL.GetComponent<Cupboard> ().Initialise ("cupboard-left","cupboard");
			furnitureInRoom.Add (cupboardL);

			GameObject cupboardR = Instantiate (Resources.Load ("Cupboard"), new Vector3 (7.42f, -4.88f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			cupboardR.transform.localScale = new Vector3 (0.9f, 1f, 1f);
			cupboardR.GetComponent<Cupboard> ().Initialise ("cupboard-right","cupboard");
			furnitureInRoom.Add (cupboardR);

			GameObject oven = Instantiate (Resources.Load ("Cupboard"), new Vector3 (-2.83f, -4.84f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			oven.transform.localScale = new Vector3 (0.95f, 1f, 1f);
			oven.GetComponent<Cupboard> ().Initialise ("oven-door","oven");
			furnitureInRoom.Add (oven);
			break;
		case 6: //studio
			GameObject sofa = Instantiate (Resources.Load ("Sofa"), new Vector3 (-5.9f, -4.8f, 0f), Quaternion.Euler (0, 0, 0)) as GameObject;
			sofa.GetComponent<Sofa> ().Initialise ();
			furnitureInRoom.Add (sofa);
			break;
		case 7: //toilets
			GameObject cubicleDoorLeft = Instantiate (Resources.Load ("Cupboard"), new Vector3 (-5.42f, -3.27f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			cubicleDoorLeft.transform.localScale = new Vector3 (0.95f, 0.95f, 1f);
			cubicleDoorLeft.GetComponent<Cupboard> ().Initialise ("cubicle-door", "cubicle", 0.14f, -0.3f);
			furnitureInRoom.Add (cubicleDoorLeft);

			GameObject cubicleDoorCentre = Instantiate (Resources.Load ("Cupboard"), new Vector3 (-2.93f, -3.27f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			cubicleDoorCentre.transform.localScale = new Vector3 (0.95f, 0.95f, 1f);
			cubicleDoorCentre.GetComponent<Cupboard> ().Initialise ("cubicle-door", "cubicle", 0.14f, -0.3f);
			furnitureInRoom.Add (cubicleDoorCentre);

			GameObject cubicleDoorRight = Instantiate (Resources.Load ("Cupboard"), new Vector3 (-0.52f, -3.27f, 2f), Quaternion.Euler (0, 0, 0)) as GameObject;
			cubicleDoorRight.transform.localScale = new Vector3 (0.95f, 0.95f, 1f);
			cubicleDoorRight.GetComponent<Cupboard> ().Initialise ("cubicle-door", "cubicle", 0.14f, -0.3f);
			furnitureInRoom.Add (cubicleDoorRight);
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// Finds the clue in the room.
	/// If clue isnt collected already, add it to cluesInRoom (used to assign clues in the room their locations).
	/// If it is already collected the clue is destroyed.
	/// </summary>
	private void getClues(){
		GameObject clueInRoom = story.getCluesInRoom (roomIndex) [0]; //instanciate clue so its name can be used

		if (!GameObject.Find ("Detective").GetComponent<Inventory> ().isCollected (clueInRoom.name)) { //If clue not already collected, add it to cluesInRoom
			this.cluesInRoom.Add (clueInRoom);
			Debug.Log ("Clue in room: " + this.cluesInRoom [0].GetComponent<Clue> ().longName);
		} else {
			Destroy (clueInRoom);
		}
	}

	/// <summary>
	/// Gets & sets the potential positions of clues in the room.
	/// </summary>
	/// <returns>The room clue locations.</returns>
	private List<Vector3> getRoomClueLocations(){
		List<Vector3> locationList = new List<Vector3> ();
		switch (roomIndex) {
		case 2: //train station
			locationList.Add (new Vector3 (-3.6f, -3.8f, 1f)); //on shelf
			locationList.Add (new Vector3 (4f, -4f, 1f)); //on bench
			break;
		case 3: //cafe
			locationList.Add (new Vector3 (-4.6f, -3f, 1f)); //on shelf
			break;
		case 4: //kitchen
			locationList.Add (new Vector3 (-5.55f, -3.89f, 1f)); //on shelf
			break;
		case 5: //bar
			locationList.Add (new Vector3 (7.43f, -2.1f, 1f));  // on bar
			locationList.Add (new Vector3 (5.08f, -2.1f, 1f));  // on bar
			locationList.Add (new Vector3 (4.36f, -3f, 1f));  // on stool
			locationList.Add (new Vector3 (1.8f, -3f, 1f));  // on stool
			break;
		case 6: //studio
			locationList.Add (new Vector3 (2f, -1.77f, 1f));  // on piano
			break;
		default:
			break;
		}

		return locationList;
	}

	/// <summary>
	/// First decides which location to put the clue by summing all possible locations and deciding which to use (either room location or furniture)
	/// Then assigns one clue to this location in a room by either changing clue's location, or testing which kind of furniture it is & moving it there.
	/// </summary>
	private void assignCluesLocations(GameObject clue){
		List<Vector3> roomLocations = getRoomClueLocations (); //possible locations that are not furniture
		int totalLocationsCount = roomLocations.Count + furnitureInRoom.Count;

		if (totalLocationsCount != 0) {
			int locationIndex = UnityEngine.Random.Range (0, totalLocationsCount);
			if (locationIndex < roomLocations.Count) {	//If clue is in room locations
				clue.gameObject.GetComponent<Transform> ().position = roomLocations [locationIndex];
			} else {
				locationIndex = locationIndex - roomLocations.Count; //change to only include furniture clues
				if (furnitureInRoom [locationIndex].GetComponent<Lockers> () != null) {
					furnitureInRoom [locationIndex].GetComponent<Lockers> ().addClue (clue);
				} else if (furnitureInRoom [locationIndex].GetComponent<Cupboard> () != null) {
					furnitureInRoom [locationIndex].GetComponent<Cupboard> ().addClue (clue);
				} else if (furnitureInRoom [locationIndex].GetComponent<Sofa> () != null) {
					furnitureInRoom [locationIndex].GetComponent<Sofa> ().addClue (clue);
				}else if (furnitureInRoom [locationIndex].GetComponent<Bin> () != null) {
						furnitureInRoom [locationIndex].GetComponent<Bin> ().addClue (clue);
				}
			}
		} else
			throw new System.NotSupportedException ("Room " + roomIndex + " does not support clues.");
		}

	/// <summary>
	/// Walk character in, get reference to story and do initialisation for scene
	/// </summary>
	void Start () {
		setOverlay ();
		GameObject detective = GameObject.Find ("Detective");
		detective.GetComponent<Detective> ().walkIn();
		scaleDetective (detective, detectiveSizeByRoom[roomIndex]);

		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story
		story.addVisitedRoom(roomIndex); //used to update map room availability

		//Make detective slightly lower in screen
		Vector3 pos = detective.transform.position;
		pos.y = -6.5f;
		detective.transform.position = pos;

		switch (roomIndex) {
		case 1: //lobby
			setDoor ();
			setLobbyMusic ();
			break;
		default:
			break;
		}

		getCharacters ();
		fengShui ();  
		getClues ();
		if (cluesInRoom.Count > 0) {
			assignCluesLocations (cluesInRoom [0]);
		} else {
			print ("No clues in this room");
		}
	}
}
