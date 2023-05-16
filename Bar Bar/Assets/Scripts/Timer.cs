using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEditor;

public class Timer : MonoBehaviour
{
    public float gameTime = 0;
    TextMeshProUGUI textString;
    public Light light;

    public GameObject roomLights;
    // Start is called before the first frame update
    void Start()
    {
        textString = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        roomLights.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        gameTime += Time.deltaTime;

        if (gameTime > 240) { textString.text = "END"; }
        else if (gameTime > 220) { textString.text = "1AM"; }
        else if (gameTime > 200) { textString.text = "12AM"; }
        else if (gameTime > 180) { textString.text = "11PM"; }
        else if (gameTime > 160) { textString.text = "10PM"; }
        else if (gameTime > 140) { roomLights.SetActive(true); }
        else if (gameTime > 140) { textString.text = "9PM"; }
        else if (gameTime > 120) { textString.text = "8PM"; }
        else if (gameTime > 100) { textString.text = "7PM"; }
        else if (gameTime > 80) { textString.text = "6PM"; }
        else if (gameTime > 60) { textString.text = "5PM"; }
        else if (gameTime > 40) { textString.text = "4PM"; }
        else if (gameTime > 20) { textString.text = "3PM"; }
        else if (gameTime > 0) { textString.text = "2PM"; }

        if (gameTime > 120 && gameTime <= 180)
        {

            RenderSettings.ambientIntensity = 1 - ( gameTime - 120)/60;
            light.intensity = Mathf.Max(1 - (gameTime - 120) / 60, 0.15f);
        }

    }
}
