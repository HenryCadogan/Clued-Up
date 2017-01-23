using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	public GameObject detective1;
	public GameObject detective2;
	public GameObject detective3;
	// Use this for initialization
	void Start () {
		int rand = Random.Range (0, 3);
		switch (rand) {
		case 0:
			detective1.SetActive (true);
			break;
		case 1:
			detective2.SetActive (true);
			break;
		case 2:
			detective3.SetActive (true);
			break;
		default:
			throw new System.IndexOutOfRangeException ();
		}
	}
}
