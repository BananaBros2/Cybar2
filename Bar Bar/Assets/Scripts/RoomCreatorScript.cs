using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System;
using Photon.Pun;

public class RoomCreatorScript : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField JoinInput;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        if (createInput.text == "")
            return;
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        if (JoinInput.text == "")
            return;
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
