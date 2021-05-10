using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvisibleWall : MonoBehaviour
{

    public UnityEvent onJetExitEvent;
    public UnityEvent onJetEnterEvent;

    [SerializeField]
    private float timerSeconds;

    private float timer = 0.0f;
    private bool start;

    private bool outOfBounds = false;

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
