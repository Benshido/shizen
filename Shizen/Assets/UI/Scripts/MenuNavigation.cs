using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public GameObject menuObjects;
    public GameObject controlsObjects;
    public GameObject creditsObjects;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
        menuObjects.SetActive(false);
        controlsObjects.SetActive(true);
    }

    public void Credits()
    {
        menuObjects.SetActive(false);
        creditsObjects.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        menuObjects.SetActive(true);
        creditsObjects.SetActive(false);
        controlsObjects.SetActive(false);
    }
}
