using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
/**
Class that stores data and performs actions reffering to jet's movement 
*/
public class JetMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    /// Default forwarding speed of the jet
    private float defaultSpeed;
    private float speed;
    [SerializeField]
    /// Max forwarding speed of the jet
    private float maxSpeed;
    
    [SerializeField]
    /// Speed of horizontal turning
    private float horizontalTurn;
    
    [SerializeField] 
    /// Speed of vertical turning
    private float verticalTurn;

    [SerializeField]
    /// Sound of jet engines
    private AudioSource jetMovement; 
    

    [Header("Physics")]
    [SerializeField]
    /// Delta of forwarding speed (increment for boosting)
    private float speedDelta;

    Rigidbody rigidbody;

    #region Timer

    private float timer = 0f;
    [Header("Fuel")]
    [SerializeField]
    /// Timer for ammo waste. This value prevents ammo amount from decreasing too fast
    private float timerMax = 0.05f; 

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
    @return Returns true if jet is boosting, false otherwise
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
