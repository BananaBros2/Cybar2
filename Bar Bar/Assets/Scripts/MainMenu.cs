using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class MainMenu : MonoBehaviour
{


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

}
