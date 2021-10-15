using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_Behavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

        //move credits up
        transform.position += new Vector3(0, 1 * Time.deltaTime, 0);

    }
}
