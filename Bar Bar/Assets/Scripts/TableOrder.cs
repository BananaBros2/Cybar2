using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TableOrder : MonoBehaviour
{
    public int desiredDrink = -2;
    public int seatID;
    public bool satOn;
    public GameObject person;

    private bool drinkChosen = false;
    public bool orderRecieved;

    public Sprite[] drinkImages;

    //Set these Textures in the Inspector
    public Texture m_MainTexture, m_Normal, m_Metal;
    Renderer m_Renderer;

    private void Start()
    {
        seatID = transform.GetSiblingIndex();
        print(seatID);
    }

    private void Update()
    {
        if (satOn && drinkChosen == false)
        {
            desiredDrink = Random.RandomRange(1, 7);
            drinkChosen = true;
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = drinkImages[desiredDrink];
        }
        if (orderRecieved)
        {
            person.GetComponent<NavMeshAgent>().enabled = true;
            person.GetComponent<PersonScript>().goal = GameObject.FindGameObjectWithTag("Finish").transform;
            person.GetComponent<NavMeshAgent>().destination = GameObject.FindGameObjectWithTag("Finish").transform.position;
            GameObject.Find("Seats").GetComponent<AvailiableSeats>().tablesInUse.Remove(gameObject);
            GameObject.Find("Seats").GetComponent<AvailiableSeats>().allTables.Add(gameObject);
            person.GetComponent<Rigidbody>().isKinematic = false;
            desiredDrink = -2;
            satOn = false;
            orderRecieved = false;
        }
    }
    
}