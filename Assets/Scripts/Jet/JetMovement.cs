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
    private float defaultSpeed;
    private float speed;
    [SerializeField]
    private float maxSpeed;
    
    [SerializeField]
    private float horizontalTurn;
    
    [SerializeField] 
    private float verticalTurn;
    

    [Header("Physics")]
    [SerializeField]
    private float speedDelta;

    Rigidbody rigidbody;

    #region Timer

    private float timer = 0f;
    [Header("Fuel")]
    [SerializeField]
    private float timerMax = 0.05f;

    #endregion
    private JetShooting fuel;


    private bool isBoosting = false;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        fuel = GetComponent<JetShooting>();
    }

    
    void Update()
    {
        Boost();
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

    private void Boost()
    {
        if(Input.GetMouseButton(1))
        {
            if(fuel.GetAmmo() > 0)
            {
                if(speed < maxSpeed)
                {
                    speed+=speedDelta;
                }
                else speed = maxSpeed;
                isBoosting = true;
                FuelDecrease();
            }
            else
            {
                if(speed > defaultSpeed)
                    speed -= speedDelta;
                else speed = defaultSpeed;
                isBoosting = false;
            }
        }
        else
        {
            if(speed > defaultSpeed)
                speed -= speedDelta;
            else speed = defaultSpeed;
            isBoosting = false;
        }
    }

#region PublicMethods

    public bool IsBoosting()
    {
        return isBoosting;
    }

#endregion

#region PrivateMethods
    private void FuelDecrease()
    {
        if(timer < timerMax)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            fuel--;
        }
    }
#endregion
}
