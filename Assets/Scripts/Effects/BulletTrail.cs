using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Controls trail effect on bullets
*/
public class BulletTrail : MonoBehaviour
{

    /**
    * Scale time of trail
    */
    [SerializeField]
    private float scaleTime;

    /**
    * Reference to TrailRenderer
    */
    [SerializeField]
    private TrailRenderer trailRenderer;

    /**
    * Speed of the trail
    */
    private float speed;

    void Start()
    {
        if(trailRenderer == null)
        {
            trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        SetSpeed(1);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        trailRenderer.time = 1 / this.speed * scaleTime;
    }

    public float GetCurrentSpeed()
    {
        return speed;
    }

    public float GetTrailTime()
    {
        return trailRenderer.time;
    }

}
