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
	public AudioSource audioSource;
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
	public Text introTextObject;
	/// <summary>
	/// The overlay panel GameObject used for fading.
	/// </summary>
	public Image overlayPanel;
	/// <summary>
	/// The background plane GameObject used for displaying a background material.
	/// </summary>
	public Renderer backgroundPlane;

	/// <summary>
	/// Fades the introduction text in/out for specified time.
	/// </summary>
	/// <param name="alpha">Alpha component where 0f is transparent and 1f is all black</param>
	/// <param name="time">time for fade, in seconds</param>
	private void fadeText(float alpha, float time){ 
		introTextObject.CrossFadeAlpha(alpha,time*5,false);
	}
	/// <summary>
	/// Displays specified string as the intro text.
	/// </summary>
	/// <param name="introText">Text string to set the intro textbox</param>
	private void displayText(string introText){	
		introTextObject.text = introText;
		fadeText (1f, 0.1f);
	}
	private Story story;

	/// <summary>
	/// Plays the main intro sequence cutscene for the game
	/// </summary>
	/// <returns>Yields WaitForSeconds.</returns>
	/// <param name="story">Main game story</param>
	IEnumerator IntroCutscene(Story story){
		audioSource.Play (); //play scream
	
		//yield return new WaitForSeconds(1f); //wait 1 second after scream begins
		displayText (story.getIntro1()); //gets text and fades in (in background)
		yield return new WaitForSeconds(2f); // wait three secs for fade, and one second after the fade ends
		overlayPanel.CrossFadeAlpha (0f, 0.5f, false); // fade out black overlay
		fadeText (0f, 2f); //fade text out
		yield return new WaitForSeconds(1f);


		displayText (story.getIntro(2));
		yield return new WaitForSeconds(1f);
		fadeText (0f, 0.5f);
		yield return new WaitForSeconds(1f);

		displayText (story.getIntro(3));
		yield return new WaitForSeconds(1f);
		fadeText (0f, 0.5f);
		overlayPanel.CrossFadeAlpha (1f, 0.5f, false);
		yield return new WaitForSeconds(1f); //wait for the overlay to fade in entirely

		SceneManager.LoadScene (2);	//loads character selection scene
		yield return null;

	}
		
	/// <summary>
	/// Initialises weather & background for scene, sets story, and loads cutscene
	/// Instantiates ew Story gameobject so that the object can safely be destroyed and reloaded when the game is played multiple times.
	/// </summary>
	void Start () {
		Material[] materialArray = new Material[4];
		materialArray[0] = (Material)Resources.Load("RonCookeHubMurderSunny", typeof(Material)); //finds material located in the resources folder
		materialArray[1] = (Material)Resources.Load("RonCookeHubMurderRain", typeof(Material));
		materialArray[2] = (Material)Resources.Load("RonCookeHubMurderSunset", typeof(Material));
		materialArray[3] = (Material)Resources.Load("RonCookeHubMurderSnow", typeof(Material));
		GameObject StoryObject = new GameObject("Story");
		StoryObject.AddComponent<Story> ();

		this.story = StoryObject.GetComponent<Story> ();

		int weather = story.setWeather (materialArray); // get weather from story; 0=sunny 1=rainy 2=sunset 3=snow

		if (weather == 1) {	//set generators depending on weather
			rainGenerator.SetActive (true);
		} else if (weather == 3) {
			snowGenerator.SetActive (true);
		}
		backgroundPlane.material = materialArray [weather]; //change background image to be the correct image
		fadeText(0f,0f); //instantaneously fade text out

		story.setStory ();
		//initialisation done, now pass to a coroutine so that time delays can be done
		StartCoroutine(IntroCutscene(story));
	}

	/// <summary>
	/// If space is pressed during this scene, then skip the intro
	/// </summary>
	void Update() {
		if (Input.GetKeyDown ("space")){
			story.setDetective (0);
			SceneManager.LoadScene (2);	//loads character selection scene
		}
	}
}
