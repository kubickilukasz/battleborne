using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvisibleWall : MonoBehaviour
{

    public UnityEvent onJetExitEvent;
    public UnityEvent onJetEnterEvent;

    private bool start;

    void Start()
    {
        start = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Jet" && !start)
        {
            onJetEnterEvent.Invoke();
            Debug.Log("Trigger enter");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Jet")
        {
            onJetExitEvent.Invoke();
            Debug.Log("Trigger exit");
            start = false;
        }    
    }
}
