using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailiableSeats : MonoBehaviour
{
    // Lists for tracking the what tables are in use.
    public List<GameObject> allTables;
    public List<GameObject> tablesInUse;

    void Start()
    {
        // Adds every gameobject with the Table tag to a gameobject list.
        allTables.AddRange(GameObject.FindGameObjectsWithTag("Table"));
    }
}
