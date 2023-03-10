using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTSCRIPTREMORESOON : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Holding>().ObjectsHolding += 1;
        GetComponent<GrabScript>().forceMulti = 0;

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
