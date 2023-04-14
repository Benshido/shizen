using UnityEngine;

public class PauseGame : MonoBehaviour
{
    bool gameIsPaused = false;
    public GameObject shizen;
    public GameObject camera;
    public GameObject menuObjects;
    public GameObject logObjects;
    public GameObject controlsObjects;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        menuObjects.SetActive(false);
        logObjects.SetActive(false);
        controlsObjects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
        camera.GetComponent<FollowCamera>().enabled = false;
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
        controlsObjects.SetActive(false);
        camera.GetComponent<FollowCamera>().enabled = true;
        shizen.SetActive(true);
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
    }

    public void OpenControlsMenu()
    {
        menuObjects.SetActive(false);
        controlsObjects.SetActive(true);
        PauseTheGame();
    }

    public void BackToPauseMenu()
    {
        menuObjects.SetActive(true);
        controlsObjects.SetActive(false);
        PauseTheGame();
    }
}
