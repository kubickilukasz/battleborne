using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvisibleWall : MonoBehaviour
{

    public UnityEvent onJetExitEvent;
    public UnityEvent onJetEnterEvent;

#region Timer
    [SerializeField]
    private float timerSeconds;

    private float timer = 0.0f;
#endregion

    private bool outOfBounds = false;

    private GameObject jet;


    void Update()
    {
        Timer();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Jet" && outOfBounds)
        {
            onJetEnterEvent.Invoke();
            outOfBounds = false;
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
                    outOfBounds = false;
                }
            }   
        }
        else
        {
            timer = 0.0f;
        }
    }
}
