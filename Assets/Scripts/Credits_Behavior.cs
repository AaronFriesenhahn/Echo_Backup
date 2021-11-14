using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_Behavior : MonoBehaviour
{
    [SerializeField] GameObject _Echo;

    // Start is called before the first frame update
    void Start()
    {
        _Echo.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key was pressed");
            SceneManager.LoadScene(0);
            GameManager.GameIsPaused = false;
        }

        //move credits up till y is 28(where the credits stop at Echo and instructions to quit or return to main menu)
        if(transform.position.y < 28)
        {
            transform.position += new Vector3(0, 1 * Time.deltaTime, 0);
        }
        if (transform.position.y > 26)
        {
            _Echo.SetActive(true);
        }
    }
}
