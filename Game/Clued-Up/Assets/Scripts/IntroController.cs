using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Controls the introduction into a new game
/// </summary>
public class IntroController : MonoBehaviour {
	/// <summary>
	/// The audio source.
	/// </summary>
	public GameObject audioSource;
	/// <summary>
	/// The snow generator.
	/// </summary>
	public GameObject snowGenerator;
	/// <summary>
	/// The rain generator.
	/// </summary>
	public GameObject rainGenerator;
	/// <summary>
	/// The intro text object.
	/// </summary>
	public GameObject introTextObject;
	/// <summary>
	/// The overlay panel.
	/// </summary>
	public GameObject overlayPanel;
	/// <summary>
	/// The background plane.
	/// </summary>
	public GameObject backgroundPlane;
	/// <summary>
	/// The story object.
	/// </summary>
	public GameObject storyObject;

	/// <summary>
	/// Fades the text.
	/// </summary>
	/// <param name="alpha">Alpha.</param>
	/// <param name="time">Time.</param>
	private void fadeText(float alpha, float time){ 
		introTextObject.GetComponent<Text>().CrossFadeAlpha(alpha,time,false);
	}

	/// <summary>
	/// Displays the introtext.
	/// </summary>
	/// <param name="introText">Intro text.</param>
	private void displayText(string introText){	
		//will change UI Text object and fade in
		introTextObject.GetComponent<Text>().text = introText;
		fadeText (1f, 3f);
	}

	/// <summary>
	/// Plays the intro to the game
	/// </summary>
	/// <returns>The cutscene.</returns>
	/// <param name="story">Story.</param>
	IEnumerator IntroCutscene(Story story){
		//plays scream, fades and updates text
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
	/// Start this instance.
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
	/// if space is held down during then skip the intro
	/// </summary>
	void Update() {
		if (Input.GetKeyDown("space"))
			SceneManager.LoadScene (2);	//loads character selection scene

	}
}
