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
    public PlayerController usedBy;
    public int coolDown;

    PhotonView view;

    public List<GameObject> orderSlots;
    public List<bool> currentOrder;
    public bool ordered;

    void Start()
    {
        view = GetComponent<PhotonView>();

        Canvas = transform.GetChild(1);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.GetChild(1).gameObject.SetActive(false);
        currentOrder = new List<bool> {false, false, false, false, false, false, false, false};
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
            coolDown -= 1;
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            view.RPC("RPC_ValueChanges", RpcTarget.AllBuffered, inUse);
        }

        
        if (coolDown == 0)
        {
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            view.RPC("RPC_ValueChanges", RpcTarget.AllBuffered, false);
        }

        if (!inUse && ordered)
        {

            for (int i = 0; i < currentOrder.Count; ++i)
            {
                if (currentOrder[i])
                {
                    GameObject instantiatedObject = PhotonNetwork.Instantiate(orderSlots[i].name, transform.position, Quaternion.identity);
                    transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>().enabled = false;
                    currentOrder[i] = false;
                    GameObject.Find("StatsObject").GetComponent<GameStats>().levelScore -= 30;
                }
            }
            ordered = false;
        }

    }

    public GameObject testObject;

    public void UIClick(int buttonID)
    {
        if (buttonID == -1)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            coolDown = 50;
            usedBy.occupied = false;
            usedBy.stallThrow = true;
            usedBy.view.RPC("RPC_PlayerChanges", RpcTarget.AllBuffered, false);
            return;
        }

        if(currentOrder[buttonID-1])
        {
            currentOrder[buttonID - 1] = false;
            transform.GetChild(1).GetChild(buttonID - 1).GetChild(0).GetComponent<Image>().enabled = false;
            for (int i = 0; i < currentOrder.Count; ++i)
            {
                if (currentOrder[i] == true) { return; }
            }
            ordered = false;

        }
        else
        {
            currentOrder[buttonID - 1] = true;
            transform.GetChild(1).GetChild(buttonID - 1).GetChild(0).GetComponent<Image>().enabled = true;
            ordered = true;
        }
        
    }

    [PunRPC]
    void RPC_ValueChanges(bool RPCinUse)
    {
        inUse = RPCinUse;
    }
}
