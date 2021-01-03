using UnityEngine;
public class BaseAIStateMachine : MonoBehaviour
{
    public bool startActive = false;
    public string currentStateName;

    //This is just a base class for when I need to get the state machine of an enemy
    //I haven't thought of any useful thing to put in here.

    //If you have anything in mind to add, please inform me!

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;

    public void Start()
    {
        if (!startActive)
            Deactivate();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true && other.CompareTag("EnemyActivator"))
            Activate();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.isTrigger == true && other.CompareTag("EnemyActivator"))
            Deactivate();
    }
}
