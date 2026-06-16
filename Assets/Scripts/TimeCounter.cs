using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    private TMP_Text timeUI;
    private float startTime;
    private float elapsedTime;
    private bool startCounter;

    private int minutes;
    private int seconds;

    void Start()
    {
        startCounter = false;
        timeUI = GetComponent<TMP_Text>();
    }

    public void StartTimeCounter()
    {
        startTime = Time.time;
        startCounter = true;
    }

    public void StopTimeCounter()
    {
        startCounter = false;
    }

    void Update()
    {
        if (startCounter)
        {
            elapsedTime = Time.time - startTime;

            minutes = (int)(elapsedTime / 60);
            seconds = (int)(elapsedTime % 60);

            timeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
