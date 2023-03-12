using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[DefaultExecutionOrder(1)]
public class Box : MonoBehaviour
{
    private Transform player;

    public float pickupDistance;
    public float forceMulti;


    public bool readyToThrow;
    public bool itemIsPicked;

    private PhotonView PV;
    PhotonView view;

    public float throwForce = 1000f;
    private Rigidbody rb;

    public GameObject resource;
    GameObject newObject;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        //player = GameObject.Find("Player").transform;
        //PickUpPoint = GameObject.Find("PickUpPoint").transform;

        view = GetComponent<PhotonView>();


    }

    // Update is called once per frame
    void Update()
    {
            //if (!view.IsMine)
            //return;

        player = GameObject.FindGameObjectWithTag("Player").transform;


        pickupDistance = Vector3.Distance(player.position, transform.position);

        if (pickupDistance <= 3)
        {
            if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<Holding>().ObjectsHolding < 1)
            {
                //PV = GetComponent<PhotonView>();
                //newObject = PhotonNetwork.Instantiate(resource.name, player.Find("Body").transform.position, Quaternion.identity);
                //PV.RPC(nameof(RPC_SetValues), RpcTarget.All);

                newObject = PhotonNetwork.Instantiate(resource.name, player.Find("Body").transform.position, Quaternion.identity);

                newObject.GetComponent<GrabScript>().itemIsPicked = true;

                //newObject.GetComponent<Rigidbody>().useGravity = false;
                //newObject.GetComponent<BoxCollider>().enabled = false;
                //newObject.GetComponent<Rigidbody>().isKinematic = true;
                //newObject.GetComponent<GrabScript>().toFollow = player.Find("Body");
                //player.GetComponent<Holding>().ObjectsHolding += 1;
                //newObject.GetComponent<GrabScript>().itemIsPicked = true;
                //newObject.GetComponent<GrabScript>().forceMulti = 0;

            }
        }
    }

    [PunRPC]
    void RPC_SetValues()
    {
        newObject = PhotonNetwork.Instantiate(resource.name, player.Find("Body").transform.position, Quaternion.identity);

        newObject.GetComponent<Rigidbody>().useGravity = false;
        newObject.GetComponent<BoxCollider>().enabled = false;
        newObject.GetComponent<Rigidbody>().isKinematic = true;
        newObject.GetComponent<GrabScript>().toFollow = player.Find("Body");
        player.GetComponent<Holding>().ObjectsHolding += 1;
        newObject.GetComponent<GrabScript>().itemIsPicked = true;
        newObject.GetComponent<GrabScript>().forceMulti = 0;
    }
}