using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    Scene currentScene;

    //Make only one possible instance of GameManager
    public static GameManager instance;

    public static bool GameIsPaused = false;

    [SerializeField] GameObject _player;

    bool clearedData = false;

    [SerializeField] GameObject pauseMenuUI = null;
    [SerializeField] GameObject mainMenuUI = null;
    public CanvasGroup _MainMenuUIGroup = null;
    [SerializeField] GameObject gameHUD = null;
    [SerializeField] GameObject optionsMenuUI = null;

    [SerializeField] GameObject Checkpoint;

    public Text scoreText = null;
    int score;

    int sceneIndex;
    int currentSceneIndex;
    static int previousSceneIndex;

    public bool mainMenuFadedOut = false;
    public bool OptionsMenuBool = false;

    //for testing respawn
    int x = 0;

    private void Awake()
    {
        #region Singleton Code
        //if no GameManager has been assigned, assign this one as the GameManager
        //if (instance == null)
        //{
        //    //Don't destroy the GameManager
        //    DontDestroyOnLoad(gameObject);
        //    //Create object as GameManager Singleton
        //    instance = this;
        //}
        //else if (instance != this)
        //{
        //    //Destroys any new objects since we have our GameManager
        //    Destroy(gameObject);
        //}
        #endregion

        pauseMenuUI = GameObject.Find("PauseMenuCanvas");
        mainMenuUI = GameObject.Find("StartMenuCanvas");
        _MainMenuUIGroup = mainMenuUI.GetComponent<CanvasGroup>();
        gameHUD = GameObject.Find("GameHUD");
        optionsMenuUI = GameObject.Find("OptionsMenuCanvas");

        UpdateUI();
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    public void Start()
    {
        //lock cursor in menu scenes
        //Debug.Log("Confining cursor.");
        Cursor.lockState = CursorLockMode.Confined;
        Pause();
    }

    public void Update()
    {
        //check for button press and save value to be loaded
        if (Input.GetKeyDown(KeyCode.Escape) && mainMenuFadedOut == true)
        {
            RetrieveSceneIndex();
            //if game is paused
            if (GameIsPaused)
            {
                //Resume();
            }
            else
            {
                Debug.Log("Current Scene Index is: " + currentSceneIndex);
                //freeze game
                //pause menu pops up
                Pause();
            }
        }
        PauseMenuState();
        UpdateUI();

        RespawnPlayer();

        //Debug.Log(GameIsPaused);
    }

    public void RespawnPlayer()
    {
        if (_player.activeSelf == false)
        {
            Debug.Log("activating player.");
            StartCoroutine(RespawnDelay());

        }
    }

    IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(1f);
        //_player.transform.position = new Vector3(0, 1, 0);
        //replace new Vector3 with Checkpoint position
        //_player.transform.position = Checkpoint.transform.position;

        //or reload current scene
        RetrieveSceneIndex();
        SceneManager.LoadScene(sceneIndex);

        _player.SetActive(true);
    }


    public void LoadLevel(string levelName)
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName);
        SceneChanged();
        UpdateUI();
    }

    public void Pause()
    {
        GameIsPaused = true;
        //timeScale slows down or speeds up game, in this case, it stops the game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        optionsMenuUI.SetActive(false);
        OptionsMenuBool = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetCursorState()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ToggleOptionsMenu()
    {
        if (OptionsMenuBool == false)
        {
            optionsMenuUI.SetActive(true);
            OptionsMenuBool = true;
        }
        else if (OptionsMenuBool == true)
        {
            optionsMenuUI.SetActive(false);
            OptionsMenuBool = false;
        }
    }

    public void ResetGame()
    {
        Debug.Log("Quitting to Main Title.");
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        currentScene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene name is: " + currentScene.name + "\nActive Scene index: " + currentScene.buildIndex);

        int sceneIndex = currentScene.buildIndex;
        int nextSceneIndex = currentScene.buildIndex + 1;

        SceneManager.LoadScene(nextSceneIndex);
    }

    //updates gameHUD (healthbar)
    public void UpdateUI()
    {
        if (mainMenuFadedOut == false)
        {
            gameHUD.SetActive(false);            
        }
        else if (mainMenuFadedOut == true)
        {
            gameHUD.SetActive(true);
        }
    }

    IEnumerator WaitBeforeRespawning()
    {
        yield return new WaitForSeconds(1f);
    }

    public void RetrieveSceneIndex()
    {
        //get current scene index
        currentScene = SceneManager.GetActiveScene();
        sceneIndex = currentScene.buildIndex;
        Debug.Log("Current Scene Index: " + sceneIndex);
    }

    //activates whenever a scene is changed
    public void SceneChanged()
    {
        Debug.Log("Scene has changed.");
        RetrieveSceneIndex();
        if (sceneIndex == 0 || sceneIndex > 1)
        {
            Debug.Log("Checking for High Score and Score UI");
        }
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        UpdateUI();
    }

    public void PauseMenuState()
    {
        if (GameIsPaused == true && mainMenuFadedOut == true)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void FadeIn()
    {
        //Debug.Log("Fade In Called.");
        StartCoroutine(FadeCanvasGroup(_MainMenuUIGroup, _MainMenuUIGroup.alpha, 1));
        mainMenuFadedOut = false;
    }

    public void FadeOut()
    {
        //Debug.Log("Fade Out Called.");
        StartCoroutine(FadeCanvasGroup(_MainMenuUIGroup, _MainMenuUIGroup.alpha, 0));
        mainMenuFadedOut = true;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float _timeStartedLerping = Time.time;
        float _timeSinceStarted = Time.time - _timeStartedLerping;
        float _percentageComplete = _timeSinceStarted / lerpTime;

        while (true)
        {
            _timeSinceStarted = Time.time - _timeStartedLerping;
            _percentageComplete = _timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, _percentageComplete);

            cg.alpha = currentValue;
            //Debug.Log(currentValue);

            if (_percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
        //pauses game at main menu and sets buttons to interactable
        if (cg.alpha == 1)
        {
            Pause();
            cg.interactable = true;
        }
        //makes main menu buttons not interactable
        if(cg.alpha == 0)
        {
            cg.interactable = false;
        }
    }
}
