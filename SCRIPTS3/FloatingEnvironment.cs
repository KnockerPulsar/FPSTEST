using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


//Used in the grapple test level, responsible for making any object with this component float, or rotate about the x,y and z axes or any combination.
public class FloatingEnvironment : MonoBehaviour
{
    public AnimationCurve curve;    //Used to animate the object in a sine wave on the y axis.
    float currentTime = 0;          //Used for calculating the floating position from the curve, starts at a random time on the curve.
    int posOrRot = 0;               //Used to determine whether the object will float or rotate.
    float rotX, rotY, rotZ;         //Storing the initial rotation values.
    float multiplier = 10f;         //A multiplier used to scale the rotation of the object.
    float scaledFixedDT = 0;        //a scaled version of Time.fixedDeltaTime, used for updating the object's rotation.


    //Generates a random integer between 0 and 7
    //If it's equal to zero, the object will float.
    //Otherwise it will rotate.
    //Initializes the variable's based on the generated number.
    void Start()
    {
        posOrRot = Random.Range(0, 8);
         
        if(posOrRot == 0)
            currentTime = Random.Range(0, 5);
        else
        {
            rotX = transform.rotation.x;
            rotY = transform.rotation.y;
            rotZ = transform.rotation.z;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        scaledFixedDT = Time.fixedDeltaTime * multiplier;

        //Changes the rotation based on the generated number.
        //Can be written in many other ways, but this is the clearest.
        //1,4,5,7 rotX
        //2,4,6,7 rotY
        //3,5,6,7 rotZ
        switch (posOrRot)
        {
            case 1:
                {
                    rotX += scaledFixedDT;
                    break;
                }
            case 2:
                {
                    rotY += scaledFixedDT;
                    break;
                }
            case 3:
                {
                    rotZ += scaledFixedDT;
                    break;
                }
            case 4:
                {
                    rotX += scaledFixedDT;
                    rotY += scaledFixedDT;
                    break;
                }
            case 5:
                {
                    rotX += scaledFixedDT;
                    rotZ += scaledFixedDT;
                    break;
                }
            case 6:
                {
                    rotY += scaledFixedDT;
                    rotZ += scaledFixedDT;
                    break;
                }
            case 7:
                {
                    rotX += scaledFixedDT;
                    rotY += scaledFixedDT;
                    rotZ += scaledFixedDT;
                    break;
                }
        }


        if (posOrRot == 0)
            transform.position += new Vector3(0, curve.Evaluate(currentTime) / 10f, 0);
        else if (posOrRot > 0)
            transform.rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
    }
}
