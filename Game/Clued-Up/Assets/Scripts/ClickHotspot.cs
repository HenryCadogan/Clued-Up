using UnityEngine;
using System.Collections;

public class ClickHotspot : MonoBehaviour {
	/// <summary>
	/// Message to appear when hotspot is clicked
	/// </summary>
	public string message;
	/// <summary>
	/// Sound to play when hotspot is clicked
	/// </summary>
	public string soundClip;
	/// <summary>
	/// SFX source of scene
	/// </summary>
	public AudioSource audioSource;

	/// <summary>
	/// HUD Controller
	/// </summary>
	private HUDController HUDC;

	/// <summary>
	/// Plaies the sound when an object is clicked.
	/// </summary>
	/// <param name="soundClip">Sound clip.</param>
	private void playSound(string soundClip){
		this.audioSource.clip = Resources.Load<AudioClip> ("Sounds/" + soundClip);
		this.audioSource.Play ();
	}

	// Use this for initialization
	void Start () {
		HUDC = GameObject.Find ("HUD").GetComponent<HUDController> ();
	}

	void OnMouseDown(){
		HUDC.displayHUDText (message);
		if (this.soundClip != "") {
			this.playSound (this.soundClip);
		}
	}
}
