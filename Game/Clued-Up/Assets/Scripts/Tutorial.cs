using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
	public GameObject tutorials;
	public SceneTransitions sceneTransitions;

	private int currentTutorial = 0;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			if (currentTutorial < 4) {
				currentTutorial++;
				tutorials.transform.GetChild (currentTutorial).gameObject.SetActive (true);
			} else {
				sceneTransitions.startSceneTransition (0);
			}
		}
		
	}
}
