using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Assessment 3
/// Used mainly for testing purposes. The IntroController will load the story, but it is only attached to the IntroAndInit scene.
/// So if we start the game in another scene, then this class kicks in and loads the story with some default options.
/// </summary>
public class StoryLoader : MonoBehaviour
{
    public static string DefaultDetectivePrefabPath = "Detectives/JaceVentura";

    /// <summary>
    /// Assessment 3
    /// Called when the script initiallly loads
    /// Checks to see if there is a story, if not, creates one, else does nothing
    /// </summary>
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

    /// <summary>
    /// Assessment 3
    /// Loads the story. It adds a gaamobject that controls the sotry and then adds the story script to it
    /// It spawns the default detective and then sets the detective in the story script
    /// </summary>
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
            SpawnDefaultDetective(); // (Assessment 3) This means we added a function that allows you to start the game in any of the scenes in order to do testing
            storyComponent.setDetective(2); //  This is done so that is sets up the detective as it should do normally
        }

        return storyComponent;
    }

    /// <summary>
    /// Assessment 3
    /// Spawns the default detective by prefab and then sets up the initial clue and encounter with someone so that the game doesnt break.
    /// </summary>
    private static void SpawnDefaultDetective() // Spawns Jace Ventura Detective, no option needed as this is for testing purposes and can later be used for saving and loading games
    {
        Debug.Log("Spawning default detective");
        GameObject detective = Instantiate(Resources.Load(DefaultDetectivePrefabPath)) as GameObject; // Instantiating the Jace Ventura detective as a GameObject from a prefab
        detective.name = "Detective"; // Setting the name of the detective
        detective.gameObject.SetActive(true); // Setting it active as the default setting for the perfab is disbaled

        // Give the detective the chalk outline clue because if he doesn't have it then things break when he tries to talk to someone.
        GameObject clue = Instantiate(Resources.Load("Clue")) as GameObject; // Instantiates a clue from a prefab as Gameobject
        Inventory inventory = detective.GetComponent<Inventory>(); // Gets the inventory srcipt on the Detective Gameobject
        inventory.collect("chalkOutline"); // Calls the collect function to say that the chalk outline has been collected otherwise there are errors when you try to talk to someone
        inventory.encounter (GameObject.Find ("Story").GetComponent<Story> ().getVictim ()); // Says we have encountered the victim. This is done because when starting the game this is done automatically so it has to be done here as well
    }
}
