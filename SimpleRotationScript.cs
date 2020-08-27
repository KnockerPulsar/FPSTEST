using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for rotating the object this script is on.
public class SimpleRotationScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(Time.deltaTime * 10, Vector3.up) * transform.localRotation;
    }
}
