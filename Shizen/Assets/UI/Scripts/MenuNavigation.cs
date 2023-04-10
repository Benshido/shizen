using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
        Debug.Log("Controls");
    }

    public void Credits()
    {
        Debug.Log("Credits");
    }

    public void ExitToMainMenu()
    {
        Debug.Log("Mainmenu");
    }

    public void ExitGame()
    {

    }
}
