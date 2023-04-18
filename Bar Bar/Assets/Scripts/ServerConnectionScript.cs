using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;

public class ServerConnectionScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
        LoadLobbyScene();
    }


    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    public override void OnJoinedLobby()
    {
        //hmm
    } 
}