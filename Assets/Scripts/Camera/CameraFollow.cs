using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    
    [SerializeField]
    public float smoothSpeed;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
        transform.position = smoothPos;
    }
}
