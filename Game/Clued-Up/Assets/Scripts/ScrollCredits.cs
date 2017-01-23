using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollCredits : MonoBehaviour {
	public float speed;	
	public Text creditsText;
	public SceneTransitions sceneController;
	/// <summary>
	/// Moves credits up.
	/// </summary>
	void Update () {
		if (Input.GetKeyDown ("space") || creditsText.transform.position.y > 960) {
			sceneController.returnToMainMenu ();
		} else{
			creditsText.rectTransform.offsetMax += new Vector2 (0, speed);
		}
	}
}
