using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

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




    public Transform toFollow;
    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        GetComponent<Rigidbody>().isKinematic = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickUpPoint = player.Find("PickUpPoint").transform;
        toFollow = player.Find("Body");

        offset = toFollow.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickUpPoint = player.Find("PickUpPoint").transform;
        toFollow = player.Find("Body");

        //var id = PhotonNetwork.AllocateViewID(0);

        //this.photonView.RPC("ChatMessage", RpcTarget.All);








        

        if (Input.GetKey(KeyCode.E) && itemIsPicked == true && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
        }

        pickupDistance = Vector3.Distance(player.position, transform.position);

        if (pickupDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && itemIsPicked == false && player.GetComponent<Holding>().ObjectsHolding < 1)
            {
                
                view.GetComponent<Rigidbody>().useGravity = false;
                view.GetComponent<BoxCollider>().enabled = false;
                toFollow = player.Find("Body");
                player.GetComponent<Holding>().ObjectsHolding += 1;
                view.transform.gameObject.GetComponent<GrabScript>().itemIsPicked = true;
                forceMulti = 0;
            }
        }

        if (itemIsPicked)
        {
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
            rb.AddForce(transform.forward * throwForce * 2);
            view.GetComponent<Rigidbody>().useGravity = true;
            view.GetComponent<BoxCollider>().enabled = true;
            toFollow = null;
            itemIsPicked = false;
            readyToThrow = false;
            player.GetComponent<Holding>().ObjectsHolding -= 1;
        }

    }

    [PunRPC]
    void ChatMessage()
    {
        // the photonView.RPC() call is the same as without the info parameter.
        // the info.Sender is the player who called the RPC.
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //PhotonView photonView = player.GetComponent<PhotonView>();
        //PhotonNetwork.AllocateRoomViewID(photonView);

    }


}
