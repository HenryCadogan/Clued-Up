using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Controls the introduction sequence in a new game
/// </summary>
public class IntroController : MonoBehaviour {
	/// <summary>
	/// The audio source GameObject.
	/// </summary>
	public GameObject audioSource;
	/// <summary>
	/// The snow generator prefab GameObject.
	/// </summary>
	public GameObject snowGenerator;
	/// <summary>
	/// The rain generator prefab GameObject.
	/// </summary>
	public GameObject rainGenerator;
	/// <summary>
	/// The intro textbox GameObject.
	/// </summary>
	public GameObject introTextObject;
	/// <summary>
	/// The overlay panel GameObject used for fading.
	/// </summary>
	public GameObject overlayPanel;
	/// <summary>
	/// The background plane GameObject used for displaying a background material.
	/// </summary>
	public GameObject backgroundPlane;
	/// <summary>
	/// The main story GameObject.
	/// </summary>
	public GameObject storyObject;

	/// <summary>
	/// Fades the introduction text in/out for specified time.
	/// </summary>
	/// <param name="alpha">Alpha component where 0f is transparent and 1f is all black</param>
	/// <param name="time for fade, in seconds">Time.</param>
	private void fadeText(float alpha, float time){ 
		introTextObject.GetComponent<Text>().CrossFadeAlpha(alpha,time,false);
	}
	/// <summary>
	/// Displays specified string as the intro text.
	/// </summary>
	/// <param name="introText">Text string to set the intro textbox</param>
	private void displayText(string introText){	
		introTextObject.GetComponent<Text>().text = introText;
		fadeText (1f, 3f);
	}

	/// <summary>
	/// Plays the main intro sequence cutscene for the game
	/// </summary>
	/// <returns>Yields WaitForSeconds.</returns>
	/// <param name="story">Main game story</param>
	IEnumerator IntroCutscene(Story story){
		audioSource.GetComponent<AudioSource> ().Play (); //play scream
		yield return new WaitForSeconds(1f); //wait 1 second after scream begins

		displayText (story.getIntro1()); //gets text and fades in (in background)
		yield return new WaitForSeconds(4f); // wait three secs for fade, and one second after the fade ends
		overlayPanel.GetComponent<Image> ().CrossFadeAlpha (0f, 3f, false); // fade out black overlay
		fadeText (0f, 3.5f); //fade text out
		yield return new WaitForSeconds(4f);


		displayText (story.getIntro2());
		yield return new WaitForSeconds(4f);
		fadeText (0f, 3.5f);
		yield return new WaitForSeconds(4f);

		displayText (story.getIntro3());
		yield return new WaitForSeconds(4f);
		fadeText (0f, 3.5f);
		overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 4f, false);
		yield return new WaitForSeconds(4f); //wait for the overlay to fade in entirely

		SceneManager.LoadScene (2);	//loads character selection scene
	}
		
	/// <summary>
	/// Initialises weather & background for scene, sets story, and loads cutscene
	/// </summary>
	void Start () {
		Story story = storyObject.GetComponent<Story> (); // reference to story object (in Story script in Story GameObject)
		Material[] materialArray = new Material[4];
		materialArray[0] = (Material)Resources.Load("RonCookeHubMurderSunny", typeof(Material)); //finds material located in the resources folder
		materialArray[1] = (Material)Resources.Load("RonCookeHubMurderRain", typeof(Material));
		materialArray[2] = (Material)Resources.Load("RonCookeHubMurderSunset", typeof(Material));
		materialArray[3] = (Material)Resources.Load("RonCookeHubMurderSnow", typeof(Material));

		int weather = story.setWeather (materialArray); // get weather from story; 0=sunny 1=rainy 2=sunset 3=snow

		if (weather == 1) {	//set generators depending on weather
			rainGenerator.SetActive (true);
		} else if (weather == 3) {
			snowGenerator.SetActive (true);
		}
		backgroundPlane.GetComponent<Renderer> ().material = materialArray [weather]; //change background image to be the correct image
		fadeText(0f,0); //instantaneously fade text out

		story.setStory ();

		//initialisation done, now pass to a coroutine so that time delays can be done
		StartCoroutine(IntroCutscene(story));
	}

	/// <summary>
	/// If space is pressed during this scene, then skip the intro
	/// </summary>
	void Update() {
		if (Input.GetKeyDown("space"))
			SceneManager.LoadScene (2);	//loads character selection scene
	}
}
