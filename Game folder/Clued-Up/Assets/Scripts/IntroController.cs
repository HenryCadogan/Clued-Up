using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {
	public GameObject audioSource;
	public GameObject snowGenerator;
	public GameObject rainGenerator;
	public GameObject introTextObject;
	public GameObject overlayPanel;
	public GameObject backgroundPlane;
	public GameObject storyObject;

	private void fadeText(float alpha, float time){ 
		introTextObject.GetComponent<Text>().CrossFadeAlpha(alpha,time,false);
	}

	private void displayText(string introText){	
		//will change UI Text object and fade in
		introTextObject.GetComponent<Text>().text = introText;
		fadeText (1f, 3f);
	}

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



	}




	// Use this for initialization
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

		//initialisation done, now pass to a coroutine so that time delays can be done
		StartCoroutine(IntroCutscene(story));
	}
}
