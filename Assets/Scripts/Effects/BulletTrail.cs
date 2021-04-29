using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{

    [SerializeField]
    private float scaleTime;

    [SerializeField]
    private TrailRenderer trailRenderer;

    // Ustawiane przez zewnętrzne skrypty
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
