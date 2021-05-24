using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
Class that represents the border of the map and performs certain actions when it's crossed
*/
public class InvisibleWall : MonoBehaviour
{

    /// Event invoked when jet leaves the area
    public UnityEvent onJetExitEvent; 
    /// Event invoked when jet enters the area
    public UnityEvent onJetEnterEvent;

#region Timer
    [SerializeField]
    /// Amount of seconds for the selfdestruction countdown
    private float timerSeconds;

    private float timer = 0.0f;
#endregion

    private bool outOfBounds = false;
    private bool start;

    private GameObject jet;


    void Start()
    {
        start = true;
    }

    void Update()
    {
        Timer();
    }

    void OnTriggerEnter(Collider other)
    {
        if((other.tag == "Jet" && outOfBounds) || start)
        {
            onJetEnterEvent.Invoke();
            outOfBounds = false;
            start = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Jet")
        {
            onJetExitEvent.Invoke();
            outOfBounds = true;
            jet = other.gameObject;
        }    
    }

    public float GetTimer(){
        return timer;
    }

    public float GetTimerSeconds(){
        return timerSeconds;
    }


    /**
    Method used for counting time until selfdestruction of jet that left the map
    */
    private void Timer()
    {
        if(outOfBounds && jet != null)
        {
            timer += Time.deltaTime;
            if(timer >= timerSeconds)
            {
                JetHealth jetHealth = jet.GetComponent<JetHealth>();
                if(jetHealth != null)
                {
                    int hitPoints = jetHealth.GetMaxHealth();
                    jetHealth.OnHit(hitPoints);
                }
            }   
        }
        else
        {
            timer = 0.0f;
        }
    }
}
