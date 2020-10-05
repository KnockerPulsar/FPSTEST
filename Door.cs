using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Door : MonoBehaviour
{
    Collider collider;
    DissolveMatScript dissolveScript;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        dissolveScript = GetComponent<DissolveMatScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        collider.isTrigger = false;
        dissolveScript.StartDissolvingIn();
    }

    public void Deactivate()
    {
        collider.isTrigger = true;
        dissolveScript.StartDissolvingOut();
    }
}
