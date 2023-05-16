using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomerSpawn : MonoBehaviour
{
    public GameObject customerObject;
    public Timer timer;

    private void Start()
    {
        PhotonNetwork.Instantiate(customerObject.name, transform.position, Quaternion.identity);
        StartCoroutine(SpawnCoroutine());
    }

    private void Update()
    {
        if (timer.gameTime >= 240)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpawnCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(9);
        while (true)
        {
            PhotonNetwork.Instantiate(customerObject.name, transform.position, Quaternion.identity);
            yield return waitTime;
        }

    }
}
