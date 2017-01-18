using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {



	// Use this for initialization
	void Start () {
		GameObject Colliders = GameObject.Find("MapColliders");
		Physics.queriesHitTriggers = true;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
		Debug.Log ("Mouse pressed");
	}
	void printThing(){
		Debug.Log ("Thing");
	}

}
