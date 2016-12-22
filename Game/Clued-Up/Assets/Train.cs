using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {
	/// <summary>
	/// How fast the train will move.
	/// </summary>
	public float speed;
	/// <summary>
	/// The chance of a train appearing.
	/// </summary>
	public float chance;
	/// <summary>
	/// The overlay sign that can be turned off if not needed.
	/// </summary>
	public GameObject overlaySign;
	/// <summary>
	/// The overlay sign that can be turned off if not needed.
	/// </summary>
	public GameObject horn;

	/// <summary>
	/// Decides wether or not to allow train to continue
	/// </summary>
	void Start () {
		int chanceI = (int)(chance * 100);
		int randI = Random.Range (0, 100);
		this.horn.GetComponent<AudioSource> ().Play ();
		Debug.Log (randI);
		if (chanceI - 2 < randI) {
			overlaySign.SetActive (false);
			this.gameObject.SetActive (false);
			this.horn.SetActive (false);
		}
	}

	/// <summary>
	/// Progresses train & removes when not seen.
	/// </summary>
	void Update () {
		if (Time.timeSinceLevelLoad > 4) {
			Vector3 pos = this.transform.position;
			pos.x -= speed;
			this.transform.position = pos;
		}
	}
}
