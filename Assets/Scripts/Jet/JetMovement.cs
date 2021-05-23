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
    private float defaultSpeed; /// Default forwarding speed of the jet
    private float speed;
    [SerializeField]
    private float maxSpeed; /// Max forwarding speed of the jet
    
    [SerializeField]
    private float horizontalTurn; /// Speed of horizontal turning
    
    [SerializeField] 
    private float verticalTurn; /// Speed of vertical turning

    [SerializeField]
    private AudioSource jetMovement; /// Sound of jet engines
    

    [Header("Physics")]
    [SerializeField]
    private float speedDelta; /// Delta of forwarding speed (increment for boosting)

    Rigidbody rigidbody;

    #region Timer

    private float timer = 0f;
    [Header("Fuel")]
    [SerializeField]
    private float timerMax = 0.05f; /// Timer for ammo waste. This value prevents ammo amount from decreasing too fast

    #endregion
    private JetShooting fuel;


    private bool isBoosting = false;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        fuel = GetComponent<JetShooting>();
        jetMovement.Play();
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


    /**
    Method used for boosting speed of jet. In order to do that right mouse button needs to be held(provided that amount of fuel is positive)
    */
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

    /**
    Method returning whether jet is currently boosting or not
    return Returns true if jet is boosting, false otherwise
    */
    public bool IsBoosting()
    {
        return isBoosting;
    }

#endregion

#region PrivateMethods
    /**
    Method that decreases the amount of fuel based on the timer
    */
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
