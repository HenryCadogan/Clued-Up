using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {
	private string type;
	private byte openLockers = 0000; // only used if type == "lockers"

	// Use this for initialization
	public void Initialise(string type){
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Furniture/" + type);
		this.type = type;
		this.name = type;
	}
}
