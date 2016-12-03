using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// Loads a new scene
/// </summary>
public class NewSceneOnClick : MonoBehaviour {
	/// <summary>
	/// The index of the scene to load
	/// </summary>
	public int sceneIndex;

	/// <summary>
	/// Loads the scene at the index sceneIndex
	/// </summary>
	/// <param name="sceneIndex">Scene index.</param>
	public void loadByIndex(int sceneIndex){
		SceneManager.LoadScene (sceneIndex);
	}

}
