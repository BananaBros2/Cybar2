using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTest : MonoBehaviour
{

    public string desiredDrink;
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.transform.tag);
        //print(other.gameObject.GetComponent<Typing>().drinkType);
        if(other.gameObject.layer.ToString() == "Items")
        {
            if (other.gameObject.GetComponent<Typing>().drinkType == desiredDrink)
            {
                Destroy(other.gameObject);
                print("yay");
            }
        }
        
    }
}
