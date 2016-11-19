using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {
	private int weather; // 0 = sunny, 1 = rainy, 2 = sunset, 3 = snowy

	public int setWeather(Material[] ma){
		weather = Random.Range (0, ma.Length);
		return weather;
	}
}