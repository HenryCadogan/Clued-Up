using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour {
	//handles what happens when player walks into a transparent collider at the edge of each side of the scene
	public GameObject overlayPanel;



	IEnumerator fadeLoadScene(int scene){
		//Fades overlay panel to black and loads new scene
		overlayPanel.SetActive (true);
		overlayPanel.GetComponent<Image> ().CrossFadeAlpha (1f, 1f, false);
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene (scene);	//loads lobby scene
	}
		


	void OnTriggerEnter(Collider detective){

		if (Time.timeSinceLevelLoad > 0.2){	//enough time for them to walk on screen
			switch (this.gameObject.name) {
			case "Room1L":
				//will force location of detective to not go past collider
				Vector3 pos = detective.transform.position;
				pos.x = -7f;
				detective.transform.position = pos;
				break;
			case "Room1R":
				bool canProgress = true; //set conditions for progressing to room 2
				if (canProgress){
					StartCoroutine(fadeLoadScene(4));
				}
				break;
			default:
				break;
			}

	}
}

}