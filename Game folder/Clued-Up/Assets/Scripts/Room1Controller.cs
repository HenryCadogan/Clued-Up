using UnityEngine;
using System.Collections;

public class Room1Controller : MonoBehaviour {
	public GameObject backgroundPane;
	public GameObject overlayPanel;
	public GameObject snowGenerator;
	public GameObject rainGenerator;
	public GameObject mainLight;

	private Story story;
	// Use this for initialization

	private void setLights(){
		//sets lighting to weather appropriate colour
		int weather = story.getWeather ();
		Debug.Log ("Lights set");
		switch (weather) {
		case 0:
			mainLight.GetComponent<Light> ().color = Color.yellow; //sunny
			break;
		case 1:
			mainLight.GetComponent<Light> ().color = new Color (0f, 0f, 0.5f); //rain
			break;
		case 2:
			mainLight.GetComponent<Light> ().color = new Color (0.4f, 0.25f, 0f); //
			break;
		case 3:
			mainLight.GetComponent<Light> ().color = Color.grey; //snow
			break;
		}
			
	}

	private void setBackground(Material[] mats){
		int weather = story.getWeather ();
		backgroundPane.GetComponent<Renderer> ().material = mats [weather];
		if (weather == 1) {	//set generators depending on weather
			rainGenerator.SetActive (true);
		} else if (weather == 3) {
			snowGenerator.SetActive (true);
		}
	}

	void Start () {
		story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story

		Material[] materialArray = new Material[4];
		materialArray[0] = (Material)Resources.Load("Room1Sunny", typeof(Material)); //finds material located in the resources folder
		materialArray[1] = (Material)Resources.Load("Room1Rain", typeof(Material));
		materialArray[2] = (Material)Resources.Load("Room1Sunset", typeof(Material));
		materialArray[3] = (Material)Resources.Load("Room1Snow", typeof(Material));

		setBackground (materialArray);
		setLights ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
