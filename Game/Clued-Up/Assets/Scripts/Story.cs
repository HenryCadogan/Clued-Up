using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Story : MonoBehaviour {
	public static Story Instance;//Makes the object persistant throughout scenes


	private int weather = 2; // 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy. 0 set for debug
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

	private string randomLineFrom(string filename){
		//returns a random line from specified file in resources
		StreamReader stream = new StreamReader(filename);
		List<string> lines = new List<string> ();

		while(!stream.EndOfStream){
			lines.Add(stream.ReadLine());
		}
		stream.Close( );

		return lines [Random.Range (0, lines.Count)];
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

		string intro1 = "It was a " + weatherString + randomLineFrom("Assets/TextFiles/intro1.txt");
		return intro1;
	}

	public string getIntro2(){
		return randomLineFrom("Assets/TextFiles/intro2.txt");
	}

	public string getIntro3(){
		return randomLineFrom ("Assets/TextFiles/intro3.txt");
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

	public int getDetective(){
		return this.detective;
	}





	public void setStory(){
		Debug.Log ("START");
	}
}