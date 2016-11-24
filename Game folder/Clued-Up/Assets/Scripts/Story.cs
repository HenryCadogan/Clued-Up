using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {
	public static Story Instance;//Makes the object persistant throughout scenes


	private int weather = 3; // 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy. 0 set for debug
	private int detective; // int set by user in character selection

	void Awake ()  
	//Makes the object persistant throughout scenes
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}
		

	public string getIntro1(){
		//returns first sentence of introduction
		string weatherString;
		switch (weather) {
		case 0:
			weatherString = "beautiful, sunny afternoon";
			break;
		case 1:
			weatherString = "wet stormy night";
			break;
		case 2:
			weatherString = "warm autumn's sunset";
			break;
		case 3:
			weatherString = "cold, snowy evening";
			break;
		default:
			throw new System.ArgumentOutOfRangeException ("Weather out of range");
		}

		string intro1 = "It was a " + weatherString + " at the Ron Cooke Hub, and some of the world's greatest detectives were enjoying a costume party.";
		return intro1;
	}

	public string getIntro2(){
		//returns second sentence of introduction
		string intro2 = "A loud shreik and thud allerted the guests to find a dead body on the ground outside. There had been a murder.";
		return intro2;
	}

	public string getIntro3(){
		//returns third sentence of introduction
		string intro3 = "But whodunnit? Only you can find the truth.";
		return intro3;
	}
		
	public int setWeather(Material[] ma){
		//sets weather condition to random int within range of array of all possible weather conditions, and returns this int.
		weather = Random.Range (0, ma.Length);
		return weather;
	}

	public int getWeather(){
		return weather;
	}

	public void setDetective(int detectiveInt){
		//sets detective to int chosen by user in Character Selection
		detective = detectiveInt;
		Debug.Log ("You have chosen dectective " + detective.ToString ());
	}
}