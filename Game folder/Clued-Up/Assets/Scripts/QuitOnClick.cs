using UnityEngine;
using System.Collections;

public class QuitOnClick : MonoBehaviour {

	public void quit(){
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.quit();
#endif
	}
}
