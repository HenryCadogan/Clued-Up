using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Scroll credits.
/// </summary>
public class ScrollCredits : MonoBehaviour {
	/// <summary>
	/// Speed at which the text should move.
	/// </summary>
	public float speed;	
	/// <summary>
	/// The GameObject that contains the text
	/// </summary>
	public Text creditsText;
	/// <summary>
	/// The scene controller.
	/// </summary>
	public SceneTransitions sceneController;
	/// <summary>
	/// Moves credits up.
	/// </summary>
	void Update () {
		if (Input.GetKeyDown ("space") || creditsText.transform.position.y > 960) {
			sceneController.returnToMainMenu ();
		} else{
			creditsText.rectTransform.offsetMax += new Vector2 (0, speed*Time.deltaTime);
		}
	}
}
