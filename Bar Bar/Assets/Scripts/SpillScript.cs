using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(DecalProjector))]
public class SpillScript : MonoBehaviour
{
    //public PhotonView view;
    public List<Texture> spillImages;

    private void Start()
    {

        transform.rotation = Quaternion.Euler(0, Random.RandomRange(0, 4) * 90, 0);

        //var decalProjector = transform.GetChild(0).GetComponent<DecalProjector>();
        //print(transform.GetChild(0).GetComponent<DecalProjector>().material.);
        //transform.GetChild(0).GetComponent<DecalProjector>().material.SetTexture("_BaseMap", spillImages[Random.RandomRange(0, spillImages.Count)]);
        
    }

}
