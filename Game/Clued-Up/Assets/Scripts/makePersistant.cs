using UnityEngine;
using System.Collections;

public class makePersistant : MonoBehaviour {
	public static GameObject Instance;//Makes the object persistant throughout scenes
	//public static makePersistant Instance; //Used to make the object persistant throughout scenes
	void Awake ()  
	//Makes the object persistant throughout scenes
	{
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this.gameObject;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}
}