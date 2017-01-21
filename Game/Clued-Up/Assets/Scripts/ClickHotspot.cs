using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickHotspot : MonoBehaviour {
	/// <summary>
	/// Message to appear when hotspot is clicked
	/// </summary>
	public string name;
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
	private bool entered;

	/// <summary>
	/// Plaies the sound when an object is clicked.
	/// </summary>
	/// <param name="soundClip">Sound clip.</param>
	private void playSound(string soundClip){
		this.audioSource.clip = Resources.Load<AudioClip> ("Sounds/" + soundClip);
		this.audioSource.Play ();
	}

	/// <summary>
	/// Selects message from list of messages for each hotspot
	/// </summary>
	/// <returns>The message.</returns>
	/// <param name="name">Name of hotspot.</param>
	private string getMessage(string name){
		List <string> messages = new List<string> ();
		switch (name) {
		//Room1 outside
		case "bikes":
			messages.Add ("Apparently some of the detectives enjoy a nice cycle to work.");
			messages.Add ("Riding a bike is great exercise.");
			break;
		case "pod3":
			messages.Add ("Another ruddy pod.");
			messages.Add ("Nothing inside there. Trust me.");
			break;
		case "pod2":
			messages.Add ("These have been locked all day, so there's nothing  inside.");
			messages.Add ("Apparently people like to study in those things...");
			break;

		//room2 lobby
		case "18":
			messages.Add ("No minors past this point!");
			break;
		case "barsign":
			messages.Add ("Good job there's a bar nearby. I could use a drink.");
			messages.Add ("A student's favourite place to be.");
			break;
		case "unisign":
			messages.Add ("Welcome to the University of York.");
			messages.Add ("The Uni where dreams are made (or something like that).");
			break;
		case "trainTimes":
			messages.Add ("Trains are no longer stopping here,.");
			messages.Add ("All aboard!");
			break;

		//room3 train station
		case "trainSign":
			messages.Add ("Helpful directions...");
			break;

		//room4 cafe
		case "18stand":
			messages.Add ("Bar this way!");
			messages.Add ("Who wants a beer? Or maybe a treble?");
			break;
		case "cafeCounter":
			messages.Add ("Now's not the time for a snack.");
			messages.Add ("Mmm... Overpriced sandwiches.");
			break;

		//room5 kitchen
		case "pan":
			messages.Add ("Sadly there's nothing in the pan");
			break;
		case "broth":
			messages.Add ("The broth is coming along nicely.");
			messages.Add ("Minestrone, my favourite.");
			break;
		case "baker":
			messages.Add ("Does anybody actually know what this does?");
			messages.Add ("It's locked! What a surprise.");
			break;
		case "bacon":
			messages.Add ("Who doesn't love bacon.");
			messages.Add ("A vegetarian's delight.");
			break;
		case "tongs":
			messages.Add ("I don't think these tongs have been used in suspicious circumstances... Or have they?");
			messages.Add ("I wonder how much bacon these have picked up in their lifetime?");
			break;
		case "grill":
			messages.Add ("Where the magic happens.");
			messages.Add ("It's not on, don't worry.");
			break;
		case "wetFloor":
			messages.Add ("Ooops! Spillage!");
			messages.Add ("Thou shalt not pass the sign. (Health and saftey gone mad etc.)");
			break;

		//room6 bar
		case "beerTaps":
			messages.Add ("Brought to you by the awesome programming team; Toby, Simon, Henry and Will.");
			break;
		case "barDeals":
			messages.Add ("I've always wanted to try a weird beer.");
			messages.Add ("Such a bargain!");
			break;

		//room7 studio
		case "keyboard":
			messages.Add ("Everybody loves these.");
			messages.Add ("Remember these from Y9 music?");
			break;
		case "controlroom":
			messages.Add ("Through the glass is where the producing magic happens.");
			break;
		case "kit":
			messages.Add ("Paradiddle. Flan. Now you're a drummer ;) ");
			messages.Add ("Legend has it, this kit was used on Stormzy's album.");
			break;
		case "painting":
			messages.Add ("Persistance Of Memory: a true masterpeice.");
			messages.Add ("Who needs music when you can paint?");
			break;
		case "controldoor":
			messages.Add ("It's locked! Theres nothing of interest in there anyway...");
			break;
		case "guitar":
			messages.Add ("Dirty deeds indeed come dirt cheap.");
			messages.Add ("Gibson Les Paul Special - arguably the most versitile guitar.");
			break;
		case "piano":
			messages.Add ("I think it's in tune!");
			messages.Add ("Love a bit of classical every once in a while.");
			break;


		//room8 toilets
		case "doors95":
			messages.Add ("The new OS from Megahard coming soon.");
			messages.Add ("Have you heard it can support multitasking?!");
			break;
		case "murderPoster":
			messages.Add ("The Great Ron Cooke Hub Murder, in cinemas July 2017.");
			break;
		case "burglarKing":
			messages.Add ("Have it my way.");
			messages.Add ("Finger sucking good.");
			break;
		case "beerBottles":
			messages.Add ("Someone's been drinking recently...");
			messages.Add ("I guess it's this way to the bar.");
			break;
		case "puddle":
			messages.Add ("I hope that's spilled beer.");
			break;
		case "condomMachine":
			messages.Add ("Better safe than sorry...");
			messages.Add ("I'm still not sure why we included this.");
			break;


		default:
			throw new System.ArgumentOutOfRangeException ("No messages for hotspot \'" + name + "\'.");
		}

		if (messages.Count > 0) {
			return messages [(int)Random.Range (0f, messages.Count)];
		} else {
			return "Message missing";
		}
	}

	// Use this for initialization
	void Start () {
		HUDC = GameObject.Find ("HUD").GetComponent<HUDController> ();
	}

	void OnMouseDown(){
		if (name != "") {
			HUDC.displayHUDText (getMessage (name));
		}
		if (this.soundClip != "") {
			this.playSound (this.soundClip);
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

	void Update(){
		if (entered) {
			Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		}
	}
}
