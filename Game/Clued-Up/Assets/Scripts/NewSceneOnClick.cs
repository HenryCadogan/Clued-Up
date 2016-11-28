using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewSceneOnClick : MonoBehaviour {
	public int sceneIndex;

	public void loadByIndex(int sceneIndex){
		SceneManager.LoadScene (sceneIndex);
	}

}
