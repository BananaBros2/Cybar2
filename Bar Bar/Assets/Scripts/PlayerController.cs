using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{


    bool slippery;
    GameObject closestPuddle;

    // object Interaction -------------------------
    public float pickupDistance;
    public Transform toFollow;
    private Vector3 offset;
    public bool Holding = false;
    GameObject closest;
    GameObject throwOrMix;
    public List<Transform> checkedItems;
    public Transform[] worldItems;

    public bool stallThrow;
    public bool holdingArrow;

    public bool occupied;

    public Image progressBarHold;
    public Image progressBar;
    public float progressFill;

    //  Photon   ------------------------
    public PhotonView view;

    public GameObject glassObject;

    // Start Values =========================================================================================================================================
    private void Start()
    {
        Cursor.visible = true;

        // Sets the photonview variable as the player's photon view component
        view = GetComponent<PhotonView>();

        // Finds and adds every current game object in the scene with the requested tags to the worldItems list
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
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Computer"))
        {
            checkedItems.Add(go.GetComponent<Transform>());
            worldItems = checkedItems.ToArray();
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Mop"))
        {
            checkedItems.Add(go.GetComponent<Transform>());
            worldItems = checkedItems.ToArray();
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("HUBComputer"))
        {
            checkedItems.Add(go.GetComponent<Transform>());
            worldItems = checkedItems.ToArray();
        }

        //Changes the properties of the physics material to stop the player from sticking to walls
        transform.GetChild(0).GetComponent<CapsuleCollider>().material.staticFriction = 0;
        transform.GetChild(0).GetComponent<CapsuleCollider>().material.dynamicFriction = 0;

        progressBar = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        progressBarHold.GetComponent<Image>().enabled = false;
        progressBar.GetComponent<Image>().enabled = false;

        toFollow = transform.Find("Body");
    }

    // Fixed Update Content =================================================================================================================================
    private void FixedUpdate()
    {
        // Checks if the current view is the owner of the game object, this avoids other players from duplicate running code in this game object.
        if (!view.IsMine) { return; }




        //if (Input.GetKey(KeyCode.LeftShift) && Holding == true)
        //{
        //    occupied = true;
        //    GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y - 0.5f, 0);
        //    progressFill += Time.deltaTime;
        //    progressBar.fillAmount = progressFill / 5;
        //    if (progressBar.fillAmount >= 1) { print("SAAA"); }
        //}
        //else { occupied = false; }


        if (closest == null) { }
        else if (slippery && Input.GetKey(KeyCode.LeftShift) && closest.tag == "Mop")
        {
            progressBarHold.GetComponent<Image>().enabled = true;
            progressBar.GetComponent<Image>().enabled = true;

            progressFill += Time.deltaTime;
            progressBar.fillAmount = progressFill / 5;
            if (progressBar.fillAmount >= 1)
            {
                view.RPC("RPC_Delete", RpcTarget.MasterClient, closestPuddle.GetPhotonView().ViewID);

                progressFill = 0;
                progressBar.fillAmount = 0;
            }
        }
        else
        {
            progressFill = 0;
            progressBar.fillAmount = 0;

            progressBarHold.GetComponent<Image>().enabled = false;
            progressBar.GetComponent<Image>().enabled = false;
        }


        if (occupied == true) { return; }

        float xTranslation = Input.GetAxis("HorizontalAim") * Time.deltaTime;
        float zTranslation = Input.GetAxis("VerticalAim") * Time.deltaTime;

        holdingArrow = false;

        if (xTranslation != 0 || zTranslation != 0)
        {
            holdingArrow = true;
            Vector3 input = (new Vector3(Input.GetAxis("HorizontalAim"), Input.GetAxis("VerticalAim"))).normalized;
            view.transform.GetChild(0).transform.rotation = Quaternion.RotateTowards(transform.GetChild(0).transform.rotation, Quaternion.Euler(Vector3.up * Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg), 1000 * Time.deltaTime);
        }
        else
        {
            Vector3 input = (new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized;
            if (input.x != 0 || input.y != 0)
            {
                view.transform.GetChild(0).transform.rotation = Quaternion.RotateTowards(transform.GetChild(0).transform.rotation, Quaternion.Euler(Vector3.up * Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg), 1000 * Time.deltaTime);
            }
        }


        if (slippery)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * 400, ForceMode.Force);
            Vector3 flatVel = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, GetComponent<Rigidbody>().velocity.z);

            if(flatVel.magnitude > 10)
            {
                Vector3 limitedVel = flatVel.normalized * 10;
                GetComponent<Rigidbody>().velocity = new Vector3(limitedVel.x, GetComponent<Rigidbody>().velocity.y, limitedVel.z);
            }

            slippery = false;
        }
        else
        {
            var normalizedVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            GetComponent<Rigidbody>().velocity = new Vector3(normalizedVector.x * Time.deltaTime * 500, GetComponent<Rigidbody>().velocity.y - 0.5f, normalizedVector.y * Time.deltaTime * 500);
        }

    }

    private void Update()
    {
        if (!view.IsMine ) { return; }


        if (Holding)
        {
            closest.GetComponent<PhotonView>().transform.position = toFollow.position - offset;
            closest.GetComponent<PhotonView>().transform.position = transform.position + Vector3.up * 1.5f;
            closest.GetComponent<PhotonView>().transform.rotation = toFollow.rotation;

            int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
            Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
            Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
            view.RPC("RPC_ItemChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, true, false, true, closest.tag);
        }


        if (occupied == true) { return; }
        
        Transform GetClosestObject(Transform[] objects)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Transform potentialTarget in objects)
            {

                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (potentialTarget.tag == "Item")
                {
                    if (dSqrToTarget < closestDistanceSqr && potentialTarget.GetComponent<ItemData>().beingHeld == false && !Holding)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = potentialTarget;
                    }

                }
                if (potentialTarget.tag == "Container")
                {
                    if (dSqrToTarget + 0.75f < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget + 0.75f;
                        bestTarget = potentialTarget;
                    }
                }
                if (potentialTarget.tag == "Computer")
                {
                    if (dSqrToTarget + 0.5f < closestDistanceSqr && potentialTarget.GetComponent<Computer>().inUse == false)
                    {
                        closestDistanceSqr = dSqrToTarget + 0.5f;
                        bestTarget = potentialTarget;
                    }
                }
                if (potentialTarget.tag == "HUBComputer")
                {
                    if (dSqrToTarget < closestDistanceSqr && potentialTarget.GetComponent<ComputerMenu>().inUse == false)
                    {
                        closestDistanceSqr = dSqrToTarget + 0.5f;
                        bestTarget = potentialTarget;
                    }
                }
                if (potentialTarget.tag == "Mop")
                {
                    if (dSqrToTarget + 0.75f < closestDistanceSqr && potentialTarget.GetComponent<MopData>().beingHeld == false)
                    {
                        closestDistanceSqr = dSqrToTarget + 0.5f;
                        bestTarget = potentialTarget;
                    }
                }

            }
            return bestTarget;
        }

        stallThrow = false;

        if (Input.GetKeyUp(KeyCode.Space) && Holding == false && stallThrow == false)
        {
            stallThrow = true;
            closest = GetClosestObject(worldItems).transform.gameObject;
            if(closest == null) { return; }

            pickupDistance = Vector3.Distance(closest.transform.position, transform.position);

            if (pickupDistance < 3f && Holding == false)
            {
                if (closest.tag == "Container")
                {
                    if (closest.GetComponent<ContainerData>().contents == "Glass" && closest.GetComponent<ContainerData>().count > 0 && !Input.GetKey(KeyCode.LeftShift))
                    {
                        if (!PhotonNetwork.IsMasterClient)
                        {

                            closest.GetComponent<ContainerData>().count -= 1;
                            closest.GetComponent<ContainerData>().view.RPC("RPC_ContainerDecrease", RpcTarget.AllBuffered, closest.GetComponent<ContainerData>().count);
                            view.RPC("RPC_InstantiateGlass", RpcTarget.MasterClient, transform.GetComponent<PhotonView>().Controller, transform.position);
                            closest = null;
                            return;
                        }
                        else
                        {
                            closest.GetComponent<ContainerData>().count -= 1;
                            closest.GetComponent<ContainerData>().view.RPC("RPC_ContainerDecrease", RpcTarget.AllBuffered, closest.GetComponent<ContainerData>().count);
                            closest = PhotonNetwork.Instantiate(glassObject.name, transform.position, Quaternion.identity);
                            Holding = true;
                            view.RPC("RPC_UpdateLists", RpcTarget.All, closest.GetPhotonView().ViewID);
                            return;
                        }
                    }
                    
                }
               
                if (closest.tag == "Item" || closest.tag == "Container" || closest.tag == "Mop")
                {
                    

                    offset = toFollow.position - transform.position;

                    closest.GetComponent<Rigidbody>().useGravity = false;
                    closest.GetComponent<BoxCollider>().enabled = false;
                    closest.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    Holding = true;

                    int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
                    Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
                    Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
                    view.RPC("RPC_ItemChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, true, false, true, closest.tag);

                }

                if (closest.tag == "Computer")
                {
                    if(!closest.GetComponent<Computer>().inUse && !closest.GetComponent<Computer>().ordered)
                    {
                        closest.transform.GetChild(1).gameObject.SetActive(true);
                        closest.GetComponent<Computer>().inUse = true;
                        PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_PlayerChanges");
                        view.RPC("RPC_PlayerChanges", RpcTarget.AllBuffered, true);
                        closest.GetComponent<Computer>().usedBy = this;
                        occupied = true;
                    }


                }

                if (closest.tag == "HUBComputer")
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        closest.transform.GetChild(1).gameObject.SetActive(true);
                        closest.GetComponent<ComputerMenu>().inUse = true;
                        PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_PlayerChanges");
                        view.RPC("RPC_PlayerChanges", RpcTarget.AllBuffered, true);
                        closest.GetComponent<ComputerMenu>().usedBy = this;
                        occupied = true;
                    }


                }

            }

        }


        if (Input.GetKeyUp(KeyCode.Space) && Holding == true && stallThrow == false && holdingArrow == false)
        {
            stallThrow = true;
            throwOrMix = GetClosestObject(worldItems).transform.gameObject;


            if (Vector3.Distance(throwOrMix.transform.position, transform.position) < 2.5f)
            {
                if (closest.tag != "Container" && throwOrMix.tag == "Container" && throwOrMix.GetComponent<ContainerData>().count > 0)
                {

                    string content = throwOrMix.GetComponent<ContainerData>().contents;
                    if (content == "Glass") { return; }

                    

                    if (closest.transform.GetComponent<ItemData>().drinkType1 == "_Empty_")
                    {
                        closest.transform.GetComponent<ItemData>().drinkType1 = content;
                        throwOrMix.transform.GetChild(throwOrMix.GetComponent<ContainerData>().count).gameObject.SetActive(false);
                        throwOrMix.GetComponent<ContainerData>().count -= 1;
                        throwOrMix.GetComponent<ContainerData>().view.RPC("RPC_ContainerDecrease", RpcTarget.AllBuffered, throwOrMix.GetComponent<ContainerData>().count);
                    }
                    else if (closest.transform.GetComponent<ItemData>().drinkType2 == "_Empty_")
                    {
                        closest.transform.GetComponent<ItemData>().drinkType2 = content;
                        throwOrMix.transform.GetChild(throwOrMix.GetComponent<ContainerData>().count).gameObject.SetActive(false);
                        throwOrMix.GetComponent<ContainerData>().count -= 1;
                        throwOrMix.GetComponent<ContainerData>().view.RPC("RPC_ContainerDecrease", RpcTarget.AllBuffered, throwOrMix.GetComponent<ContainerData>().count);
                    }
                    else if (closest.transform.GetComponent<ItemData>().drinkType3 == "_Empty_")
                    {
                        closest.transform.GetComponent<ItemData>().drinkType3 = content;
                        throwOrMix.transform.GetChild(throwOrMix.GetComponent<ContainerData>().count).gameObject.SetActive(false);
                        throwOrMix.GetComponent<ContainerData>().count -= 1;
                        throwOrMix.GetComponent<ContainerData>().view.RPC("RPC_ContainerDecrease", RpcTarget.AllBuffered, throwOrMix.GetComponent<ContainerData>().count);
                    }
                    return;
                }
            }



            closest.transform.GetComponent<Rigidbody>().useGravity = true;
            closest.transform.GetComponent<BoxCollider>().enabled = true;
            Holding = false;
            //closest.transform.position += transform.GetChild(0).transform.position*0.1f;
            //thisGameObject.transform.position -= new Vector3(0, 1, 0);
            int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
            Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
            Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
            view.RPC("RPC_ItemChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, false, false, false, closest.tag);
            closest = null;
        }

        if (Input.GetKeyUp(KeyCode.Space) && Holding == true && stallThrow == false && holdingArrow == true)
        {
            stallThrow = true;
            closest.transform.GetComponent<Rigidbody>().useGravity = true;
            closest.transform.GetComponent<BoxCollider>().enabled = true;
            Holding = false;

            int ObjectRPCID = closest.GetComponent<PhotonView>().ViewID;
            Vector3 newRPCPosition = closest.GetComponent<PhotonView>().transform.position;
            Quaternion newRPCRotation = closest.GetComponent<PhotonView>().transform.rotation;
            view.RPC("RPC_ItemChanges", RpcTarget.All, ObjectRPCID, newRPCPosition, newRPCRotation, false, true, false, closest.tag);
            closest = null;
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Spillage") { slippery = true; closestPuddle = other.gameObject; }
        
    }

    [PunRPC]
    void RPC_InstantiateGlass(Player RPCSentBy, Vector3 RPCTransform)
    {
        GameObject Glass = PhotonNetwork.Instantiate(glassObject.name, RPCTransform, Quaternion.identity);
        view.RPC("RPC_GlassFollowUp", RPCSentBy, Glass.GetPhotonView().ViewID);
        view.RPC("RPC_UpdateLists", RpcTarget.All, Glass.GetPhotonView().ViewID);
    }
    [PunRPC]
    void RPC_GlassFollowUp(int RPCGlass)
    {
        transform.GetComponent<PhotonView>();
        closest = PhotonView.Find(RPCGlass).gameObject;
        Holding = true;
        toFollow = transform.Find("Body");
    }

    [PunRPC]
    void RPC_UpdateLists(int RPCGlass)
    {
        // Runs the following code per every object tagged as a player
        foreach (GameObject players in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.GetComponent<PlayerController>().checkedItems.Add(PhotonView.Find(RPCGlass).transform);
            players.GetComponent<PlayerController>().worldItems = players.GetComponent<PlayerController>().checkedItems.ToArray();
        }
    }


    [PunRPC]
    void RPC_ItemChanges(int objectRPC, Vector3 newPosition, Quaternion newRotation, bool phantom, bool Thrown, bool Holding, string objectTag)
    {
        PhotonView ObjectSync = PhotonView.Find(objectRPC);
        ObjectSync.transform.position = newPosition;
        ObjectSync.transform.rotation = newRotation;
        ObjectSync.transform.GetComponent<Rigidbody>().useGravity = !phantom;
        ObjectSync.transform.GetComponent<Rigidbody>().isKinematic = phantom;
        if(objectTag == "Item")
            ObjectSync.transform.GetComponent<ItemData>().beingHeld = Holding;

        if (phantom)
        {
            ObjectSync.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ObjectSync.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (Thrown)
            ObjectSync.transform.GetComponent<Rigidbody>().AddForce(transform.Find("Body").forward * 1000 * 2);
    }

    [PunRPC]
    void RPC_PlayerChanges(bool RPCkinematic)
    {
        transform.GetComponent<Rigidbody>().isKinematic = RPCkinematic;
        
    }

    [PunRPC]
    void RPC_Delete(int RPCID)
    {

        PhotonNetwork.Destroy(PhotonView.Find(RPCID).gameObject);

    }


}