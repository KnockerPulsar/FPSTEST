using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for rotating the object this script is on.
public class SimpleRotateFloatScript : MonoBehaviour
{
    public float rotSpeed = 10f;
    public float floatSpeed = 2f;
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(Time.deltaTime * rotSpeed, Vector3.up) * transform.localRotation;

        Vector3 loc = transform.localPosition;
        loc.y += Mathf.Sin(Time.time) / 1000f * floatSpeed;

        transform.localPosition = loc;
    }
}
