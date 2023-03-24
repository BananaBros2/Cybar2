using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class ItemData : MonoBehaviour
{

    public string drinkType1 = "Empty";
    public string drinkType2 = "Empty";
    public string drinkType3 = "Empty";
    public bool ice = false;

    public List<string> ingredientsList;


    public bool beingHeld = false;

    Color blank = new Color(0, 0, 0);

    Color vodka = new Color(255, 255, 255);
    Color orange = new Color(255, 81, 0);
    Color cranberry = new Color(255, 0, 0);
    Color grapefruit = new Color(255, 134, 0);
    Color pineapple = new Color(239, 213, 94);
    Color finalColour;

    string drinkName;

    private void Update()
    {
        Color GetColor(string flavour)
        {
            if (flavour == "Empty") { return blank; }

            if (flavour == "Vodka") { return vodka; }
            else if(flavour == "Orange") { return orange; }
            else if (flavour == "Cranberry") { return cranberry; }
            else if (flavour == "Grapefruit") { return grapefruit; }
            else if (flavour == "Pineapple") { return pineapple; }

            return vodka;
        }

        Color colour1 = GetColor(drinkType1);
        Color colour2 = GetColor(drinkType2);
        Color colour3 = GetColor(drinkType3);
        if(colour1 == blank)
        {
            finalColour = new Color(0.85f, 0.85f, 0.85f);
            print("1");
        }
        else if (colour2 == blank)
        {
            finalColour = colour1 / 255;
            print("2");
        }
        else if (colour3 == blank)
        {
            finalColour = new Color((colour1.r + colour2.r) / 2 / 255, (colour1.g + colour2.g) / 2 / 255, (colour1.b + colour2.b) / 2 / 255);
            print("3");
        }
        else if (colour3 != blank)
        {
            finalColour = new Color((colour1.r + colour2.r + colour3.r) / 3 / 255, (colour1.g + colour2.g + colour3.g) / 3 / 255, (colour1.b + colour2.b + colour3.b) / 3 / 255);
            print("4");
        }
        
        gameObject.GetComponent<Renderer>().material.color = finalColour;

        ingredientsList = new List<string> { drinkType1, drinkType2, drinkType3 };

        ingredientsList.Sort((x, y) => string.Compare(x, y));
    }

}
