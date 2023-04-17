using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSText : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float pollingTime = 1.5f;
    private float time;
    private int frameCount;


    void Update()
    {
        updateFPS();
    }
    private void updateFPS()
    {
        time += Time.deltaTime;
        frameCount++;
        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = "FPS: " + frameRate.ToString();
            time -= pollingTime;
            frameCount = 0;
        }
    }
}
