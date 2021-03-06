﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Responsible for keeping track of the time since spawning and showing it in the ui
public class TimerScript : MonoBehaviour
{
                //A container responsible for storing commonly used / shared variables.
    public TextMeshProUGUI timerText;       //The time UI component.
    string seconds, minutes;         //Self explanatory.


    // Update is called once per frame
    //Calculates the current second and minutes through the PlayerVariables variable, then displays them in the UI.
    void Update()
    {
        seconds = Mathf.RoundToInt(PlayerVariables.secondsSinceLevelStart % 59).ToString("00");
        minutes = ((int)PlayerVariables.secondsSinceLevelStart / 60).ToString("00");

        timerText.SetText("Time: " + minutes + ":" + seconds);
    }
}
