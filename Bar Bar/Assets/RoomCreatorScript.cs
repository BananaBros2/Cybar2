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

    string[] text_playerNames;
    private PhotonView PV;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("SampleScene");
        //PV = GetComponent<PhotonView>();
        //GetComponent<PhotonView>().RPC(nameof(SetLobbyPlayersData), RpcTarget.All);
        
    }

    [PunRPC]
    public void SetLobbyPlayersData()
    {
        for (int index = 0; index < PhotonNetwork.PlayerList.Length; index++)
        {
            text_playerNames[index] = "";

        }

        int i = 0;

        for (int index = 0; index < PhotonNetwork.PlayerList.Length; index++)
        {

            try
            {
                text_playerNames[index] = PhotonView.Find(i + 1).Owner.NickName;
                //playersInfo[index].SetActive(true);
            }
            catch (Exception e)
            {
                index--;
            }

            i++;

        }
    }


}
