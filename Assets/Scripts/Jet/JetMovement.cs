using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class JetMovement : MonoBehaviour
{
    [SerializeField]
    public float speed;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * speed;
        Vector3 torque = new Vector3(Input.GetAxis("Vertical")*10, Input.GetAxis("Horizontal")*10, 0);
        rigidbody.AddRelativeTorque(torque * Time.fixedDeltaTime);
        rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0)), .5f);

    }
}
