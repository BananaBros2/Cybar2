using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks
{

    public bool gamePaused;

    public GameObject pauseMenu;

    public GameObject mainButtons;
    public GameObject settingsMenu;
    public bool settingsUp = false;

    private void Start()
    {
        Cursor.visible = false;
    }

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
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        gamePaused = false;
        if (PhotonNetwork.OfflineMode)
            Time.timeScale = 1;
    }

    void Pause()
    {
        Cursor.visible = true;
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
        if (!settingsUp)
        {
            mainButtons.SetActive(false);
            settingsMenu.SetActive(true);
            settingsUp = true;
        }
        else
        {
            mainButtons.SetActive(true);
            settingsMenu.SetActive(false);
            settingsUp = false;
        }
    }

    public void BackHUB()
    {
        Time.timeScale = 1;
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.LoadLevel("HUB Area");
    }


    public void MainMenu()
    {
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
        
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");

        base.OnLeftRoom();
    }


    public void QuitButton()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }


}
