using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class JetMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float horizontalTurn;
    [SerializeField] 
    private float verticalTurn;
    
    [Header("Physics")]
    Rigidbody rigidbody;

    [Header("Rotation")] 
    public Transform model;

    [SerializeField] 
    public float leanValue;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        
        rigidbody.velocity = transform.forward * speed;
        Vector3 torque = new Vector3(-Input.GetAxis("Mouse Y")*verticalTurn,Input.GetAxis("Mouse X")*horizontalTurn, 0);
        rigidbody.AddRelativeTorque(torque * Time.fixedDeltaTime);
        //model.localEulerAngles = new Vector3(Input.GetAxis("Mouse Y") * leanValue
        //    , model.localEulerAngles.y, Input.GetAxis("Mouse X") * leanValue);
        
    }
}
