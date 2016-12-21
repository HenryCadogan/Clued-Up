using UnityEngine;
using System.Collections;

public class ClickHotspot : MonoBehaviour {
	/// <summary>
	/// Message to appear when hotspot is clicked
	/// </summary>
	public string message;
	/// <summary>
	/// HUD Controller
	/// </summary>
	private HUDController HUDC;

	// Use this for initialization
	void Start () {
		HUDC = GameObject.Find ("HUD").GetComponent<HUDController> ();
	}

	void OnMouseDown(){
		HUDC.displayHUDText (message);
	}
}
