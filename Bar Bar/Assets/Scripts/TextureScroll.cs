using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public float YScroll = 0.01f;
    public float XScroll = 0.01f;

    public MeshRenderer objectMesh;


    void Start()
    {
        objectMesh = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        objectMesh.material.mainTextureOffset = new Vector2(objectMesh.material.mainTextureOffset.x + XScroll* Time.deltaTime, objectMesh.material.mainTextureOffset.y + YScroll * Time.deltaTime);
    }
}
