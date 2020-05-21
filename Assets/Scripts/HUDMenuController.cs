﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HUDMenuController : MonoBehaviour
{
    static HUDMenuController instance = null;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject endGameMenu = null;
    private int fullObjectiveCount = 0;
    private int finalObjectiveCount = 0;
    private int currentFinalCaught = 0;

    [SerializeField] private Text endFinalScoreText = null;
    [SerializeField] private Text endTimeText = null;

    public static void addObjective(bool isFinal = false)
    {
        instance.fullObjectiveCount++;
        if (isFinal)
            instance.finalObjectiveCount++;
    }

    public static void caughtNewFinal()
    {
        instance.currentFinalCaught++;
        if (instance.currentFinalCaught == instance.finalObjectiveCount)
            instance.triggerEndOfGame();
    }

    public void triggerEndOfGame()
    {
        ColorController.instance.IsPaused = true;
        endGameMenu.SetActive(true);

        endFinalScoreText.text = "You collected " + ScoreManager.Score.ToString() + " out of " + fullObjectiveCount.ToString() + " Christmas trees!";

        float gameTime = ColorController.instance.GameTimer;
        int seconds = (int) (gameTime % 60);
        int minutes = (int) ((gameTime / 60f) % 60);
        int hours = (int) (gameTime / 3600);

        string timeText = "Time: ";
        if (hours > 0)
            timeText += "" + hours.ToString() + ":";
        if (minutes > 0 || hours > 0)
            timeText += "" + minutes.ToString() + ":";
        timeText += "" + seconds.ToString();

        endTimeText.text = timeText;
    }

    public void toMainMenu()
    {
        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }

    public void resumeGame()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        if (endGameMenu != null)
            endGameMenu.SetActive(false);

        ColorController.instance.IsPaused = false;
    }

    public void restartGame()
    {
        // NOT async because we want to make sure every script is properly disabled.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        //to make sure the menus are closed
        resumeGame();
    }
    
    void OnEnable()
    {
        if (instance != null && instance != this)
        {
            // Application.Quit(); // replace this with a proper throw statement
        }
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