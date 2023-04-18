using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.ToString() == "7")
        {
            if (other.gameObject.GetComponent<ItemData>().drinkID == transform.parent.gameObject.GetComponent<TableOrder>().desiredDrink)
            {
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = transform.position;
                other.transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                transform.parent.gameObject.GetComponent<TableOrder>().orderRecieved = true;
            }

        }

    }
}
