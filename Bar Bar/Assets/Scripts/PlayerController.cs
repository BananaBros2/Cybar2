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
    GameObject throwOrMix;
    public List<Transform> checkedItems;
    public Transform[] worldItems;

    public bool readyToThrow;
    public float forceMulti;

    //  Photon   ------------------------
    PhotonView view;

    public GameObject Beer;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        //view.Owner.NickName = "Player " + PhotonNetwork.PlayerList.Length;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Item"))
        {
            checkedItems.Add(go.GetComponent<Transform>());
            worldItems = checkedItems.ToArray();
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Container"))
        {
            checkedItems.Add(go.GetComponent<Transform>());
            worldItems = checkedItems.ToArray();
        }

    }

    private void FixedUpdate() // Update is called once per frame
    {
        if (view.IsMine)
        {
            if (airbourne)
            {
                transform.GetComponent<Rigidbody>().AddForce(Physics.gravity * Time.deltaTime * 10);
                //transform.position += Physics.gravity * Time.deltaTime;
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
                print(potentialTarget);
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (potentialTarget.tag == "Item")
                {
                    if (dSqrToTarget < closestDistanceSqr && potentialTarget.GetComponent<ItemData>().beingHeld == false)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = potentialTarget;
                    }
                }
                if (potentialTarget.tag == "Container")
                {
                    if (dSqrToTarget < closestDistanceSqr && potentialTarget.GetComponent<ContainerData>().count > 0)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = potentialTarget;
                    }
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
                if (closest.tag == "Container")
                {
                    if (closest.GetComponent<ContainerData>().contents == "Glass")
                    {
                        closest = PhotonNetwork.Instantiate(Beer.name, transform.position, Quaternion.identity);
                        checkedItems.Add(closest.transform);
                        worldItems = checkedItems.ToArray();
                    }
                    
                }
               
                if (closest.tag == "Item")
                {
                    toFollow = transform.Find("Body");

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
            throwOrMix = GetClosestObject(worldItems).transform.gameObject;

            if (Vector3.Distance(throwOrMix.transform.position, transform.position) < 2.5f)
            {
                if (throwOrMix.tag == "Container")
                {
                    string content = throwOrMix.GetComponent<ContainerData>().contents;
                    if (content == "Glass") { return; }

                    if (closest.transform.GetComponent<ItemData>().drinkType1 == "Empty")
                    {
                        closest.transform.GetComponent<ItemData>().drinkType1 = content;
                    }
                    else if (closest.transform.GetComponent<ItemData>().drinkType2 == "Empty")
                    {
                        closest.transform.GetComponent<ItemData>().drinkType2 = content;
                    }
                    else if (closest.transform.GetComponent<ItemData>().drinkType3 == "Empty")
                    {
                        closest.transform.GetComponent<ItemData>().drinkType3 = content;
                    }
                    else { Debug.Log("Full"); }
                    return;
                }
            }

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
        ObjectSync.transform.GetComponent<Rigidbody>().isKinematic = phantom;
        ObjectSync.transform.GetComponent<ItemData>().beingHeld = Holding;
        if (phantom)
        {
            ObjectSync.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ObjectSync.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (Thrown)
            ObjectSync.transform.GetComponent<Rigidbody>().AddForce(transform.Find("Body").forward * 1000 * 2);
    }

}