using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    public float minX = 2;
    public float maxX = 2;
    public float minY = 2;
    public float maxY = 2;
    public float minZ = 2;
    public float maxZ = 2;

    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        if (PhotonNetwork.OfflineMode == false)
        {
            player = PhotonNetwork.Instantiate("Player", randomPosition, Quaternion.identity);
        }
        else
        {
            player = Instantiate(playerPrefab, randomPosition, Quaternion.identity);
        }
        //player.gameObject.name = ("Player " + PhotonNetwork.LocalPlayer.ActorNumber);
    }
}
