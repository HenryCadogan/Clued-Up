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

		SceneManager.LoadScene ("CharacterSelection");	//loads character selection scene
		yield return null;

	}
		
	/// <summary>
	/// Initialises weather & background for scene, sets story, and loads cutscene
	/// Instantiates ew Story gameobject so that the object can safely be destroyed and reloaded when the game is played multiple times.
	/// </summary>
	void Start () {
		Story story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story

        // Set RCH background material based on weather
		Material[] materialArray = new Material[4];
		materialArray[0] = (Material)Resources.Load("RonCookeHubMurderSunny", typeof(Material)); //finds material located in the resources folder
		materialArray[1] = (Material)Resources.Load("RonCookeHubMurderRain", typeof(Material));
		materialArray[2] = (Material)Resources.Load("RonCookeHubMurderSunset", typeof(Material));
		materialArray[3] = (Material)Resources.Load("RonCookeHubMurderSnow", typeof(Material));

        Material backgroundMaterial = materialArray[0];
        switch (story.getWeather())
        {
            case Story.WeatherOption.SUN:
                backgroundMaterial = materialArray[0];
                break;
            case Story.WeatherOption.RAIN:
                rainGenerator.SetActive (true);
                backgroundMaterial = materialArray[1];
                break;
            case Story.WeatherOption.SUNSET:
                backgroundMaterial = materialArray[2];
                break;
            case Story.WeatherOption.SNOW:
                snowGenerator.SetActive (true);
                backgroundMaterial = materialArray[2];
                break;
        }

        backgroundPlane.material = backgroundMaterial;
		fadeText(0f,0f); //instantaneously fade text out

		StartCoroutine(IntroCutscene(story));
	}

	/// <summary>
	/// If space is pressed during this scene, then skip the intro
	/// </summary>
	void Update() {
		if (Input.GetKeyDown ("space")){
            Story story = GameObject.Find("Story").GetComponent<Story>(); // references persistant object story
			story.setDetective (0);
			SceneManager.LoadScene ("CharacterSelection");	//loads character selection scene
		}
	}
}
