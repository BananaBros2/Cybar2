using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class loadingBar : MonoBehaviour
{
    public float fillAmount = 0f;

    private Camera cam;
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        progressBar = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.fillAmount = fillAmount;

        transform.LookAt(new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z));
        
    }
}
