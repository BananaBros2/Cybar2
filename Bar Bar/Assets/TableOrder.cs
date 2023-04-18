using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableOrder : MonoBehaviour
{
    public int desiredDrink = -2;
    public int seatID;
    public bool satOn;
    private bool drinkChosen = false;
    public bool orderRecieved;

    public Sprite[] drinkImages;

    //Set these Textures in the Inspector
    public Texture m_MainTexture, m_Normal, m_Metal;
    Renderer m_Renderer;

    private void Start()
    {
        seatID = transform.GetSiblingIndex();
        print(seatID);

        //Fetch the Renderer from the GameObject
        m_Renderer = transform.GetChild(0).GetComponent<Renderer>();

        //Make sure to enable the Keywords
        m_Renderer.material.EnableKeyword("_NORMALMAP");
        m_Renderer.material.EnableKeyword("_METALLICGLOSSMAP");

        //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        m_Renderer.material.SetTexture("_MainTex", m_MainTexture);
        //Set the Normal map using the Texture you assign in the Inspector
        m_Renderer.material.SetTexture("_BumpMap", m_Normal);
        //Set the Metallic Texture as a Texture you assign in the Inspector
        m_Renderer.material.SetTexture("_MetallicGlossMap", m_Metal);
    }

    private void Update()
    {
        if (satOn && drinkChosen == false)
        {
            desiredDrink = Random.RandomRange(1, 7);
            drinkChosen = true;
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = drinkImages[desiredDrink];
        }
    }
    
}