using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    float gameTime = 0;
    TextMeshProUGUI textString;

    // Start is called before the first frame update
    void Start()
    {
        textString = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        
        if (gameTime > 275) { textString.text = "1AM"; }
        else if (gameTime > 250) { textString.text = "12AM"; }
        else if (gameTime > 225) { textString.text = "11PM"; }
        else if (gameTime > 200) { textString.text = "10PM"; }
        else if (gameTime > 175) { textString.text = "9PM"; }
        else if (gameTime > 150) { textString.text = "8PM"; }
        else if (gameTime > 125) { textString.text = "7PM"; }
        else if (gameTime > 100) { textString.text = "6PM"; }
        else if (gameTime > 75) { textString.text = "5PM"; }
        else if (gameTime > 50) { textString.text = "4PM"; }
        else if (gameTime > 25) { textString.text = "3PM"; }
        else if (gameTime > 0) { textString.text = "2PM"; }
    }
}
