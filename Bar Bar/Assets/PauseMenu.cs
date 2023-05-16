using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    void Resume()
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

    public void Settings()
    {

    }

    public void Lobby()
    {

    }

    public void MainMenu()
    {

    }

    public void QuitButton()
    {

    }


}
