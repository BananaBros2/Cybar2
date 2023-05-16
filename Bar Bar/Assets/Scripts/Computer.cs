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
    public bool ordered = false;

    public Image progressBarHold;
    public Image progressBar;
    public float progressFill;




    public GameObject block;
    int total;

    public BoxCollider deliveryZone;
    Vector3 size;




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
            view.RPC("RPC_ValueChanges", RpcTarget.AllBuffered, inUse, ordered);
        }

        
        if (coolDown == 0)
        {
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            view.RPC("RPC_ValueChanges", RpcTarget.AllBuffered, false, ordered);
        }

        if (!inUse && ordered)
        {
            progressBarHold.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(true);

            progressFill += Time.deltaTime;
            progressBar.fillAmount = progressFill / 6;
            if (progressBar.fillAmount >= 1)
            {
                transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().Select();
                progressFill = 0;
                ordered = false;

                progressBarHold.gameObject.SetActive(false);
                progressBar.gameObject.SetActive(false);

                size = deliveryZone.size;

                for (int y = 0; y < 250; ++y)
                {
                    for (int x = 0; x < (size.x / 2); ++x)
                    {
                        for (int z = 0; z < (size.z / 2); ++z)
                        {
                            for (int i = 0; i < currentOrder.Count; ++i)
                            {
                                if (currentOrder[i])
                                {
                                    Vector3 boxSpawnArea = new Vector3(deliveryZone.transform.position.x + 1 + x * 2 - (size.x / 2), deliveryZone.transform.position.y - 2 + y * 0.8f, deliveryZone.transform.position.z + 1 + z * 2 - (size.z / 2));
                                    int layerMask = (1 << LayerMask.NameToLayer("Items"));

                                    if (Physics.CheckBox(boxSpawnArea, new Vector3(0.5f, 0.4f, 0.55f), Quaternion.Euler(0,0,0), layerMask)) { break; }

                                    GameObject instantiatedObject = PhotonNetwork.Instantiate(orderSlots[i].name, boxSpawnArea, Quaternion.identity);

                                    transform.GetChild(1).GetChild(i + 1).GetChild(0).GetComponent<Image>().enabled = false;
                                    currentOrder[i] = false;
                                    GameObject.Find("StatsObject").GetComponent<GameStats>().levelScore -= 30;
                                    if (instantiatedObject.GetComponent<ContainerData>().contents == "Glass") { GameObject.Find("StatsObject").GetComponent<GameStats>().levelScore -= 20; }
                                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().checkedItems.Add(instantiatedObject.transform);
                                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().worldItems = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().checkedItems.ToArray();
                                    total -= 1;
                                    break;
                                }
                            }
                            
                            if (total == 0) { return; }
                        }

                    }
                }


            }
            

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
            total -= 1;
            transform.GetChild(1).GetChild(buttonID).GetChild(0).GetComponent<Image>().enabled = false;
            for (int i = 0; i < currentOrder.Count; ++i)
            {
                if (currentOrder[i] == true) { return; }
            }
            ordered = false;

        }
        else
        {
            total += 1;
            currentOrder[buttonID - 1] = true;
            transform.GetChild(1).GetChild(buttonID).GetChild(0).GetComponent<Image>().enabled = true;
            ordered = true;
        }
        
    }

    [PunRPC]
    void RPC_ValueChanges(bool RPCinUse, bool RPCordered)
    {
        inUse = RPCinUse;
        ordered = RPCordered;
    }
}
