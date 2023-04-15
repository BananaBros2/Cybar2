using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Seat_Library : MonoBehaviour
{

    PhotonView view;

    public List<Transform> checkedItems;
    public Transform[] worldItems;


    // Start is called before the first frame update
    private void Start()
    {
        view = GetComponent<PhotonView>();

        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform seat in allChildren)
        {
            checkedItems.Add(seat.GetComponent<Transform>());
            worldItems = checkedItems.ToArray();
        }
        
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
