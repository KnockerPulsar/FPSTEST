using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Responsible for rotating the question marks and showing text when the pVars.player is close enough and hiding it when the pVars.player goes far enough.
public class TutorialScript : MonoBehaviour
{
    public PlayerVariables pVars;    //A container responsible for storing commonly used / shared variables.
    [TextArea(20, 20)]                //Specifies the area this string takes up in the inspector.
    public string TutorialText;      //The text that is viewed when the pVars.player is close enough.
    Canvas prefabCanvas;             //The canvas on which the text is shown. (Worldview)
    TextMeshPro TMP;                 //The text component in the canvas.
    bool active = false;             //Used to keep track of whether to view or hide the text and rotate.
    Vector3 newFwdDir;               //Used to rotate the question mark towards the player.
    float slerpPercentage;           //Used to spherically interpolate between the current rotation and the new rotation.

    // Start is called before the first frame update    
    void Start()
    {
        prefabCanvas = transform.GetChild(0).GetComponent<Canvas>();
        TMP = prefabCanvas.GetComponent<TextMeshPro>();
        TMP.enabled = false;
    }

    // Update is called once per frame
    //If the player hasn't yet overlapped the hit sphere, the object will continue rotating.
    //Otherwise, the object will smoothly rotate towards the player.
    //And if the player is further away than 40 meters, makes the object inactive.
    void Update()
    {
        if (!active)
            transform.localRotation = Quaternion.AngleAxis(0.1f, Vector3.up) * transform.localRotation;
        else
        {
            if (slerpPercentage < 1)
                slerpPercentage += Time.deltaTime / 2f;

            float distanceFromplayer = Vector3.Distance(pVars.player.transform.position, transform.position);
            newFwdDir = pVars.player.transform.position - transform.position;
            newFwdDir.y = 0;
            newFwdDir.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newFwdDir, Vector3.up), slerpPercentage);

            if (distanceFromplayer > 40f)
            {
                slerpPercentage = 0;
                active = false;
                TMP.enabled = false;
            }
        }
    }

    //If the player overlaps the hit sphere, activates the object and the attached text.
    private void OnTriggerEnter(Collider other)
    {
        active = true;

        if (TMP != null)
        {
            TMP.enabled = true;
            TMP.SetText(TutorialText);
        }
        else
            print("CAN'T GET TMP COMPONENT");
    }
}
