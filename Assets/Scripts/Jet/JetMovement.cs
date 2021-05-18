using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;


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


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {

        rigidbody.velocity = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 torque = new Vector3(-Input.GetAxis("Mouse Y")*verticalTurn,Input.GetAxis("Mouse X")*horizontalTurn, 0);
        rigidbody.AddRelativeTorque(torque * Time.fixedDeltaTime);

    }

    void LateUpdate()
    {
       rigidbody.rotation =
            Quaternion.Lerp(rigidbody.rotation, Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0)), .5f);
    }

}
