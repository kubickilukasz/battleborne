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
    public float speed;
    [SerializeField]
    public float horizontalTurn;
    [SerializeField] 
    public float verticalTurn;

    [Header("Physics")]
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        
        rigidbody.velocity = transform.forward * speed;
        Vector3 torque = new Vector3(Input.GetAxis("Mouse Y")*verticalTurn,Input.GetAxis("Mouse X")*horizontalTurn, 0);
        rigidbody.AddRelativeTorque(torque * Time.fixedDeltaTime);

    }
}
