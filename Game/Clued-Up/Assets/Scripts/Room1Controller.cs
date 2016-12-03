﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Room1Controller : MonoBehaviour {
	/// <summary>
	/// The background pane.
	/// </summary>
	public GameObject backgroundPane;
	/// <summary>
	/// The floor object
	/// </summary>
	public GameObject floor;
	/// <summary>
	/// The overlay panel.
	/// </summary>
	public GameObject overlayPanel;
	/// <summary>
	/// The snow generator.
	/// </summary>
	public GameObject snowGenerator;
	/// <summary>
	/// The rain generator.
	/// </summary>
	public GameObject rainGenerator;
	/// <summary>
	/// The ron cooke lights.
	/// </summary>
	public GameObject ronCookeLights;
	/// <summary>
	/// The main light.
	/// </summary>
	public GameObject mainLight;
	/// <summary>
	/// The detectives.
	/// </summary>
	public GameObject detectives;
	/// <summary>
	/// The story.
	/// </summary>
	private Story story;

	/// <summary>
	/// Sets the lights of the scene
	/// </summary>
	private void setLights(){
		//sets lighting to weather appropriate colour
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
	/// Sets the background.
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
		if (weather == 1) {	//set generators depending on weather
			rainGenerator.SetActive (true);
		} else if (weather == 3) {
			snowGenerator.SetActive (true);
		}
	}

	/// <summary>
	/// Sets the overlay.
	/// </summary>
	private void setOverlay(){
		//turns on overlay panel, fades out
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image>().CrossFadeAlpha(0f,3f,false);
	}

	/// <summary>
	/// Sets the clues.
	/// </summary>
	private void setClues(){
		//instanciates new clue prefab with location & rotation, scales for perspective calls its initialisation method
		//todo makes sure they havent already been collected first
		GameObject bodyClue = Instantiate (Resources.Load ("Clue"), new Vector3(1.5f,-5.99f,-1.2f), Quaternion.Euler(90,0,0)) as GameObject;
		bodyClue.GetComponent<Transform> ().localScale = new Vector3 (1f, 4.5f, 1f);
		bodyClue.GetComponent<BoxCollider> ().size = new Vector3 (4.5f, 1.75f, 0f);	//manually set box collider as object is on floor & fixed position
		bodyClue.GetComponent<Clue>().initialise("chalkOutline", "Chalk Outline", "A chalk outline of the body of " + story.getVictim().longName +". " + this.getChalkOutlineDescription());
	}

	/// <summary>
	/// Gets the chalk outline description.
	/// </summary>
	/// <returns>The chalk outline description.</returns>
	private string getChalkOutlineDescription(){
		//TODO read random outline descriptions from file
		return "It looks like the body was removed by the police a while ago.";
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story
		detectives.transform.GetChild(story.getDetective()).gameObject.SetActive(true); //only activates the chosen detective by using the detective int as an index of children of the Detectives object

		setOverlay ();
		setBackground ();
		setLights ();
		setClues ();

		GameObject detective = GameObject.Find("Detective");
		detective.GetComponent<Detective> ().walkIn();
		//Make detective slightly lower in screen
		Vector3 pos = detective.transform.position;
		pos.y = -5.9f;
		detective.transform.position = pos;

	}
}
