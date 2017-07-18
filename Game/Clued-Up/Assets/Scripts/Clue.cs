using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// The clue class
/// </summary>

public class Clue : MonoBehaviour {
	/// <summary>
	/// //Long name displayed in game (i.e with spaces). Not to be confused with this.name which is the GameObject name
	/// </summary>
	public string longName;
	/// <summary>
	/// The description of the clue
	/// </summary>
	public string description;
	/// <summary>
	/// The 2D sprite of the clue
	/// </summary>
	public Sprite sprite;
	/// <summary>
	/// if this clue is the motive clue
	/// </summary>
	public bool isMotive;
	/// <summary>
	/// if this item should dissapear from the game when collected
	/// </summary>
	public bool disappearWhenClicked; 
	/// <summary>
	/// <c>True<c>/ Clue has been collected
	/// </summary>
	public bool isCollected = false;


	/// <summary>
	/// The clue description panel to be populated & shown when collected.
	/// </summary>
	private GameObject cluePanel;
	/// <summary>
	/// The clue title GameObject within the clue panel.
	/// </summary>
	private Text cluePanelName;
	/// <summary>
	/// The description GameObject within the clue panel.
	/// </summary>
	private Text cluePanelDescription;
	/// <summary>
	/// The image GameObject within CluePanel.
	/// </summary>
	private Image cluePanelImage;
	/// <summary>
	/// The overlay panel to be used for fading.
	/// </summary>
	private GameObject overlayPanel;
	/// <summary>
	/// The heads up display prefab
	/// </summary>
	private GameObject hud;
	private bool entered;

    // Data holder for clues
    private class ClueMeta
    {
        public string shortName;
        public string longName;
        public string description;
        public bool disappearWhenClicked;
        public float localScale;
        public bool isMotive;

        public ClueMeta(string shortName, string longName, string description, bool disappearWhenClicked=true,
                        float localScale=0.25f, bool isMotive=false)
        {
            this.shortName = shortName;
            this.longName = longName;
            this.description = description;
            this.disappearWhenClicked = disappearWhenClicked;
            this.localScale = localScale;
            this.isMotive = isMotive;
        }

    }

