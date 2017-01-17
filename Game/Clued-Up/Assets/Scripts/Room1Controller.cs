using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Room1Controller : MonoBehaviour {
	/// <summary>
	/// The background image pane GameObject.
	/// </summary>
	public GameObject backgroundPane;
	/// <summary>
	/// The floor image GameObject
	/// </summary>
	public GameObject floor;
	/// <summary>
	/// The overlay panel GameObject used for fading.
	/// </summary>
	public GameObject overlayPanel;
	/// <summary>
	/// The snow generator prefab GameObject.
	/// </summary>
	public GameObject snowGenerator;
	/// <summary>
	/// The rain generator prefab GameObject.
	/// </summary>
	public GameObject rainGenerator;
	/// <summary>
	/// The Ron Cooke Hub Lights group GameObject.
	/// </summary>
	public GameObject ronCookeLights;
	/// <summary>
	/// The main light GameObject.
	/// </summary>
	public GameObject mainLight;
	/// <summary>
	/// The detectives GameObject that contains all Dectectives as children.
	/// </summary>
	public GameObject detectives;
	/// <summary>
	/// The GUI HUD.
	/// </summary>
	public GameObject hud;
	/// <summary>
	/// The main story.
	/// </summary>
	private Story story;
	/// <summary>
	/// The inventory.
	/// </summary>
	private Inventory inventory;


	/// <summary>
	/// Sets the lights of the scene depending on weather condition
	/// </summary>
	private void setLights(){
		int weather = story.getWeather ();
		switch (weather) {
		case 0:
			mainLight.GetComponent<Light> ().color = new Color(0.65f,0.65f,0.65f); //sunny
			break;
		case 1:
			mainLight.SetActive (false); //rain
			ronCookeLights.SetActive(true);
			break;
		case 2:
			mainLight.GetComponent<Light> ().color = new Color (0.65f, 0.5f, 0.4f); //sunset
			break;
		case 3:
			mainLight.GetComponent<Light> ().color = Color.white; //snow
			mainLight.GetComponent<Light>().shadowStrength = 0.05f;
			break;
		}
	}

	/// <summary>
	/// Sets the background pane & floor GameObject images depending on weather condition. Also sets rain or snow.
	/// </summary>
	private void setBackground(){
		Material[] materialArray = new Material[8];
		materialArray[0] = (Material)Resources.Load("Room1Sunny", typeof(Material)); //finds material located in the resources folder
		materialArray[1] = (Material)Resources.Load("Room1Rain", typeof(Material));
		materialArray[2] = (Material)Resources.Load("Room1Sunset", typeof(Material));
		materialArray[3] = (Material)Resources.Load("Room1Snow", typeof(Material));
		//floor materials start
		materialArray[4] = (Material)Resources.Load("Room1FSunny", typeof(Material));
		materialArray[5] = (Material)Resources.Load("Room1FRain", typeof(Material));
		materialArray[6] = (Material)Resources.Load("Room1FSunset", typeof(Material));
		materialArray[7] = (Material)Resources.Load("Room1FSnow", typeof(Material));

		int weather = story.getWeather ();
		backgroundPane.GetComponent<Renderer> ().material = materialArray [weather];
		floor.GetComponent<Renderer> ().material = materialArray [weather + 4];
		if (weather == 1) {
			rainGenerator.SetActive (true);
		} else if (weather == 3) {
			snowGenerator.SetActive (true);
		}
	}

	/// <summary>
	/// Prepares the overlay by turning on and immediately fading out, giving a fade from black effect.
	/// </summary>
	private void setOverlay(){
		//turns on overlay panel, instantly turns black & fades out
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image>().CrossFadeAlpha(1f,0f,false);
		overlayPanel.GetComponent<Image>().CrossFadeAlpha(0f,3f,false);
	}
	/// <summary>
	/// Gets the clues for this room from story, and positions them within the room.
	/// </summary>
	private void getClues(){
		Debug.Log ("HERE");
		GameObject chalkOutline = story.getCluesInRoom(0) [0];
		chalkOutline.GetComponent<Transform> ().localScale = new Vector3 (1f, 4.5f, 1f); //sizes clue correctly
		chalkOutline.GetComponent<Transform>().position = new Vector3(1.5f,-5.99f,-1.2f); 
		chalkOutline.GetComponent<Transform> ().Rotate (new Vector3 (90f, 0f, 0f));
		chalkOutline.GetComponent<BoxCollider> ().size = new Vector3 (4.5f, 1.75f, 0f);	//manually set box collider as this clue is on floor, so normal collider doesnt work
	}
		
	/// <summary>
	/// Can the player progress from this scene to another? 
	/// </summary>
	/// <returns><c>true</c>, if criteria is met (namely they have collected the body clue), <c>false</c> otherwise.</returns>
	public bool canProgress(){
		if (inventory.isCollected("chalkOutline")){
			return true;
		}else{
			hud.GetComponent<HUDController> ().displayHUDText ("Try inspecting the scene of the murder");
			return false;
		}
	}
	/// <summary>
	/// Get reference of story, run initialisation sequence & make detective walk in
	/// </summary>
	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story
		inventory = GameObject.Find("Detective").GetComponent<Inventory>();

		setOverlay ();
		setBackground ();
		setLights ();
		getClues ();

		GameObject detective = GameObject.Find("Detective");
		detective.GetComponent<Detective> ().walkIn();

		Vector3 pos = detective.transform.position; //Make detective slightly lower in screen
		pos.y = -5.9f;
		detective.transform.position = pos;
	}

	/// <summary>
	/// If number is pressed during this scene, then skip to appropriate room
	/// </summary>
	void Update() {
		if (Input.GetKeyDown ("2")){
			SceneManager.LoadScene (4);	//train station
		}else if (Input.GetKeyDown ("4")){
			SceneManager.LoadScene (7);	//loads kitchen
		}else if (Input.GetKeyDown ("5")){
			SceneManager.LoadScene (8);	//load bar
		}else if (Input.GetKeyDown ("6")){
			SceneManager.LoadScene (8);	//loads studio
		}else if (Input.GetKeyDown ("7")){
			SceneManager.LoadScene (9);	//loads toilets
	}

	}
}
