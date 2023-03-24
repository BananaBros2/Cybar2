using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerData : MonoBehaviour
{
    public string contents = "Vodka";
    public int count;
    public int capacity = 3;

    private void Start()
    {
        count = capacity;
    }

}
