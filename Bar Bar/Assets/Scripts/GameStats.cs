using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameStats : MonoBehaviour
{
    public GameObject canvas;
    private GameObject scoreText;
    public GameObject endScreen;

    public GameObject resultsList;

    PhotonView view;

    public int levelScore = 0;
    private int oldScore;

    public int served;
    public int missed;
    public int spent;
    public int messCount;
    public int total;

    public bool gameEnded;

    void Start()
    {
        scoreText = canvas.transform.GetChild(2).gameObject;
        oldScore = levelScore;

        view = GetComponent<PhotonView>();

        Time.timeScale = 1;
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

        print($"Served {served} | Missed {missed} | Spent | {spent}");

        if (gameEnded)
        {
            Time.timeScale = 0;
            gameEnded = false;

            GameObject[] messyObjects = GameObject.FindGameObjectsWithTag("Container");
            messCount = messyObjects.Length;
            messyObjects = GameObject.FindGameObjectsWithTag("Spillage");
            messCount = messyObjects.Length;

            resultsList.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ($"Total Served: {served} (x30)");
            resultsList.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ($"Orders Missed: {missed} -(x10)");
            resultsList.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ($"Money Spent: {spent*-1}");
            resultsList.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = ($"Mess Penalty: {messCount} -(x5)");

            total = served * 30 - (missed * 10 + spent + messCount * 5);
            resultsList.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = ($"Final Score: {total}");
        }
        
    }

    [PunRPC]
    void RPC_ValueChanges(int RPCscore)
    {
        levelScore = RPCscore;
    }

}
