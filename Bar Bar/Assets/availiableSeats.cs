using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class availiableSeats : MonoBehaviour
{

    public List<GameObject> allTables;
    public List<GameObject> tablesInUse;

    void Start()
    {
        allTables.AddRange(GameObject.FindGameObjectsWithTag("Table"));
    }
}
