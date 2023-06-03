using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Photon.Pun;

using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class ItemData : MonoBehaviour
{
    public int drinkID;

    public string drinkType1 = "_Empty_";
    public string drinkType2 = "_Empty_";
    public string drinkType3 = "_Empty_";
    public bool ice = false;

    public List<string> ingredientsList;
    public List<string> oldList;

    public bool beingHeld = false;

    Color blank = new Color(0, 0, 0);

    Color vodka = new Color(200, 200, 200);
    Color orange = new Color(255, 81, 0);
    Color cranberry = new Color(255, 0, 0);
    Color grapefruit = new Color(255, 134, 0);
    Color pineapple = new Color(239, 213, 94);
    Color finalColour;

    public PhotonView view;
    bool skip = true;

    public GameObject spill;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        oldList = new List<string> { "_Empty_", "_Empty_", "_Empty_" };
        if (drinkType1 == "_Empty_") 
        {
            transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        }
        
    }

    private void Update()
    {

        transform.GetChild(0).GetComponent<Renderer>().material.color = finalColour;
        ingredientsList = new List<string> { drinkType1, drinkType2, drinkType3 };
        ingredientsList.Sort((x, y) => string.Compare(x, y));

        if (oldList.SequenceEqual(ingredientsList))
        {
            return;
        }
        oldList = ingredientsList;

        if (ingredientsList.SequenceEqual(new List<string> { "_Empty_", "_Empty_", "_Empty_" })) { transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false; }
        else { transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true; }

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
        // Unknown
        else { drinkID = -1; }

        
        transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(finalColour.r, finalColour.g, finalColour.b);

        Color GetColor(string flavour)
        {
            if (flavour == "_Empty_") { return blank; }

            if (flavour == "Vodka") { return vodka; }
            else if (flavour == "Orange") { return orange; }
            else if (flavour == "Cranberry") { return cranberry; }
            else if (flavour == "Grapefruit") { return grapefruit; }
            else if (flavour == "Pineapple") { return pineapple; }

            return vodka;
        }

        Color colour1 = GetColor(drinkType1);
        Color colour2 = GetColor(drinkType2);
        Color colour3 = GetColor(drinkType3);
        if (colour1 == blank)
        {
            finalColour = new Color(0.85f, 0.85f, 0.85f, 200);
        }
        else if (colour2 == blank)
        {
            finalColour = new Color(colour1.r / 255, colour1.g / 255, colour1.b / 255, 200);
        }
        else if (colour3 == blank)
        {
            finalColour = new Color((colour1.r + colour2.r) / 2 / 255, (colour1.g + colour2.g) / 2 / 255, (colour1.b + colour2.b) / 2 / 255, 200);
        }
        else if (colour3 != blank)
        {
            finalColour = new Color((colour1.r + colour2.r + colour3.r) / 3 / 255, (colour1.g + colour2.g + colour3.g) / 3 / 255, (colour1.b + colour2.b + colour3.b) / 3 / 255, 200);
        }
        PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
        view.RPC("RPC_ValueChanges", RpcTarget.OthersBuffered, drinkType1, drinkType2, drinkType3, finalColour.r, finalColour.g, finalColour.b, beingHeld);
    }


    [PunRPC]
    void RPC_ValueChanges(string RPCType1, string RPCType2, string RPCType3, float RPCred, float RPCgreen, float RPCblue, bool RPCheld)
    {
        drinkType1 = RPCType1;
        if (drinkType1 != "_Empty_") { transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true; }
        drinkType2 = RPCType2;
        drinkType3 = RPCType3;
        finalColour = new Color(RPCred, RPCgreen, RPCblue, 200);
        beingHeld = RPCheld;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Activates if the glass collides with another object at a magnitude of more than 8
        // Throwing a glass will normally result in it breaking, but just dropping it normally won't
        if (collision.relativeVelocity.magnitude > 8)
        {
            // Checks if the Glass has any contents before spawning a puddle
            if (!ingredientsList.SequenceEqual(new List<string> { "_Empty_", "_Empty_", "_Empty_" }))
            {
                // Projects a ray downwards 20 units that will only return true if colliding with a object in the 'Floor' layer
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 20, (1 << LayerMask.NameToLayer("Floor"))))
                {

                    // Creates the puddle prefab at the location of where the raycast hit.
                    view.RPC("RPC_SpillInstantiate", RpcTarget.MasterClient, hit.point);


                }
            }

            view.RPC("RPC_UpdateLists", RpcTarget.All);
            view.RPC("RPC_Delete", RpcTarget.MasterClient);

            
        }
    }

    [PunRPC]
    void RPC_SpillInstantiate(Vector3 RPCPosition)
    {
        GameObject instantiatedObject = PhotonNetwork.Instantiate(spill.name, RPCPosition, Quaternion.identity);
        view.RPC("RPC_Spill", RpcTarget.All, instantiatedObject.GetPhotonView().ViewID);
    }

    [PunRPC]
    void RPC_Spill(int RPCID)
    {
        var decalProjector = PhotonView.Find(RPCID).transform.GetChild(0).GetComponent<DecalProjector>();

        // Creates a new material that copies the old's properties
        // Using the old one would result in all puddles copying any tweaks done in script
        decalProjector.material = Instantiate<Material>(decalProjector.material);

        // Uses a custom made color decal property to mix the image with the current colour of the glass contents
        decalProjector.material.color = (finalColour);
    }

    [PunRPC]
    void RPC_UpdateLists()
    {
        // Runs the following code per every object tagged as a player
        foreach (GameObject players in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.GetComponent<PlayerController>().checkedItems.Remove(this.transform);
            players.GetComponent<PlayerController>().worldItems = players.GetComponent<PlayerController>().checkedItems.ToArray();
        }
    }

    [PunRPC]
    void RPC_Delete()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

}
