using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MasterButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            transform.gameObject.SetActive(false);
        }
    }

}
