using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A class that creates the end game screen and exits it.
/// </summary>
public class GameOver : MonoBehaviour
{
    /// <summary>
    /// The text object in the Game Over scene.
    /// </summary>
    public GameObject textPanel;
    /// <summary>
    /// The AudioSource component in the Game Over scene.
    /// </summary>
    public AudioSource audioSource;
    /// <summary>
    /// The scene controller.
    /// </summary>
    public SceneTransitions sceneController;
    /// <summary>
    /// The story used in the game.
    /// </summary>
    private Story story;
    /// <summary>
    /// Provides an easy to use handler to fade the text.
    /// </summary>
    /// <param name="textObject">The text object to be faded.</param>
    /// <param name="alpha">The target alpha value for the object.</param>
    /// <param name="time">The amount of time over which the crossfade should elapse.</param>
    private void fadeText(GameObject textObject, float alpha, float time)
    {
        textObject.GetComponent<Text>().CrossFadeAlpha(alpha, time, false);
    }
    /// <summary>
    /// Runs the end game cutscene.
    /// </summary>
    /// <returns>While the Enumerator may return a value, I would not trust it to be usable.</returns>
    IEnumerator LeaderBoard()
    {
        List<int> ScoreList = GetScores();
        yield return new WaitForSeconds(1f); //wait 1 second after scream begins
        textPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "1." = ScoreList[0];
        CreateStars(ScoreList[0], textPanel.transform.GetChild(0).GetChild(1));
        fadeText(textPanel.transform.GetChild(0).gameObject, 1f, 2f);

        yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
        textPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "2." = ScoreList[1];
        CreateStars(ScoreList[0], textPanel.transform.GetChild(1).GetChild(1));
        fadeText(textPanel.transform.GetChild(1).gameObject, 1f, 2f);

        yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
        textPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "3." = ScoreList[2];
        CreateStars(ScoreList[0], textPanel.transform.GetChild(2).GetChild(1));
        fadeText(textPanel.transform.GetChild(2).gameObject, 1f, 2f);

        yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
        textPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "4." = ScoreList[3];
        CreateStars(ScoreList[0], textPanel.transform.GetChild(3).GetChild(1));
        fadeText(textPanel.transform.GetChild(3).gameObject, 1f, 2f);

        yield return new WaitForSeconds(3f); //wait 1 second after last fade ends
        textPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "5." = ScoreList[4];
        CreateStars(ScoreList[0], textPanel.transform.GetChild(4).GetChild(1));
        fadeText(textPanel.transform.GetChild(4).gameObject, 1f, 2f);

        yield return new WaitForSeconds(3f); // wait three secs for fade, and one second after the fade ends
        fadeOutAllText(0f, 2f);

        yield return new WaitForSeconds(3f); // wait three secs for fade, and one second after the fade ends

        SceneManager.LoadScene(12); //load credits
    }

    public List<int> GetScores()
    { 
        list<int> IntList = new list<int>();
        string str = PlayerPrefs.GetString("ScoreString");
        string[] strArray = str.Split("£");
        foreach (string s in strArray)
        {
            IntList.Add(int.Parse(s));
        }
        IntList.Sort();
    }

    /// <summary>
    /// Get the star rating from your score
    /// </summary>
    private int RateScore(int i = Story.Instance.Score)
    {
        //int i = Story.Instance.Score;
        if (PlayerPrefs.HasKey("ScoreString"))
        {
            string str = PlayerPrefs.GetString("ScoreString");
            str = string.Concat(str, "£", i.ToString());
            PlayerPrefs.SetString("ScoreString", str);
        }
        else
        {
            PlayerPrefs.SetString("ScoreString", i.ToString());
        }

        if (i < 10)
        {
            return 1;
        }
        if (10 <= i && i < 25)
        {
            return 2;
        }
        if (25 <= i && i < 40)
        {
            return 3;
        }
        if (40 <= i && i < 55)
        {
            return 4;
        }
        if (i > 55)
        {
            return 5;
        }
        return 0;
    }

    /// <summary>
    /// Instantiates the stars
    /// </summary>
    private void CreateStars(int j = Story.Instance.Score, Transform t)
    {
        var g = Resources.Load("Star");
        int i = RateScore(j);
        for (int j = 0; j < i; j++)
        {
            GameObject star = Instantiate(g, t.GetChild(1), false) as GameObject;
        }
    }

    /// <summary>
    /// Simultaneously fades out all text boxes.
    /// </summary>
    private void fadeOutAllText(float alpha, float time)
    {
        fadeText(textPanel.transform.GetChild(0).gameObject, alpha, time);
        fadeText(textPanel.transform.GetChild(1).gameObject, alpha, time);
        fadeText(textPanel.transform.GetChild(2).gameObject, alpha, time);
        fadeText(textPanel.transform.GetChild(3).gameObject, alpha, time);
        fadeText(textPanel.transform.GetChild(4).gameObject, alpha, time);
    }

    // Use this for initialization
    void Start()
    {
        Story story = FindObjectOfType<Story>();
        fadeOutAllText(0f, 0f); //instantaneously fades out all text boxes
        StartCoroutine(LeaderBoard(story));
    }
}
