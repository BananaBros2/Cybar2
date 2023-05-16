using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool gamePaused;

    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        gamePaused = false;
        if (PhotonNetwork.OfflineMode)
            Time.timeScale = 1;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        gamePaused = true;
        if (PhotonNetwork.OfflineMode)
            Time.timeScale = 0;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        string currentSceneName = SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel(currentSceneName);
    }

    public void Settings()
    {

    }

    public void BackHUB()
    {
        Time.timeScale = 1;
        PhotonNetwork.LoadLevel("HUB Area");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");
        
    }

    public void QuitButton()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }


}
