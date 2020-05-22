using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HUDMenuController : MonoBehaviour
{
    static HUDMenuController instance = null;

    public static HUDMenuController Instance
    {
        get { return instance; }
    }

    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject endGameMenu = null;
    [SerializeField] private GameObject loadingText = null;
    private int fullObjectiveCount = 0;
    private int finalObjectiveCount = 0;
    private int currentCaught = 0;
    private int currentFinalCaught = 0;

    [SerializeField] private Text endFinalScoreText = null;
    [SerializeField] private Text[] timeTexts = null;

    [SerializeField] private Text[] highScoreAnyTexts = null;
    [SerializeField] private Text[] highScoreTexts = null;

    private string prefKeyAny = "highscoreany";
    private string prefKeyfull = "highscorefull";
    

    public static void addObjective(bool isFinal = false)
    {
        instance.fullObjectiveCount++;
        if (isFinal)
            instance.finalObjectiveCount++;
    }

    public void collectedObjective(bool isFinal)
    {
        currentCaught++;
        if (isFinal)
            currentFinalCaught++;
        
        if (currentCaught == fullObjectiveCount || currentFinalCaught == finalObjectiveCount)
            triggerEndOfGame();
    }

    public string toTimeText(float gameTime)
    {
        int seconds = (int) (gameTime % 60);
        int minutes = (int) ((gameTime / 60f) % 60);
        int hours = (int) (gameTime / 3600);

        string timeText = "";
        if (hours > 0)
            timeText += "" + hours.ToString() + "h ";
        if (minutes > 0 || hours > 0)
            timeText += "" + minutes.ToString() + "m ";
        timeText += "" + seconds.ToString() + "s";

        return timeText;
    }

    public void setTimeText()
    {
        float gameTime = ColorController.instance.GameTimer;

        string timeText = "Time: " + toTimeText(gameTime);

        foreach (Text el in timeTexts)
            el.text = timeText;
    }

    public void useHighScores(Text[] displays, string prefKey, bool savenew = false)
    {
        float gameTime = ColorController.instance.GameTimer;

        // if we don't want to save it we'll just ignore it
        bool addedAlready = !savenew;

        List<float> highScores = new List<float>();

        int i = 0;
        string key = prefKey + i.ToString();
        while (PlayerPrefs.HasKey(key))
        {
            // so that the list is ordered right off the bat
            float inter = PlayerPrefs.GetFloat(key);
            if (!addedAlready && gameTime <= inter)
            {
                highScores.Add(gameTime);
                addedAlready = true;
            }
            highScores.Add(inter);
            i++;
            key = prefKey + i.ToString();
        }
        if (!addedAlready)
            highScores.Add(gameTime);

        // we now have a list of highscores

        for (i = 0; i < highScores.Count; i++)
        {
            if (i >= displays.Length)
                break;
            // display 'em
            displays[i].text = toTimeText(highScores[i]);

            if (savenew)
            {
                // save 'em
                key = prefKey + i.ToString();
                PlayerPrefs.SetFloat(key, highScores[i]);
            }
        }

        #if !UNITY_STANDALONE && !UNITY_EDITOR
            if (savenew)
                PlayerPrefs.Save();
            // Save() is called during OnApplicationQuit(), but OnApplicationQuit() doesn't happen with WebGL
        #endif
    }

    public void resetScores()
    {
        resetScoreList(prefKeyAny);
        resetScoreList(prefKeyfull);

        foreach (Text item in highScoreAnyTexts)
        {
            item.text = "--";
        }

        foreach (Text item in highScoreTexts)
        {
            item.text = "--";
        }
    }

    public void resetScoreList(string prefKey)
    {
        int i = 0;
        string key = prefKey + i.ToString();
        while (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            i++;
            key = prefKey + i.ToString();
        }
    }

    public void triggerEndOfGame()
    {
        ColorController.instance.IsPaused = true;
        endGameMenu.SetActive(true);

        endFinalScoreText.text = "You collected " + ScoreManager.Score.ToString() + " out of " + fullObjectiveCount.ToString() + " Christmas trees!";

        useHighScores(highScoreAnyTexts, prefKeyAny, true);
        useHighScores(highScoreTexts, prefKeyfull, currentCaught >= fullObjectiveCount);

        setTimeText();
    }

    public void triggerPauseMenu()
    {
        ColorController.instance.IsPaused = true;
        pauseMenu.SetActive(true);

        setTimeText();
    }

    public void toMainMenu()
    {
        loadingText.SetActive(true);
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
        loadingText.SetActive(true);

        // not async because we want to make sure every script is properly disabled.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        //to make sure the menus are closed
        resumeGame();
    }

    void Update()
    {
        if (GameInputManager.getKeyDown(GameInputManager.InputType.Pause))
        {
            triggerPauseMenu();
        }
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
