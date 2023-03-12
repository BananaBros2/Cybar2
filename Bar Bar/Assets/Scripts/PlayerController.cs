using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{

    // Movement -------------------------
    bool airbourne = true;

    // Grabbing -------------------------
    public float pickupDistance;
    public Transform toFollow;
    private Vector3 offset;
    public bool Holding = false;
    GameObject closest;

    public List<Transform> checkedItems;
    public Transform[] worldItems;


    public bool readyToThrow;
    public float forceMulti;

    //  Photon   ------------------------
    PhotonView view;


    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            //gameObject.tag = "NLPlayer";
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Item"))
        {
            checkedItems.Add(go.GetComponent<Transform>());
            worldItems = checkedItems.ToArray();
        }

        var photonView = GetComponent<PhotonView>();
        photonView.Owner.NickName = "Player " + PhotonNetwork.PlayerList.Length;


    }

    private void FixedUpdate() // Update is called once per frame
    {
        if (view.IsMine)
        {
            if (airbourne)
            {
                transform.position += Physics.gravity * Time.deltaTime;
            }

            float xTranslation = Input.GetAxis("Horizontal") * Time.deltaTime;
            float zTranslation = Input.GetAxis("Vertical") * Time.deltaTime;

            if (xTranslation != 0 || zTranslation != 0)
            {
                Vector3 input = (new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized;
                view.transform.GetChild(0).transform.rotation = Quaternion.RotateTowards(transform.GetChild(0).transform.rotation, Quaternion.Euler(Vector3.up * Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg), 1000 * Time.deltaTime);
            }
            var normalizedVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            view.transform.Translate(normalizedVector.x * Time.deltaTime * 10, 0, normalizedVector.y * Time.deltaTime * 10);


            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

    }


    private void Update()
    {
        if (!view.IsMine)
            return;

        Transform GetClosestObject(Transform[] objects)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Transform potentialTarget in objects)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && potentialTarget.GetComponent<ItemData>().beingHeld == false)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        }










        if (Input.GetKey(KeyCode.E) && Holding && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.E) && Holding == false)
        {
            closest = GetClosestObject(worldItems).transform.gameObject;
            pickupDistance = Vector3.Distance(closest.transform.position, transform.position);

            if (pickupDistance < 2.5f && Holding == false)
            {
                toFollow = transform.Find("Body");

                view.RPC(nameof(RPC_Holding), RpcTarget.All);
                //PV.RPC(nameof(RPC_Holding), RpcTarget.All);
                offset = toFollow.position - transform.position;

                closest.GetComponent<Rigidbody>().useGravity = false;
                closest.GetComponent<BoxCollider>().enabled = false;
                closest.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                Holding = true;
                forceMulti = 0;

                int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
                Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
                Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
                view.RPC("RPC_ValueChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, true, false, true);
            }

        }


        if (Holding)
        {
            closest.GetComponent<PhotonView>().transform.position = toFollow.position - offset;
            closest.GetComponent<PhotonView>().transform.position = transform.position + Vector3.up * 1.5f;
            closest.GetComponent<PhotonView>().transform.rotation = toFollow.rotation;

            int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
            Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
            Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
            view.RPC("RPC_ValueChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, true, false, true);
        }


        if (Input.GetKeyUp(KeyCode.E) && Holding == true)
        {
            readyToThrow = true;
            if (forceMulti > 10)
            {
                //thisGameObject.transform.position += transform.forward;
                //thisGameObject.transform.position -= new Vector3(0, 1, 0);
                closest.transform.GetComponent<Rigidbody>().useGravity = true;
                closest.transform.GetComponent<BoxCollider>().enabled = true;
                readyToThrow = false;
                Holding = false;
                forceMulti = 0;

                int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
                Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
                Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
                view.RPC("RPC_ValueChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, false, false, false);
            }
        }

        if (Input.GetKeyUp(KeyCode.Q) && Holding == true)
        {
            closest.transform.GetComponent<Rigidbody>().useGravity = true;
            closest.transform.GetComponent<BoxCollider>().enabled = true;
            readyToThrow = false;
            Holding = false;
            forceMulti = 0;

            int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
            Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
            Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
            view.RPC("RPC_ValueChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, false, true, false);
        }


        

    }


    private void OnCollisionEnter()
    {
        airbourne = false;
    }
    private void OnCollisionExit(Collision exitedObject)
    {
        airbourne = true;
    }


    [PunRPC]
    void RPC_ValueChanges(int objectRPC, Vector3 newPosition, Quaternion newRotation, bool phantom, bool Thrown, bool Holding)
    {
        PhotonView ObjectSync = PhotonView.Find(objectRPC);
        ObjectSync.transform.position = newPosition;
        ObjectSync.transform.rotation = newRotation;
        ObjectSync.transform.GetComponent<Rigidbody>().useGravity = !phantom;
        ObjectSync.transform.GetComponent<Rigidbody>().useGravity = !phantom;
        if(phantom)
        {
            ObjectSync.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ObjectSync.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (Thrown)
            ObjectSync.transform.GetComponent<Rigidbody>().AddForce(transform.Find("Body").forward * 1000 * 2);
        ObjectSync.transform.GetComponent<ItemData>().beingHeld = Holding;
    }




    [PunRPC]
    void RPC_Holding()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(0, 1, 0);

    }

}