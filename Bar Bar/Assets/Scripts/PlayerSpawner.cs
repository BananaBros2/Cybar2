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

    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        player.gameObject.name = ("Player " + PhotonNetwork.LocalPlayer.ActorNumber);
    }

}
