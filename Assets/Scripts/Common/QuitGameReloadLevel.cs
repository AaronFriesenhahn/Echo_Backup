using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGameReloadLevel : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            //Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key was pressed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.GameIsPaused = false;
        }
    }
}
