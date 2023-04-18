using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PersonScript : MonoBehaviour
{
    public GameObject targetSeat;
    public Transform goal;
    //public GameObject turnKey;
    NavMeshAgent agent;
    public bool seated = false;

    public List<GameObject> allTables;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        
        if (goal == null)
        {
            allTables = GameObject.Find("Seats").GetComponent<availiableSeats>().allTables;
            int randomNumber = Random.Range(0, allTables.Count - 1);

            targetSeat = allTables[randomNumber];

            goal = targetSeat.transform;
            GameObject.Find("Seats").GetComponent<availiableSeats>().tablesInUse.Add(allTables[randomNumber]);
            GameObject.Find("Seats").GetComponent<availiableSeats>().allTables.RemoveAt(randomNumber);

            agent.destination = goal.position;
        }

        if (seated)
        {
            
        }
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.transform == goal)
        {
            goal.GetComponent<TableOrder>().satOn = true;
            seated = true;
            agent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(goal.position.x, goal.position.y + 1.5f, goal.position.z);
            transform.rotation = Quaternion.Euler(0, goal.rotation.y, goal.rotation.z);
        }

    }
                                                                                                                                                                                    

}
