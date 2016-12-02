using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	//Class that handles movement of the character

	public float minX;
	public float maxX;
	public float maxZ;
	public bool walkInDirectionIsLeft = true; //next walk in direction between scenes

	private string direction = "right"; //current direction
	private Animator anim;

	private void turnPlayer(string direction){
		//chnages to y rotation of player object depending on the direction it walks
		if (direction == "right") {
			gameObject.transform.localEulerAngles = new Vector3 (0, 90, 0); 
		} else if (direction == "left") {
			gameObject.transform.localEulerAngles = new Vector3 (0, 270, 0);
		} else {
			gameObject.transform.localEulerAngles = new Vector3 (0, 180, 0);
		}
	}

	public void walkIn(){
		//Player walks in from a direction called by SceneController scripts
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






	// Update is called once per frame
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
