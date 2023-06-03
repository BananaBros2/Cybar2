using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class ComputerMenu : MonoBehaviour
{

    [Header("Computer")]
    public Transform Canvas;
    public bool Activated;
    public GameObject lastSelected;
    private Camera cam;

    public bool inUse;
    public PlayerController usedBy;
    public int coolDown;

    PhotonView view;

    public PauseMenu pauseMenu;
    bool newPause;

    void Start()
    {
        view = GetComponent<PhotonView>();

        Canvas = transform.GetChild(1);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.GetChild(1).gameObject.SetActive(false);
    }


    void Update()
    {
        if (pauseMenu.gamePaused) 
        {
            newPause = true;
            return;
        }
        if (!pauseMenu.gamePaused && newPause)
        {
            lastSelected.GetComponent<Button>().Select();
            newPause = false;
        }

        if (PhotonNetwork.IsMasterClient)
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
            }


            if (coolDown == 0)
            {
                inUse = false;
            }
        }   

    }

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

        if (buttonID == 1)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            coolDown = 50;
            usedBy.occupied = false;
            usedBy.stallThrow = true;
            usedBy.view.RPC("RPC_PlayerChanges", RpcTarget.AllBuffered, true);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("1-3");
        }


    }

}
