using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trash : MonoBehaviour
{
    public PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item" || other.tag == "Container")
        {
            int newIndex = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().checkedItems.IndexOf(other.transform);

            if (PhotonNetwork.IsMasterClient)
            {
                view.RPC("RPC_UpdateLists", RpcTarget.All, newIndex);
            }
        }
    }

    [PunRPC]
    void RPC_UpdateLists(int RPCindex)
    {
        Transform objectToDelete = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().checkedItems[RPCindex];

        // Runs the following code per every object tagged as a player
        foreach (GameObject players in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.GetComponent<PlayerController>().checkedItems.Remove( players.GetComponent<PlayerController>().checkedItems[RPCindex]);
            players.GetComponent<PlayerController>().worldItems = players.GetComponent<PlayerController>().checkedItems.ToArray();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(objectToDelete.gameObject);
        }
    }

}