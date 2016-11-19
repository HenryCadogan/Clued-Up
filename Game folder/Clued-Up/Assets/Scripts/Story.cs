using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {
	private int weather; // 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy

	public string getIntro1(){
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
		string intro2 = "A loud shreik and thud allerted the guests to find a dead body on the ground outside. There had been a murder.";
		return intro2;
	}

	public string getIntro3(){
		string intro3 = "But whodunnit? Only you can find the truth.";
		return intro3;
	}

	public int setWeather(Material[] ma){
		weather = Random.Range (0, ma.Length);
		return weather;
	}
}