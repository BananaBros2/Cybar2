using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonScript : MonoBehaviour
{
    public Transform goal;
    //public GameObject turnKey;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //goal = transform.Find("Seats").GetComponent<Seat_Library>().worldItems[Random.Range(0, transform.Find("Seats").GetComponent<Seat_Library>().worldItems.Length)];
        agent.destination = goal.position;
    }

    private void Update()
    {
        //if (grabScript.itemIsPicked == true) { agent.isStopped = true; }
        //turnKey.transform.rotation *= Quaternion.AngleAxis(70 * agent.speed * Time.deltaTime, Vector3.left);
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.transform == goal)
        {
            
            goal = GameObject.Find("Seats").transform.GetChild(Random.Range(0,25));
            agent.destination = goal.position;
        }

    }
                                                                                                                                                                                    

}
