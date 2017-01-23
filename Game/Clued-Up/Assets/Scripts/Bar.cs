using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Bar : MonoBehaviour {

	public Image fullPint;
	public Image stream;
	public AudioSource source;
	public AudioSource jukeboxSource;
	public float speed;
	public int song;
	private bool hasBeenPoured = false;
	private bool pouring = false;
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