    private static Dictionary<string, ClueMeta> clueMetaDict = new Dictionary<string, ClueMeta>
    {
        { "chalkOutline",       new ClueMeta("chalkOutline", "Chalk Outline", "A chalk outline of the body of $VICTIM!", disappearWhenClicked:false, localScale: 1f) },
        { "microphone",         new ClueMeta("microphone", "Microphone", "Someone wants to make themselves heard") },
        { "wizzardHat",         new ClueMeta("wizzardHat", "Wizard's Hat", "Looks like part of a Halloween costume")},
        { "moustache",          new ClueMeta("moustache", "Fake Moustache", "Is someone trying to disguise themselves?")},
        { "pen",                new ClueMeta("pen", "Pen", "A fancy fountain pen. This could belong to $RANDOMCHAR or maybe it is $KILLER's")},
        { "plunger",            new ClueMeta("plunger", "Plunger", "Oh wow. A plunger!")},
        { "brownHair",          new ClueMeta("brownHair", "Brown Hair", "Whose hair is this colour?")},
        { "sandwich",           new ClueMeta("sandwich", "Sandwich", " A cheese sandwich. It’s half eaten. Delicious.")},
        { "stapler",            new ClueMeta("stapler", "Stapler", "Staples things.")},
        { "suitcase",           new ClueMeta("suitcase", "Suitcase", "Looks like someone’s forgot their suitcase. Oh no.")},
        { "wand",               new ClueMeta("wand", "Wand", "Casts spells and things.")},
        { "sunglasses",         new ClueMeta("sunglasses", "Sunglasses", "Why would you need sunglasses in York? Unless someone is trying to conceal their identity...")},
        { "whistle",            new ClueMeta("whistle", "Whistle", "It must belong to the train conductor...I wonder why it's here...")},
        { "whiteHair",          new ClueMeta("whiteHair", "White Hair", "Whose hair is this colour?")},
        { "money",              new ClueMeta("money", "Money", "Someone's been careless...or they've got too much money...", localScale:0.15f)},
        { "tape",               new ClueMeta("tape", "Tape", "The hottest mix tape to drop in 2017", localScale:0.1f)},
        { "bling",              new ClueMeta("bling", "Bling", "What kind of person would wear this around their neck?")},
        { "ladder",             new ClueMeta("ladder", "Ladder", "Oops. Someone dropped their ladder.")},
        { "ticket",             new ClueMeta("ticket", "Ticket", "A train ticket for use on the train.", localScale:0.1f)},
        { "coal",               new ClueMeta("coal", "Coal", "A singular lump of coal. Truly the greatest stocking stuffer.")},
        { "cloak",              new ClueMeta("cloak", "Invisibility Cloak", "Someone is trying to conceal themselves...")},
        { "blondHair",          new ClueMeta("blondHair", "Blond Hair", "Whose hair is this colour?")},
        { "chefHat",            new ClueMeta("chefHat", "Chef's Hat", "I wonder who this belongs to...")},
        { "comb",               new ClueMeta("comb", "Comb", "I wonder who would carry a comb with them...")},
        { "monocle",            new ClueMeta("monocle", "Monocle", "This must belong to someone important.")},
        { "feather",            new ClueMeta("feather", "Feather", "Where could this feather have come from?")},
        { "lighter",            new ClueMeta("lighter", "Lighter", "Somoeone dropped their lighter. Oops.")},
        { "blackHair",          new ClueMeta("blackHair", "Black Hair", "Whose hair is this colour?")},
        { "hammer",             new ClueMeta("hammer", "Hammer", "A bloody hammer.", true)},
        { "gun",                new ClueMeta("gun", "Gun", "A smoking gun.", true)},
        { "knife",              new ClueMeta("knife", "Knife", "A bloody knife.", true)},
        { "salmon",             new ClueMeta("salmon", "Salmon", "A bloody salmon that has been handled under suspicious circumstances.", true)},
        { "polaroid",           new ClueMeta("polaroid", "Polaroid", "A dirty, crumpled polaroid. You can vaguely make out $VICTIM and someone else. They have their arms around one another, and they’re laughing. You can see written on the back of the photo a pair of initials. You realise that these must be the initials of the people in the photo. One of which is our victim..." , isMotive:true)},
        { "recorder",           new ClueMeta("recorder", "Recorder", "The quality is terrible, but you can just about make out the voices. It appears to be two people having a conversation. You think you recognise one of the voices as being $VICTIM. The other voice sounds similar to $KILLER but you are very unsure if it is really them. At first they are calm, and you can’t quite hear what they’re saying. But gradually it gets louder, their voices rising as their anger bubbles up. Everything goes quiet. There seems to be some muffled shuffling in the background. The recording cuts off. Everything is starting to make sense...", isMotive:true)},
        { "diary",              new ClueMeta("diary", "Diary", "$VICTIM’s diary. You know you shouldn’t be reading it, but you find yourself flicking through the pages anyway. After all, it could be used for evidence, right? You turn to the most recent entry. The day before the party. You can’t help noticing how many times $KILLER has been etched into the page with vigor. You read the entire page before slamming the book shut.", isMotive:true, localScale:0.15f)},
        { "letter",             new ClueMeta("letter", "Letter", "A crumpled letter. The writing is small and scrawling, but if you squint you can just make it out. It appears to be addressed to $VICTIM. It’s messy and smudged, so you don’t attempt to read the entire thing, but you can make out the names $RANDOMCHAR and $KILLER. How very suspicious..." , isMotive:true, localScale:0.2f)},
    };


    /// <summary>
    /// Returns the sprite for the given clue, loading it from resources.
    /// </summary>
    /// <param name="clueName"></param>
    /// <returns></returns>
    public static Sprite getClueSprite(string clueName)
    {
        Sprite sprite = Resources.Load<Sprite>("Clues/" + clueName);
        if (sprite == null)
        {
            throw new Exception("Couldn't find a sprite for the clue " + clueName);
        }
        return sprite;
    }

    public static string getClueLongName(string shortName)
    {
        ClueMeta meta = clueMetaDict[shortName];
        return meta.longName;
    }

    public static string getClueDescription(string shortName)
    {
        // Access the story to get the name of the killer etc so that can be substitued into the descriptions
        ClueMeta meta = clueMetaDict[shortName];
        string victim = Story.Instance.getVictim().longName;
        string killer = Story.Instance.getMurderer().longName;
        string randomCharacter = Story.Instance.randomAliveCharacter().longName;

        string description = meta.description
            .Replace("$VICTIM", victim)
            .Replace("$KILLER", killer)
            .Replace("$RANDOMCHAR", randomCharacter);
        return description;
    }
    
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Clue"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Clue"/>.</returns>
	public override string ToString ()
	{
		return this.longName;
	}

