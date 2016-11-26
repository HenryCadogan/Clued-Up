using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OverlayPanelFader : MonoBehaviour {
	public float time;
	public bool fadeIn;
	// Use this for initialization
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
