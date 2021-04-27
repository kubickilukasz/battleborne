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

    [Header("Explosion")]
    [SerializeField]
    private GameObject explosion;

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

       // transform.Rotate(transform.rotation.x,transform.rotation.y,0);

    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag != "Ammo")
        {
            explosion.GetComponent<ParticleSystem>().Play();
            GetComponent<Renderer>().enabled = false;
        }
    }
}
