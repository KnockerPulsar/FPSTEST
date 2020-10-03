using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for rotating the object this script is on.
public class SimpleRotationScript : MonoBehaviour
{
    public float rotSpeed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(Time.deltaTime * rotSpeed, Vector3.up) * transform.localRotation;
    }
}
