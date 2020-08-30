using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The base state class, any created state inherits from here.
public interface StateInterface
{
    //Should be called every frame on the state
    void Tick();

    //For setup.
    void OnEnter();

    //For cleanup.
    void OnExit();
}
