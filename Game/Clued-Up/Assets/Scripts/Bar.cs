using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// A class dedicated to various details in the Bar.
/// </summary>
public class Bar : MonoBehaviour {

	/// <summary>
	/// An image file of the full pint.
	/// </summary>
	public Image fullPint;
	/// <summary>
	/// An image file of the stream of beer.
	/// </summary>
	public Image stream;
	/// <summary>
	/// The AudioSource component of the beer pump.
	/// </summary>
	public AudioSource source;
	/// <summary>
	/// The AudioSource component of the jukebox.
	/// </summary>
	public AudioSource jukeboxSource;
	/// <summary>
	/// The rate at which the pint fills.
	/// </summary>
	public float speed;
	/// <summary>
	/// An index of the current song playing in the jukebox
	/// </summary>
	public int song;
	/// <summary>
	/// A flag storing whether the pint has been poured or not.
	/// </summary>
	private bool hasBeenPoured = false;
	/// <summary>
	/// A state storing whether or not the beer is being poured.
	/// </summary>
	private bool pouring = false;
	/// <summary>
	/// A state storing whether or not the mouse cursor is placed over the beer pump or not.
	/// </summary>
	private bool entered = false;


	/// <summary>
	/// When Pump is clicked begin pouring
	/// </summary>
	public void OnMouseDown(){
		if (song != -1) {	//if there is a song property, play song
			switch (song) {
			case 0: 
				jukeboxSource.clip = Resources.Load<AudioClip> ("Sounds/rock-song1");
				break;
			case 1: 
				jukeboxSource.clip = Resources.Load<AudioClip> ("Sounds/rock-song2");
				break;
			case 2: 
				jukeboxSource.clip = Resources.Load<AudioClip> ("Sounds/dubstep-song");
				break;
			case 3: 
				jukeboxSource.clip = Resources.Load<AudioClip> ("Sounds/acoustic-song");
				break;
			default:
				throw new System.ArgumentOutOfRangeException ("Song index is not valid");

			}
			jukeboxSource.Play ();

		}else if (!hasBeenPoured) { //otherwise the beer must have been clicked.
			source.Play ();
			hasBeenPoured = true;
			pouring = true;
		}
	}
	/// <summary>
	/// Initialises full pint underneath pint mask 
	/// </summary>
	void Start(){
		if (song == -1) { //the tap
			Vector3 pos = fullPint.transform.localPosition;
			pos.y = -1200;
			fullPint.transform.localPosition = pos;
		}
	}
	/// <summary>
	/// If beer is pouring, raise its posiion. Also if cursor is within the pump & it hasnt been poured, change the cursor
	/// </summary>
	void Update(){
		if (entered && !hasBeenPoured) {
			Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		}
		if (pouring) {
			if (fullPint.transform.localPosition.y >= 0) {
				pouring = false;
				stream.gameObject.SetActive (false);
			} else {
				Vector3 pos = fullPint.transform.localPosition;
				pos.y += speed * 10;
				fullPint.transform.localPosition = pos;

				pos = stream.transform.localPosition;
				pos.y -= 50;
				stream.transform.localPosition = pos;
			}
		}
	}

	void OnMouseEnter(){
		Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		entered = true;
	}
	void OnMouseExit(){
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		entered = false;
	}

}
