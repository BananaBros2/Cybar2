using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class TableOrder : MonoBehaviour
{
    public int desiredDrink = -2;
    public int seatID;
    public bool satOn;
    public GameObject person;

    private bool drinkChosen = false;
    public bool orderRecieved;

    public Sprite[] drinkImages;

    public Image progressBar;
    public float progressFill;

    PhotonView view;

    private void Start()
    {
        seatID = transform.GetSiblingIndex();
    }

    public void FixedUpdate()
    {
        view = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (satOn && drinkChosen == false)
        {
            desiredDrink = Random.RandomRange(1, 7);
            drinkChosen = true;
            transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = drinkImages[desiredDrink];
            transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = true;
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            GetComponent<PhotonView>().RPC("RPC_ValueChanges", RpcTarget.OthersBuffered, desiredDrink, satOn, orderRecieved, person.gameObject.GetPhotonView().ViewID);
        }

        if(drinkChosen)
        {
            progressFill += Time.deltaTime;
            progressBar.fillAmount = 1 - progressFill / 30;

            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_BarSync");
            view.RPC("RPC_BarSync", RpcTarget.OthersBuffered, progressFill);

            if (progressBar.fillAmount <= 0) 
            {
                person.GetComponent<NavMeshAgent>().enabled = true;
                person.GetComponent<PersonScript>().goal = GameObject.FindGameObjectWithTag("Finish").transform;
                person.GetComponent<NavMeshAgent>().destination = GameObject.FindGameObjectWithTag("Finish").transform.position;

                GameObject.Find("Seats").GetComponent<AvailiableSeats>().tablesInUse.Remove(gameObject);
                GameObject.Find("Seats").GetComponent<AvailiableSeats>().allTables.Add(gameObject);

                GameObject.Find("StatsObject").GetComponent<GameStats>().levelScore -= 10;

                transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = drinkImages[0];
                transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = false;

                person.GetComponent<Rigidbody>().isKinematic = false;
                desiredDrink = -2;
                satOn = false;
                orderRecieved = false;
                drinkChosen = false;
                progressFill = 0;
                progressBar.fillAmount = 0;
                person = null;

                PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
                PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_HideOrder");
                view.RPC("RPC_HideOrder", RpcTarget.OthersBuffered);
            }
        }

        if (orderRecieved)
        {
            person.GetComponent<NavMeshAgent>().enabled = true;
            person.GetComponent<PersonScript>().goal = GameObject.FindGameObjectWithTag("Finish").transform;
            person.GetComponent<NavMeshAgent>().destination = GameObject.FindGameObjectWithTag("Finish").transform.position;
            
            GameObject.Find("Seats").GetComponent<AvailiableSeats>().tablesInUse.Remove(gameObject);
            GameObject.Find("Seats").GetComponent<AvailiableSeats>().allTables.Add(gameObject);

            GameObject.Find("StatsObject").GetComponent<GameStats>().levelScore += 30;

            transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = drinkImages[0];
            transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = false;

            person.GetComponent<Rigidbody>().isKinematic = false;
            desiredDrink = -2;
            satOn = false;
            orderRecieved = false;
            drinkChosen = false;
            person = null;
            progressFill = 0;
            progressBar.fillAmount = 0;

            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_HideOrder");
            view.RPC("RPC_HideOrder", RpcTarget.OthersBuffered);
        }
    }


    [PunRPC]
    void RPC_ValueChanges(int RPCdrink, bool RPCsatOn, bool RPCrecieved, int RPCIndex)
    {
        desiredDrink = RPCdrink;
        satOn = RPCsatOn;
        orderRecieved = RPCrecieved;

        person = PhotonView.Find(RPCIndex).gameObject;
        person.GetComponent<Rigidbody>().isKinematic = true;

        if (desiredDrink != -2)
        {
            transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = drinkImages[desiredDrink];
            transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = true;
        }
    }

    [PunRPC]
    void RPC_BarSync(float RPCFill)
    {
        progressFill = RPCFill;
        progressBar.fillAmount = 1 - progressFill / 40;
    }

    [PunRPC]
    void RPC_HideOrder()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = drinkImages[0];
        transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = false;

        person.GetComponent<Rigidbody>().isKinematic = false;
        desiredDrink = -2;
        satOn = false;
        orderRecieved = false;
        drinkChosen = false;
        person = null;
        progressFill = 0;
        progressBar.fillAmount = 0;
    }

}