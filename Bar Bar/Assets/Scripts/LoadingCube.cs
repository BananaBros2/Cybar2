using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCube : MonoBehaviour
{
    int spinDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        int randNumber = Random.Range(1, 101);
        if (randNumber == 100)
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        print(randNumber);
        if (randNumber == 1)
            spinDirection = -1;


    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 0.5f * spinDirection, transform.rotation.eulerAngles.z);
    }
}
