using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class ItemData : MonoBehaviour
{
    public int drinkID;

    public string drinkType1 = "_Empty_";
    public string drinkType2 = "_Empty_";
    public string drinkType3 = "_Empty_";
    public bool ice = false;

    public List<string> ingredientsList;

    public bool beingHeld = false;

    Color blank = new Color(0, 0, 0);

    Color vodka = new Color(200, 200, 200);
    Color orange = new Color(255, 81, 0);
    Color cranberry = new Color(255, 0, 0);
    Color grapefruit = new Color(255, 134, 0);
    Color pineapple = new Color(239, 213, 94);
    Color finalColour;

    private void Update()
    {
        ingredientsList = new List<string> { drinkType1, drinkType2, drinkType3 };

        ingredientsList.Sort((x, y) => string.Compare(x, y));

        // BASE DRINKS
        if (ingredientsList.SequenceEqual(new List<string> { "_Empty_", "Grapefruit", "Vodka" })) { drinkID = 1; }          // Greyhound
        else if (ingredientsList.SequenceEqual(new List<string> { "_Empty_", "Orange", "Vodka" })) { drinkID = 2; }         // Screwdriver
        else if (ingredientsList.SequenceEqual(new List<string> { "_Empty_", "Cranberry", "Vodka" })) { drinkID = 3; }      // Cape Codder
        else if (ingredientsList.SequenceEqual(new List<string> { "Cranberry", "Grapefruit", "Vodka" })) { drinkID = 4; }   // Sea Breeze
        else if (ingredientsList.SequenceEqual(new List<string> { "Cranberry", "Orange", "Vodka" })) { drinkID = 5; }       // Madras
        else if (ingredientsList.SequenceEqual(new List<string> { "Cranberry", "Pineapple", "Vodka" })) { drinkID = 6; }    // Bay Breeze
        // ALTERNATIVE DRINKS 
        else if (ingredientsList.SequenceEqual(new List<string> { "Grapefruit", "Grapefruit", "Vodka" })) { drinkID = 7; }  // Weak Greyhound
        else if (ingredientsList.SequenceEqual(new List<string> { "Grapefruit", "Vodka", "Vodka" })) { drinkID = 8; }       // Strong Greyhound
        else if (ingredientsList.SequenceEqual(new List<string> { "Orange", "Orange", "Vodka" })) { drinkID = 9; }         // Weak Screwdriver
        else if (ingredientsList.SequenceEqual(new List<string> { "Orange", "Vodka", "Vodka" })) { drinkID = 10; }         // Strong Screwdriver
        else if (ingredientsList.SequenceEqual(new List<string> { "Cranberry", "Cranberry", "Vodka" })) { drinkID = 11; }      // Weak Cape Codder
        else if (ingredientsList.SequenceEqual(new List<string> { "Cranberry", "Vodka", "Vodka" })) { drinkID = 12; }      // Strong Cape Codder

        else { drinkID = -1; }                                                                                              // Unknown

        gameObject.GetComponent<Renderer>().material.color = finalColour;

        Color GetColor(string flavour)
        {
            if (flavour == "_Empty_") { return blank; }

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
            //print("1");
        }
        else if (colour2 == blank)
        {
            finalColour = colour1 / 255;
            //print("2");
        }
        else if (colour3 == blank)
        {
            finalColour = new Color((colour1.r + colour2.r) / 2 / 255, (colour1.g + colour2.g) / 2 / 255, (colour1.b + colour2.b) / 2 / 255);
            //print("3");
        }
        else if (colour3 != blank)
        {
            finalColour = new Color((colour1.r + colour2.r + colour3.r) / 3 / 255, (colour1.g + colour2.g + colour3.g) / 3 / 255, (colour1.b + colour2.b + colour3.b) / 3 / 255);
            //print("4");
        }
        

    }

}
