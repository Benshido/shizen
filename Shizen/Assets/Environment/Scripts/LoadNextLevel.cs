using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    int currentSceneBuildIndex;

    private void OnTriggerEnter(Collider other)
    {
        // gets the curent screen
        //Scene sceneLoaded = SceneManager.GetActiveScene();
        // loads next level
        if (other.gameObject.tag == "Player")
        {
            currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneBuildIndex + 1);
        }
    }
}
