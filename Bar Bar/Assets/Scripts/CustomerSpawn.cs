using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class CustomerSpawn : MonoBehaviour
{
    public GameObject customerObject;
    public Timer timer;

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(customerObject.name, transform.position, Quaternion.identity);
            StartCoroutine(SpawnCoroutine());
        }

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
        WaitForSeconds waitTime = new WaitForSeconds(9 - PhotonNetwork.CurrentRoom.PlayerCount);
        while (true)
        {
            GameObject customer = PhotonNetwork.Instantiate(customerObject.name, transform.position, Quaternion.identity);
            customer.GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(40, 1000);
            yield return waitTime;
        }

    }
}
