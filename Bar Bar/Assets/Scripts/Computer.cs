using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Computer : MonoBehaviour
{
    [Header("Ingredients")]
    public bool vodka = false;
    public bool grapefruit = false;
    public bool orange = false;
    public bool cranberry = false;
    public bool pineapple = false;

    [Header("Computer")]
    public Transform Canvas;
    public bool Activated;
    public GameObject lastSelected;
    private Camera cam;

    public bool inUse;

    PhotonView view;

    public List<GameObject> orderSlots;

    void Start()
    {
        view = GetComponent<PhotonView>();

        Canvas = transform.GetChild(1);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.GetChild(1).gameObject.SetActive(false);
    }


    void Update()
    {

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            lastSelected.GetComponent<Button>().Select();
        }
        lastSelected = EventSystem.current.currentSelectedGameObject;

        transform.GetChild(1).LookAt(new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z));

        if (inUse == true)
        {
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            view.RPC("RPC_ValueChanges", RpcTarget.OthersBuffered, inUse);
        }

    }

    public GameObject testObject;

    public void UIClick(int buttonID)
    {
        GameObject instantiatedObject = PhotonNetwork.Instantiate(orderSlots[buttonID-1].name, transform.position, Quaternion.identity);
    }

    [PunRPC]
    void RPC_ValueChanges(bool RPCinUse)
    {
        inUse = RPCinUse;
    }
}
