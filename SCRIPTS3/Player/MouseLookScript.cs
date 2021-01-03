using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

//Responsible for rotating the player about the Y axis and the player's camera about its local X axis.
public class MouseLookScript : MonoBehaviour
{

    public bool lockCursor = true;                              //Whether to capture the cursor or not?
    public Vector3 targetDir;                                   //Gets the initial orientation of the player.
    public Vector2 sensitivity = new Vector2(1, 1);             //Scaling factor for both X and Y.
    public Vector2 smoothing = new Vector2(3, 3);               //Smoothing factor for both X and Y.
    public Vector2 clampInDeg = new Vector2(360, 180);          //X and Y angle clamps.
    Vector2 smoothMouse;                                        //Smoothed mouse movement.
    Quaternion targetOrientation;                               //Stores the initial rotation on the parent object.
    Vector2 mouseAbsolute;                                      //Absolute/Final mouse angle change after smoothing.


    // Start is called before the first frame update
    void Awake()
    {
        targetOrientation = gameObject.transform.rotation;
        PlayerVariables.playerCamParent = GameObject.Find("Cameras");
        PlayerVariables.playerCam = Camera.main.gameObject;

        //Capturing the cursor.
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        MouseLook();
    }

    private void MouseLook()
    {
        //Fatigued artist's method 
        //https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/

        if (PlayerVariables.paused)
            return;


        //Gets the raw mouse x and y data
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));

        //Scales the given mouse data by the scaling*smoothing
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        //Interpolates both the x and y values between the previous mouse value  and the new given value, based on the smoothing given
        //More smoothing equals a value closer to the previous frame's value
        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        //Adds the new rotation to the old.
        mouseAbsolute += smoothMouse;

        //Clamping calculations based on the given clamp degrees
        if (clampInDeg.x < 360)
            mouseAbsolute.x = Mathf.Clamp(mouseAbsolute.x, -clampInDeg.x / 2, clampInDeg.x / 2);
        if (clampInDeg.y < 360)
            mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampInDeg.y / 2, clampInDeg.y / 2);


        //Modified to work with a parent object + a child PlayerVariables.playerCamera.
        //The parent rotates about the y/up axis with the given mouse x values.
        //The child rotates about the x/right axis with the given mouse y values.
        //We multiply by targetOrientation outside in the parent so that the original start orientation is preserved, otherwise the player's rotation
        //Will always start at (0,0,0) no matter what.
        //We can multiply the "Vector3.up" by the targetOrientaion if we want the play to start with an up axis other than the global y axis.
        PlayerVariables.playerCam.transform.localRotation = Quaternion.AngleAxis(mouseAbsolute.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(mouseAbsolute.x, Vector3.up) * targetOrientation;
    }


}