    /// <summary>
    /// Populates and displays the clue information on screen.
    /// </summary>
    public void displayClueInformation(){
		//Locate various UI elements
		this.cluePanel = GameObject.Find ("CluePanel");
		this.cluePanelDescription = GameObject.Find ("Description").GetComponent<Text> ();
		this.cluePanelName = GameObject.Find ("ClueName").GetComponent<Text> ();
		this.cluePanelImage = GameObject.Find ("ClueImage").GetComponent<Image> ();
		this.overlayPanel = GameObject.Find ("OverlayPanel");
		//Assign text to some of the UI components.
		this.cluePanelName.text = this.longName;
		this.cluePanelImage.sprite = this.sprite;
		this.cluePanelDescription.text = this.description;
		this.cluePanel.GetComponent<CanvasGroup>().alpha = 1f; //change opacity to 0.3 instantaneously

		this.overlayPanel.GetComponent<CanvasGroup> ().alpha = 0.5f; //make overlay canvas group visible
		this.overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 0f, false); //instantaneously fade overlay panel. This is needed becuse initial fade leaves <Image> alpha at 0
		this.overlayPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true; //make it so nothing else on screen can be interacted with while overlay panel is on

	}		
	/// <summary>
	/// Activated when GameObject clicked on. If it's the first time, send message to HUD, add it to inventory, display clue.
	/// </summary>
	void OnMouseDown(){
		Inventory inventory = GameObject.Find ("Detective").GetComponent<Inventory> ();
		if (Time.timeScale != 0) {	//if game isn't paused
			if (!(inventory.isCollected (this.name))) {
				inventory.collect (this.name);
				this.gameObject.GetComponent<AudioSource> ().Play ();
				hud.GetComponent<HUDController> ().displayHUDText (this.longName + " added to inventory.");
				this.isCollected = true;

				if (this.name == "chalkOutline") { // add victim to character notebook
					inventory.encounter (GameObject.Find ("Story").GetComponent<Story> ().getVictim ());
				}				
				displayClueInformation ();

				if (this.disappearWhenClicked) {
					gameObject.GetComponent<Renderer> ().enabled = false;	//hides but does not destroy the clue
				}
			}
		}
	}
	/// <summary>
	/// Initialise the Clue with properties and finds sprite with same name as this clue which it also scales.
	/// </summary>
	/// <param name="clueName">Object name of the clue</param>
	public void initialise(string clueName){
        ClueMeta meta;
        clueName = clueName.Trim();

        if (!clueMetaDict.ContainsKey(clueName))
        {
            Debug.Log("clueMetaDict has no entry for " + clueName);
            throw new KeyNotFoundException();
        }
        else
        {
            try
            {
                meta = clueMetaDict[clueName];
            }
            catch (KeyNotFoundException)
            {
                Debug.Log("Valid keys are: ");
                foreach (var key in Clue.clueMetaDict.Keys)
                {
                    Debug.Log(key);
                }
                throw;
            }
        }

		this.hud = GameObject.Find ("HUD");
		this.name = meta.shortName;
		this.longName = meta.longName;
		this.description = getClueDescription(clueName);
		this.isMotive = meta.isMotive;
		this.disappearWhenClicked = meta.disappearWhenClicked;
        this.sprite = getClueSprite(clueName);
		this.transform.localScale = new Vector3(meta.localScale,meta.localScale,meta.localScale);
	}

    /// <summary>
    /// On mouse enter, when the mouse enters the trigger zone, shows magnifyiing glass
    /// </summary>
    void OnMouseEnter(){
		Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		entered = true;
	}
    /// <summary>
    /// On mouse exit, when the mouse leaves the trigger zone, shows normal cursor
    /// </summary>
    void OnMouseExit(){
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		entered = false;
	}
    /// <summary>
    ///  Called every frame, Sets cursor
    /// </summary>
    void Update(){
		if (entered) {
			Cursor.SetCursor (Resources.Load<Texture2D> ("clueCursor"), Vector2.zero, CursorMode.Auto);
		}
	}
}
