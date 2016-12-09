using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the movement and propertiesof the character
/// </summary>
public class Detective : MonoBehaviour {
	/// <summary>
	/// The z coordinate the detective should be forced to have, making him unable to walk towards the camera.
	/// </summary>
	public float maxZ;
	/// <summary>
	/// The animator GameObject used by the detective.
	/// </summary>
	public Animator anim;
	/// <summary>
	/// The footsteps audio source.
	/// </summary>
	public AudioSource footstepsAudioSource;
	/// <summary>
	/// //The next walk in direction to be used when switching between scenes
	/// </summary>
	public bool walkInDirectionIsLeft = true; 
	/// <summary>
	/// The detective can walk right (i.e. not forced not to).
	/// </summary>
	public bool canWalkRight = true;
	/// <summary>
	/// The detective can walk left (i.e. not forced not to).
	/// </summary>
	public bool canWalkLeft = true;

	/// <summary>
	/// The main story.
	/// </summary>
	public Story story;
	/// <summary>
	/// The current direction the detective is facing.
	/// </summary>
	private string direction = "right";

	/// <summary>
	/// Starts or stops the detective walking animations, movement and footsteps
	/// </summary>
	/// <param name="walking">If set to <c>true</c> the character will walk.</param>
	private void setWalk(bool walking){
		if (anim.GetBool ("walking") != walking) { // if not already walking... need this to keep the footsteps from restarting  every frame
			anim.SetBool ("walking", walking);
			if (walking) {
				footstepsAudioSource.Play ();
			} else {
				footstepsAudioSource.Stop ();
			}
		}
	}
	private void setWalkSound(){
		switch (SceneManager.GetActiveScene().buildIndex){
		case 3: //room0 crime scene
			if (story.getWeather () == 1) { 	//rainy
				footstepsAudioSource.clip = (AudioClip)Resources.Load ("Sounds/footsteps-wet");
			} else if (story.getWeather () == 3) { //snow
				footstepsAudioSource.clip = (AudioClip)Resources.Load ("Sounds/footsteps-snow");
			} else {
				footstepsAudioSource.clip = (AudioClip)Resources.Load ("Sounds/footsteps-concrete");
			}
			break;
		case 4: //room1 lobby
			footstepsAudioSource.clip = (AudioClip)Resources.Load ("Sounds/footsteps-concrete");
			break;
		case 5: //room2 train station
			footstepsAudioSource.clip = (AudioClip)Resources.Load ("Sounds/footsteps-concrete");
			break;
		case 6: //room3 cafe
			footstepsAudioSource.clip = (AudioClip)Resources.Load ("Sounds/footsteps-concrete");
			break;
		default:
			Debug.Log ("Error, room index out of range!");
			break;
		}
	}
	/// <summary>
	/// Changes the y-direction of the detective GameObject to make him face differernt directions
	/// </summary>
	/// <param name="direction">Direction to turn the player, either "right" "left" or "centre".</param>
	private void turnPlayer(string direction){
		if (direction == "right") {
			gameObject.transform.localEulerAngles = new Vector3 (0, 90, 0); 
		} else if (direction == "left") {
			gameObject.transform.localEulerAngles = new Vector3 (0, 270, 0);
		} else {
			gameObject.transform.localEulerAngles = new Vector3 (0, 180, 0);
		}

		this.direction = direction;
	}
	/// <summary>
	/// Player walks onto the scene from specific direction. Called from SceneController scripts.
	/// </summary>
	public void walkIn(){
		if (this.walkInDirectionIsLeft) { //if position should be from left
			this.direction = "right";	//sets facing right
			turnPlayer("right");
			Vector3 pos = this.transform.position;
			pos.x = -10f;
			this.transform.position = pos;
		} else {
			this.direction = "left";	//sets facing left
			turnPlayer("left");
			Vector3 pos = this.transform.position;
			pos.x = 10f;
			this.transform.position = pos;
		}
		setWalkSound ();
		setWalk (true);
	}

	/// <summary>
	/// Initialises story var.
	/// </summary>
	void Awake(){
		story = GameObject.Find ("Story").GetComponent<Story> ();
	}
						
	/// <summary>
	/// Handles player walking toggle, direction & fixes Z position of detective GameObject to stop him walking forwards
	/// </summary>
	void Update () {
		if (Input.GetKey ("right") && (Time.timeSinceLevelLoad > 1f)) {
			if (direction != "right") {	//if previous direction is left or down, then turn the player
				turnPlayer ("right");
				this.canWalkRight = true;
			}
			setWalk (this.canWalkRight); //false if the player cannot move, true otherwise
		} else if (Input.GetKey ("left") && (Time.timeSinceLevelLoad > 1f)) {
			if (direction != "left") {	//if previous direction is right or down, then turn the player
				turnPlayer ("left");
				this.canWalkLeft = true;
			}
			setWalk (this.canWalkLeft);
		} else if (Input.GetKey ("down")  && (Time.timeSinceLevelLoad > 1f)) {
			turnPlayer ("down");
			setWalk (false);
		} else if (Time.timeSinceLevelLoad > 1f) { 
			setWalk (false);
		}	
				
		Vector3 pos = this.transform.position;
		pos.z = Mathf.Clamp (this.transform.position.x, maxZ, maxZ); //clamp z so detective cant walk forward
		this.transform.position = pos;
	}
}
