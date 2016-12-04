using UnityEngine;
using System.Collections;
/// <summary>
/// Quit the game on click.
/// </summary>
public class QuitOnClick : MonoBehaviour {
	/// <summary>
	/// Returns to Unity editor if running within, otherwise the game's application is quit.
	/// </summary>
	public void quit(){
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
