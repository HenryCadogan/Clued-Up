using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Animator anim;
	private string direction = "right";



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

	IEnumerator TimedWalkIn(){
		yield return new WaitForSeconds (2);
		anim.SetBool ("walking", false);
	}

	void Start () {
		anim = gameObject.GetComponentInChildren<Animator> (); // Finds the animator that controls this object
		StartCoroutine("TimedWalkIn"); //as the character walks in, time the enterence and stop it walking
	}

	// Update is called once per frame
	void Update () {
		// Walking annimations triggered if keys pressed
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
		} else {
			anim.SetBool ("walking", false);
		}	
	}
}
