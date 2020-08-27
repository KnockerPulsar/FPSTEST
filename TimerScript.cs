using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Responsible for keeping track of the time since spawning and showing it in the ui
public class TimerScript : MonoBehaviour
{
    public PlayerVariables pVars;           //A container responsible for storing commonly used / shared variables.
    public TextMeshProUGUI timerText;       //The time UI component.
    float seconds = 0, minutes = 0;         //Self explanatory.


    // Update is called once per frame
    //Calculates the current second and minutes through the pVars variable, then displays them in the UI.
    void Update()
    {
        seconds = Mathf.RoundToInt(pVars.secondsSinceLevelStart % 59);
        minutes = (int)pVars.secondsSinceLevelStart / 60;
            
        timerText.SetText("Time: " + minutes + ":" + seconds);
    }
}
