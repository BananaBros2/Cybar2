using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Photon.Pun;

public class PersonScript : MonoBehaviour
{
    public GameObject targetSeat;
    public Transform goal;
    //public GameObject turnKey;
    NavMeshAgent agent;
    public bool seated = false;

    int randomNumber;
    public List<GameObject> allTables;

    public PhotonView view;


    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
    }


    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
        { 
            return;
        }

        if (goal == null)
        {
            allTables = GameObject.Find("Seats").GetComponent<AvailiableSeats>().allTables;
            randomNumber = Random.Range(0, allTables.Count - 1);

            targetSeat = allTables[randomNumber];

            goal = targetSeat.transform;
            GameObject.Find("Seats").GetComponent<AvailiableSeats>().tablesInUse.Add(allTables[randomNumber]);
            GameObject.Find("Seats").GetComponent<AvailiableSeats>().allTables.RemoveAt(randomNumber);

            agent.destination = goal.position;
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            view.RPC("RPC_ValueChanges", RpcTarget.OthersBuffered, goal.position, goal.rotation, seated);
        }

    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (collidedObject.transform == goal)
        {
            if (collidedObject.transform.tag == "Finish")
            {
                PhotonNetwork.Destroy(gameObject);
                return;
            }

            goal.GetComponent<TableOrder>().satOn = true;
            goal.GetComponent<TableOrder>().person = gameObject;
            seated = true;
            agent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(goal.position.x, goal.position.y + 1.5f, goal.position.z);
            transform.rotation = Quaternion.Euler(0, goal.rotation.y, goal.rotation.z);
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            view.RPC("RPC_ValueChanges", RpcTarget.OthersBuffered, goal.position, goal.rotation, seated);
        }

    }


    [PunRPC]
    void RPC_ValueChanges(Vector3 RPCgoalPosition, Quaternion RPCgoalRotation, bool RPCseated)
    {
        if(RPCseated == true)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(RPCgoalPosition.x, RPCgoalPosition.y + 1.5f, RPCgoalPosition.z);
            transform.rotation = Quaternion.Euler(0, RPCgoalRotation.y, RPCgoalRotation.z);
        }
        else
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<NavMeshAgent>().destination = RPCgoalPosition;
        }

    }

}
