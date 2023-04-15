using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonScript : MonoBehaviour
{
    public Transform goal;
    //public GameObject turnKey;
    NavMeshAgent agent;
    public bool seated = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if(goal == null)
        {
            goal = GameObject.Find("Seats").transform.GetChild(Random.Range(0, GameObject.Find("Seats").transform.childCount-1));
            agent.destination = goal.position;
        }
        if(seated)
        {
            
        }
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.transform == goal)
        {
            seated = true;
            agent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(goal.position.x, goal.position.y + 1.5f, goal.position.z);
            transform.rotation = Quaternion.Euler(0, goal.rotation.y, goal.rotation.z);
        }

    }
                                                                                                                                                                                    

}
