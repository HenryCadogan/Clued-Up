using UnityEngine;
using System.Collections;
/// <summary>
/// Make persistant.
/// </summary>
public class makePersistant : MonoBehaviour {
	/// <summary>
	/// Copy of this object
	/// </summary>
	public static GameObject Instance;
	/// <summary>
	/// Makes the GameObject persistant through all scenes while also only allowing one single instance of the class
	/// </summary>
	void Awake ()  
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