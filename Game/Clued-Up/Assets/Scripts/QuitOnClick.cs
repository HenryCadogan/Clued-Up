using UnityEngine;
using System.Collections;
/// <summary>
/// Quit the game on click.
/// </summary>
public class QuitOnClick : MonoBehaviour {
	/// <summary>
	/// Sets playing to false
	/// </summary>
	public void quit(){
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
