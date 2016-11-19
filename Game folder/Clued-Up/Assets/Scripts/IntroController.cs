using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {
	public GameObject audioSource;
	public GameObject snowGenerator;
	public GameObject rainGenerator;
	public GameObject introText;
	public GameObject overlayPanel;
	public GameObject backgroundPlane;
	public GameObject story;


	//new ienumerable 




	// Use this for initialization
	void Start () {
		Material[] materialArray = new Material[4];
		materialArray[0] = (Material)Resources.Load("RonCookeHubMurderSunny", typeof(Material)); //finds material located in the resources folder
		materialArray[1] = (Material)Resources.Load("RonCookeHubMurderRain", typeof(Material));
		materialArray[2] = (Material)Resources.Load("RonCookeHubMurderSunset", typeof(Material));
		materialArray[3] = (Material)Resources.Load("RonCookeHubMurderSnow", typeof(Material));

		int weather = story.GetComponent<Story> ().setWeather (materialArray); // get weather from story; 0=sunny 1=rainy 2=sunset 3=snow

		if (weather == 1) {	//set generators depending on weather
			rainGenerator.SetActive (true);
		} else if (weather == 3) {
			snowGenerator.SetActive (true);
		}
		//todo make overlaypanel opaque and text transparent
		backgroundPlane.GetComponent<Renderer> ().material = materialArray [weather]; //change background image to be the correct image

		//initialisation done, now pass to a coroutine so that time delays can be done

	}
}
