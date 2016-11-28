using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	//Class that handles movement of the character
		
	public static PlayerController Instance; //Used to make the object persistant throughout scenes
	public float minX;
	public float maxX;
	public float maxZ;

	private string direction = "right";
	private Animator anim;


	void Awake ()  
	//Makes the object persistant throughout scenes
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}

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

	void Start () {
		anim = gameObject.GetComponentInChildren<Animator> (); // Finds the animator that controls this object
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

		if (Time.timeSinceLevelLoad > 1){	//if enough time has passed to allow detective to walk on then clamp to certain range
			Vector3 pos = this.transform.position;
			pos.x = Mathf.Clamp (this.transform.position.x, minX, maxX);
			pos.z = Mathf.Clamp (this.transform.position.x, maxZ, maxZ); //leeway on z position. but cannot walk forward
			this.transform.position = pos;
		}




	}
}
