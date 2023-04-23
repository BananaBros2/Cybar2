using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WindUpPath : MonoBehaviour
{
    public Transform goal;
    public GameObject turnKey;
    NavMeshAgent agent;

    // Start is called before the first frame update 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    private void Update()
    {
        //if (grabScript.itemIsPicked == true) { agent.isStopped = true; }
        turnKey.transform.rotation *= Quaternion.AngleAxis(70 * agent.speed * Time.deltaTime, Vector3.left);
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.transform == goal)
        {
            goal = GameObject.Find("Seats").transform.GetChild(Random.Range(0, 24));
            agent.destination = goal.position;
        }
        //if(collidedObject.transform == goal)
        //{
        //    print("oh no");
        //    if(collidedObject.transform.tag == "Table")
        //    {
        //        goal = GameObject.Find("Bluid Keg").transform;
        //        agent.destination = goal.position;
        //    }
        //    if (collidedObject.transform.tag == "Keg")
        //    {
        //        goal = GameObject.Find("Goal").transform;
        //        agent.destination = goal.position;
        //    }
        //}

    }
}
