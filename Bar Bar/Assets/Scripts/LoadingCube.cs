using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 0.4f, transform.rotation.eulerAngles.z);
    }
}
