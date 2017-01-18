using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AtriumMoveScript : MonoBehaviour {

	GameObject mPanel;
	// Use this for initialization
	void Start () {
		mPanel = GameObject.Find ("MapPanel");
	}

	void OnMouseDown(){
		
		print ("Collider pressed, moving to atrium");
		//fade out, hide the menu and then load the scene
		//mPanel.SetActive(false);
		//Nasty hard coded value here but simply look in the build settings for 
		//SceneManager.LoadScene(4);
	}
}
