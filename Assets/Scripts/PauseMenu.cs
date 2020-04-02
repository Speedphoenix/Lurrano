using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GameIsPause = false;
    public GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        Debug.Log("Resumefonction");
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;

    }
    void Pause()
    {
        Debug.Log("Pausefonction");
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;

    }

    public void LoadMenu()
    {
        Debug.Log("Load menu...");
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit game ...");
        Application.Quit();
    }
}
