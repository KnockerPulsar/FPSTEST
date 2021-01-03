using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Door : MonoBehaviour
{
    public DissolveMatScript dissolveScript;
    Collider coll;


    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
    }


    public void Close()
    {
        if (coll)
            coll.isTrigger = false;
        if (dissolveScript)
            dissolveScript.StartDissolvingIn();
    }

    public void Open()
    {
        if (coll)
            coll.isTrigger = true;
        if (dissolveScript)
            dissolveScript.StartDissolvingOut();
    }
}
