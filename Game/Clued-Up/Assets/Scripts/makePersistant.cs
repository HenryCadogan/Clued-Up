using UnityEngine;
using System.Collections;
/// <summary>
/// Make persistant.
/// </summary>
public class makePersistant : MonoBehaviour {
	/// <summary>
	/// Makes the object persistant throughout scenes
	/// </summary>
	public static GameObject Instance;
	/// <summary>
	/// Awake this instance.
	/// </summary>
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