using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	/// The current direction the detective is facing.
	/// </summary>
	private string direction = "right";

	/// <summary>
	/// Starts or stops the detective walking animations, movement and footsteps
	/// </summary>
	/// <param name="walking">If set to <c>true</c> the character will walk.</param>
	private void setWalk(bool walking){
		anim.SetBool ("walking", walking);
		Debug.Log (footstepsAudioSource.isPlaying);
		if (walking && ( ! (footstepsAudioSource.isPlaying))) {
			footstepsAudioSource.Play ();
		} else if (footstepsAudioSource.isPlaying ){
			footstepsAudioSource.Stop ();
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
	}
	/// <summary>
	/// Player walks onto the scene from specific direction. Called from SceneController scripts
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
		setWalk (true);
	}
		


	/// <summary>
	/// Handles player walking toggle, direction & fixes Z position of detective GameObject to stop him walking forwards
	/// </summary>
	void Update () {
		// Walking annimations triggered if keys pressed
		if (Time.timeSinceLevelLoad > 1f){	//so player cannot walk out of scene at beginning or paused
			if (Input.GetKey ("right")) {
				if (direction != "right") {	//if previous direction is left or down, then turn the player
					turnPlayer ("right");
					direction = "right";
				}
				setWalk (true);

			} else if (Input.GetKey ("left")) {
				if (direction != "left") {	//if previous direction is right or down, then turn the player
					turnPlayer ("left");
					direction = "left";
				}
				setWalk (true);

			} else if (Input.GetKey ("down")) {
				turnPlayer ("down");
				direction = "down";
				setWalk (false);
			} else if (Time.timeSinceLevelLoad > 0.6f) { //this value depicts how long detective walks on screen for before stopping
				setWalk(false);
			}	
				
			Vector3 pos = this.transform.position;
			pos.z = Mathf.Clamp (this.transform.position.x, maxZ, maxZ); //clamp z so detective cant walk forward
			this.transform.position = pos;
		}
	}
}
