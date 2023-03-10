using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        print(collision.transform.tag);
        if (Input.GetButton("Grab"))
        {
             collision.gameObject.transform.position = transform.position;
        }
    }
}
