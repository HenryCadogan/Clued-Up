using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Room1Controller : MonoBehaviour {
	public GameObject backgroundPane;
	public GameObject floor;
	public GameObject overlayPanel;
	public GameObject snowGenerator;
	public GameObject rainGenerator;
	public GameObject ronCookeLights;
	public GameObject mainLight;

	private Story story;
	// Use this for initialization

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

	private void setBackground(Material[] mats){
		int weather = story.getWeather ();
		backgroundPane.GetComponent<Renderer> ().material = mats [weather];
		floor.GetComponent<Renderer> ().material = mats [weather + 4];
		if (weather == 1) {	//set generators depending on weather
			rainGenerator.SetActive (true);
		} else if (weather == 3) {
			snowGenerator.SetActive (true);
		}
	}

	private void setOverlay(){
		//turns on overlay panel, fades out
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image>().CrossFadeAlpha(0f,3f,false);
	}

	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story

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

		setOverlay ();
		setBackground (materialArray);
		setLights ();
	}
}
