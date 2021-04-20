using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    private Transform target;
    
    [Header("Camera Values")]
    [SerializeField]
    private float smoothSpeed;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float rotSpeed;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
    
        Vector3 desiredPos = target.position - target.TransformDirection(offset);//target.position + offset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
        Quaternion smoothRot = Quaternion.Lerp(transform.rotation, target.rotation,rotSpeed);
        transform.position = smoothPos;
        transform.rotation = smoothRot;

    }
}
