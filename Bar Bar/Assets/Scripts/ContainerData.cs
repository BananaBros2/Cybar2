using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContainerData : MonoBehaviour
{
    public string contents = "Vodka";
    public int count;
    public int capacity = 3;

    private Camera cam;
    public Image progressBar;

    private void Start()
    {
        count = capacity;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        progressBar = transform.GetChild(0).GetComponentInChildren<Image>();
        progressBar.fillAmount = count / capacity;
    }

    private void Update()
    {
        progressBar.fillAmount = (float)count / (float)capacity;
        print(count / capacity);

        transform.GetChild(0).LookAt(new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z));

    }
}
