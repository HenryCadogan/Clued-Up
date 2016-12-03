using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Fades the overlay panel
/// </summary>
public class OverlayPanelFader : MonoBehaviour {
	/// <summary>
	/// The time to fade in
	/// </summary>
	public float time;
	/// <summary>
	/// The fade in.
	/// </summary>
	public bool fadeIn;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		float alpha;
		if (fadeIn){
			alpha = 0f;
		}else {
			alpha = 1f;
		}

		this.GetComponent<Image> ().CrossFadeAlpha (alpha, time, false); //referes to panel the script is attatched to
	}

}
