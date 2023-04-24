using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameStats : MonoBehaviour
{
    public GameObject canvas;
    private GameObject scoreText;

    PhotonView view;

    public int levelScore = 0;
    private int oldScore;

    void Start()
    {
        scoreText = canvas.transform.GetChild(1).gameObject;
        oldScore = levelScore;

        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldScore != levelScore)
        {
            scoreText.GetComponent<TextMeshProUGUI>().text = levelScore.ToString();
            oldScore = levelScore;
            PhotonNetwork.RemoveBufferedRPCs(view.ViewID, "RPC_ValueChanges");
            view.RPC("RPC_ValueChanges", RpcTarget.OthersBuffered, levelScore);
        }
        
    }

    [PunRPC]
    void RPC_ValueChanges(int RPCscore)
    {
        levelScore = RPCscore;
    }

}
