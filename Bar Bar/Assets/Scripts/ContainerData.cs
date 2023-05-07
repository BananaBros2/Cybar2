using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adds tools needed for the UI
using UnityEngine.UI;

public class ContainerData : MonoBehaviour
{
    // Variables for determining the contents of a container.
    public string contents = "Vodka";
    public int capacity = 3;
    public int count;

    // Variables for storing data on the scene camera and 
    private Camera cam;
    public Image image;

    private void Start()
    {
        // Sets the contents to the max value on start.
        count = capacity;

        // Stores the camera as a field that can be changed later.
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // Stores the second image in the canvas as field that will be changed later.
        image = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        // Sets the fillAmount of the image to full.
        image.fillAmount = 1;
    }

    
    private void Update()
    {
        if(transform.GetComponent<Rigidbody>().velocity.magnitude <= 0.01f)
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        // Sets the fillAmount to the capacity of the container by dividing the current count by the capacity
        image.fillAmount = (float)count / (float)capacity;

        // Changes the rotation of the canvas to face towards the camera 
        transform.GetChild(0).LookAt(new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z));
    }
}
