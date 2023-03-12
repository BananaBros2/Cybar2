using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;

    private PhotonView PV;

    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        var photonView = GetComponent<PhotonView>();
        player.gameObject.name = ("Player " + PhotonNetwork.LocalPlayer.ActorNumber);
        print("Player " + photonView.ControllerActorNr);
        PV = GetComponent<PhotonView>();
        PV.RPC(nameof(RPC_ColorChange), RpcTarget.All);

    }

    [PunRPC]
    void RPC_PlayerName()
    {

        player = GameObject.FindGameObjectWithTag("NewPlayer");
        player.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 0, 0);
        player.tag = "Player";
    }

    [PunRPC]
    void RPC_ColorChange()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 0, 0);
    }

}
