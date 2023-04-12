using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    bool gameIsPaused = false;
    public GameObject shizen;
    public GameObject camera;
    public GameObject menuObjects;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        menuObjects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                PauseTheGame();
            }
            else
            {
                UnpauseTheGame();
            }
        }
    }

    public void PauseTheGame()
    {
        menuObjects.SetActive(true);
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
        camera.SetActive(true);
        shizen.SetActive(true);
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
    }
}
