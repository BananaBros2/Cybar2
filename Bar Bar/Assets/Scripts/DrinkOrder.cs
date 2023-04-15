using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkOrder : MonoBehaviour
{
    public string desiredDrink;
    public string[] highballs;

    void Start()
    {
        List<string> highballs = new List<string> { "Vodka", "Orange", "Grapefruit", "Cranberry", "Pineapple"};
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.transform.tag);
        //print(other.gameObject.GetComponent<Typing>().drinkType);
        if (other.gameObject.layer.ToString() == "Items")
        {
            if (other.gameObject.GetComponent<ItemData>().drinkType1 == desiredDrink)
            {
                Destroy(other.gameObject);
                print("yay");

            }

        }

    }
}