using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Used mainly for testing purposes. The IntroController will load the story, but it is only attached to the IntroAndInit scene.
/// So if we start the game in another scene, then this class kicks in and loads the story with some default options.
/// </summary>
public class StoryLoader : MonoBehaviour
{
    public static string DefaultDetectivePrefabPath = "Detectives/JaceVentura";

    // Use this for initialization
    void Start()
    {
        Debug.Log("StoryLoader#start called");
        if (!Story.isLoaded())
        {
            Debug.Log("StoryLoader#start: story is not loaded so loading it");
            loadStory();
        }
        else
        {
            Debug.Log("StoryLoader#start: story is already loaded.");
        }
    }

    public static Story loadStory()
    {
        Debug.Log("StoryLoader#loadStory called");
        if (Story.isLoaded())
        {
            throw new System.Exception("Trying to load story when it has already been loaded.");
        }

        // Create a game object to hold the story and add a Story component to it
        GameObject storyObject = new GameObject("Story");
        storyObject.AddComponent<Story>();
        Story storyComponent = storyObject.GetComponent<Story>();

        // Set the weather and the story
        storyComponent.setStory();

        // Spawn a detective if we're not in the CharacterSelection / MainMenu / Intro scenes
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu" || currentScene.name == "IntroAndInit" || currentScene.name == "CharacterSelection")
        {
            Debug.Log("StoryLoader#loadStory not making a detective because we are in an initial scene.");
        }
        else
        {
            SpawnDefaultDetective();
            storyComponent.setDetective(2);
        }

        return storyComponent;
    }

    private static void SpawnDefaultDetective()
    {
        Debug.Log("Spawning default detective");
        GameObject detective = Instantiate(Resources.Load(DefaultDetectivePrefabPath)) as GameObject;
        //detective.AddComponent<Detective>();
        detective.name = "Detective";
        detective.gameObject.SetActive(true);


        // Give the detective the chalk outline clue because if he doesn't have it then things break when he tries to talk to someone.
        GameObject clue = Instantiate(Resources.Load("Clue")) as GameObject;
        Inventory inventory = detective.GetComponent<Inventory>();
        inventory.collect("chalkOutline");
        inventory.encounter (GameObject.Find ("Story").GetComponent<Story> ().getVictim ());
        //detective.transform.position = new Vector3(0.0f, -5.9f, -2f);
    }
}
