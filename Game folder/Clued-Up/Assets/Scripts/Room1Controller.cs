using UnityEngine;
using System.Collections;

public class Room1Controller : MonoBehaviour {
	public GameObject backgroundPane;
	public GameObject overlayPanel;
	public GameObject snowGenerator;
	public GameObject rainGenerator;

	private Story story;
	// Use this for initialization


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

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
