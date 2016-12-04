using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// Loads a new scene IS THIS NEEDED? IF YOU ARE READING THIS, PLEASE CHECK WETHER THIS IS IN USE AND/OR WETHER IT CAN BE MERGED WITH ANOTHER SCRIPT
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
