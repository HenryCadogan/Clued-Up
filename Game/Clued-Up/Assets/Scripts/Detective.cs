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
	/// //The next walk in direction to be used when switching between scenes
	/// </summary>
	public bool walkInDirectionIsLeft = true; 

	/// <summary>
	/// The current direction the detective is facing.
	/// </summary>
	private string direction = "right";
	/// <summary>
	/// The animator GameObject used by the detective.
	/// </summary>
	private Animator anim;


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
		anim = this.gameObject.GetComponentInChildren<Animator> (); // Initial setup as this is the first called funtion

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

		anim.SetBool ("walking", true);
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
				anim.SetBool ("walking", true);

			} else if (Input.GetKey ("left")) {
				if (direction != "left") {	//if previous direction is right or down, then turn the player
					turnPlayer ("left");
					direction = "left";
				}
				anim.SetBool ("walking", true);

			} else if (Input.GetKey ("down")) {
				turnPlayer ("down");
				direction = "down";
				anim.SetBool ("walking", false);
			} else if (Time.timeSinceLevelLoad > 0.6f) { //this value depicts how long detective walks on screen for before stopping
				anim.SetBool ("walking", false);
			}	
				
			Vector3 pos = this.transform.position;
			pos.z = Mathf.Clamp (this.transform.position.x, maxZ, maxZ); //clamp z so detective cant walk forward
			this.transform.position = pos;
		}
	}
}
