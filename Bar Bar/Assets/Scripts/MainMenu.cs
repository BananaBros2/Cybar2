using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject settingsMenu;
    public bool settingsUp = false;

    public void SingleplayerButton()
    {
        PhotonNetwork.OfflineMode = true;
        SceneManager.LoadScene("HUB Area");
    }

    public void MultiplayerButton()
    {
        PhotonNetwork.OfflineMode = false;
        SceneManager.LoadScene("Loading Screen");
    }

    public void Tutorial()
    {
        PhotonNetwork.OfflineMode = true;
        SceneManager.LoadScene("1-1");

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

    public void ExitButton()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

}
