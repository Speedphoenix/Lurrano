using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] private Text countText;
    [SerializeField] private int scoreIncrement = 1;
    [SerializeField] private int startScore = 0;
    private int scoreCount = 0;
    [SerializeField] private string scoreText = "Score:";

    public static int Score
    {
        get { return instance.scoreCount; }
    }

    void Start()
    {
        scoreCount = startScore;
        countText.text = scoreText + " " + scoreCount.ToString();
    }

    public void addToScore(int toAdd)
    {
        scoreCount += toAdd;
        countText.text = scoreText + " " + scoreCount.ToString();
    }

    public void incrementScore()
    {
        addToScore(scoreIncrement);
    }

    void OnEnable()
    {
        if (instance != null)
            Application.Quit(); // replace this with a proper throw statement
        else
            instance = this;
    }

    void OnDisable()
    {
        if (instance != null)
            instance = null;
    }

    void OnApplicationQuit()
    {
        instance = null;
    }
}
