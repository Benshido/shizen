using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    bool gameIsPaused = false;
    public GameObject shizen;
    public GameObject camera;
    public GameObject menuObjects;
    public GameObject logObjects;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        menuObjects.SetActive(false);
        logObjects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                menuObjects.SetActive(true);
                PauseTheGame();
            }
            else UnpauseTheGame();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!gameIsPaused)
            {
                logObjects.SetActive(true);
                PauseTheGame();
            }
            else UnpauseTheGame();
        }
    }

    public void PauseTheGame()
    {
        camera.SetActive(false);
        shizen.SetActive(false);
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
    }

    public void UnpauseTheGame()
    {
        menuObjects.SetActive(false);
        logObjects.SetActive(false);
        camera.SetActive(true);
        shizen.SetActive(true);
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
    }
}
