using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // gets the curent screen
        //Scene sceneLoaded = SceneManager.GetActiveScene();
        // loads next level
        SceneManager.LoadScene(2);
    }
}
