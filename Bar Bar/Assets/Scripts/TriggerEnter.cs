using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class TriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.ToString() == "7" && other.tag == "Item")
        {
            if (other.gameObject.GetComponent<ItemData>().drinkID == transform.parent.gameObject.GetComponent<TableOrder>().desiredDrink)
            {
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = transform.position;
                other.transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.parent.gameObject.GetComponent<TableOrder>().orderRecieved = true;

                other.gameObject.GetComponent<ItemData>().drinkType1 = "_Empty_";
                other.gameObject.GetComponent<ItemData>().drinkType2 = "_Empty_";
                other.gameObject.GetComponent<ItemData>().drinkType3 = "_Empty_";
                other.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
            }

        }

    }
}
