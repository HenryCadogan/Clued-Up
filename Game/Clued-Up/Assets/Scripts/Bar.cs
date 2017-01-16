using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Bar : MonoBehaviour {

	public Image fullPint;
	public GameObject tap;
	public float speed;
	private bool hasBeenPoured = false;
	private bool pouring = false;

	/// <summary>
	/// When Pump is clicked begin pouring
	/// </summary>
	void OnMouseDown(){
		if (!hasBeenPoured) {
			hasBeenPoured = true;
			pouring = true;
			tap.SetActive (true);
		}
	}
	/// <summary>
	/// Initialises full pint underneath pint mask 
	/// </summary>
	void Start(){
		Vector3 pos = fullPint.transform.localPosition;
		pos.y = -1200;
		fullPint.transform.localPosition = pos;
	}
	/// <summary>
	/// If beer is pouring, raise its posiion
	/// </summary>
	void Update(){
		if (pouring) {
			if (fullPint.transform.localPosition.y >= 0) {
				pouring = false;
				tap.SetActive (false);
			} else {
				Vector3 pos = fullPint.transform.localPosition;
				pos.y += speed * 10;
				fullPint.transform.localPosition = pos;
			}
		}
	}
}
