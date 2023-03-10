using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[DefaultExecutionOrder(1)]
public class GrabScript : MonoBehaviourPunCallbacks
{
    private Transform PickUpPoint;
    private Transform player;

    public float pickupDistance;
    public float forceMulti;

    public bool readyToThrow;
    public bool itemIsPicked;

    public float throwForce = 1000f;
    private Rigidbody rb;

    PhotonView view;
    GameObject thisGameObject;

    private PhotonView PV;


    public Transform toFollow;
    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        //if (!view.IsMine)
        //return;

        //PV = GetComponent<PhotonView>();
        //PV.RPC(nameof(RPC_Start), RpcTarget.All);

        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        GetComponent<Rigidbody>().isKinematic = false;

        PV = GetComponent<PhotonView>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        toFollow = player.Find("Body");

        offset = toFollow.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!view.IsMine)
            //return;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickUpPoint = player.Find("PickUpPoint").transform;
        toFollow = player.Find("Body");





        if (Input.GetKey(KeyCode.E) && itemIsPicked == true && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
        }

        pickupDistance = Vector3.Distance(player.position, transform.position);

        if (pickupDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && itemIsPicked == false && player.GetComponent<Holding>().ObjectsHolding < 1)
            {
                //PV.RPC(nameof(RPC_PickUp), RpcTarget.All);
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                player.GetComponent<Holding>().ObjectsHolding += 1;
                itemIsPicked = true;
                forceMulti = 0;

            }
        }

        if (itemIsPicked)
        {
            //PV.RPC(nameof(RPC_Holding), RpcTarget.All);
            player = GameObject.FindGameObjectWithTag("Player").transform;
            PickUpPoint = player.Find("PickUpPoint").transform;
            toFollow = player.Find("Body");

            //PV.RPC(nameof(RPC_Holding), RpcTarget.All);

            transform.position = toFollow.position - offset;
            transform.position = transform.position + Vector3.up * 1.5f;
            transform.rotation = toFollow.rotation;
            view.GetComponent<Rigidbody>().useGravity = false;
            view.GetComponent<BoxCollider>().enabled = false;
        }


        if (Input.GetKeyUp(KeyCode.E) && itemIsPicked == true)
        {
            readyToThrow = true;
            if (forceMulti > 10)
            {
                //PV.RPC(nameof(RPC_Drop), RpcTarget.All);

                player = GameObject.FindGameObjectWithTag("Player").transform;
                PickUpPoint = player.Find("PickUpPoint").transform;
                toFollow = player.Find("Body");

                //thisGameObject.transform.position += transform.forward;
                //thisGameObject.transform.position -= new Vector3(0, 1, 0);
                view.GetComponent<Rigidbody>().useGravity = true;
                view.GetComponent<BoxCollider>().enabled = true;
                toFollow = null;
                view.transform.gameObject.GetComponent<GrabScript>().itemIsPicked = false;
                readyToThrow = false;
                player.GetComponent<Holding>().ObjectsHolding -= 1;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q) && itemIsPicked == true)
        {
            //PV.RPC(nameof(RPC_Throw), RpcTarget.All);

            player = GameObject.FindGameObjectWithTag("Player").transform;
            PickUpPoint = player.Find("PickUpPoint").transform;
            toFollow = player.Find("Body");

            rb.AddForce(transform.forward * throwForce * 2);
            view.GetComponent<Rigidbody>().useGravity = true;
            view.GetComponent<BoxCollider>().enabled = true;
            toFollow = null;
            itemIsPicked = false;
            readyToThrow = false;
            player.GetComponent<Holding>().ObjectsHolding -= 1;
        }

    }


    //[PunRPC]
    //void RPC_Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    view = GetComponent<PhotonView>();
    //    GetComponent<Rigidbody>().isKinematic = false;

    //    PV = GetComponent<PhotonView>();

    //    player = GameObject.FindGameObjectWithTag("Player").transform;
    //    toFollow = player.Find("Body");

    //    offset = toFollow.position - transform.position;
    //}

    [PunRPC]
    void RPC_PickUp()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickUpPoint = player.Find("PickUpPoint").transform;
        toFollow = player.Find("Body");

        view.GetComponent<Rigidbody>().useGravity = false;
        view.GetComponent<BoxCollider>().enabled = false;
        toFollow = player.Find("Body");
        player.GetComponent<Holding>().ObjectsHolding += 1;
        view.transform.gameObject.GetComponent<GrabScript>().itemIsPicked = true;
        forceMulti = 0;
    }

    [PunRPC]
    void RPC_Holding()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickUpPoint = player.Find("PickUpPoint").transform;
        toFollow = player.Find("Body");

        transform.position = toFollow.position - offset;
        transform.position = transform.position + Vector3.up * 1.5f;
        transform.rotation = toFollow.rotation;
        view.GetComponent<Rigidbody>().useGravity = false;
        view.GetComponent<BoxCollider>().enabled = false;
    }

    [PunRPC]
    void RPC_Drop()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickUpPoint = player.Find("PickUpPoint").transform;
        toFollow = player.Find("Body");

        //thisGameObject.transform.position += transform.forward;
        //thisGameObject.transform.position -= new Vector3(0, 1, 0);
        view.GetComponent<Rigidbody>().useGravity = true;
        view.GetComponent<BoxCollider>().enabled = true;
        toFollow = null;
        view.transform.gameObject.GetComponent<GrabScript>().itemIsPicked = false;
        readyToThrow = false;
        player.GetComponent<Holding>().ObjectsHolding -= 1;
    }

    [PunRPC]
    void RPC_Throw()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickUpPoint = player.Find("PickUpPoint").transform;
        toFollow = player.Find("Body");

        rb.AddForce(transform.forward * throwForce * 2);
        view.GetComponent<Rigidbody>().useGravity = true;
        view.GetComponent<BoxCollider>().enabled = true;
        toFollow = null;
        itemIsPicked = false;
        readyToThrow = false;
        player.GetComponent<Holding>().ObjectsHolding -= 1;
    }

    [PunRPC]
    void RPC_SetValues()
    {

    }

}
