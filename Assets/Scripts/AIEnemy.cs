using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(Rigidbody))]
public class AIEnemy : MonoBehaviour
{
    //Status AI
    /*
     * Pomysl:
     * Przeciwnik zaczyna w pozycji RETREAT, wracajac do wskazanego targetu
     * Jesli gracz znajdzie sie w zasiegu wzroku przeciwnik zmienia stan na CHASE.
     * W CHASE przeciwnik leci prosto w kierunku gracza (TODO: Atak).
     * Jesli gracz znajdzie sie poza zasiegiem, zmieniony zostaje stan na RETREAT.
     * W RETREAT przeciwnik wraca do swojego targetu. Jesli znajdzie sie u swojego targetu, wraca do IDLE
    */
    public Rigidbody rigidbody;

    public enum State
    {
        IDLE,
        CHASE,
        RETREAT
    };

    [SerializeField]
    private State state;



    //Zmienne odpowiedzialne za Idle
    [SerializeField]
    private float idleMaxSpeed = 0f;

    [SerializeField]
    public GameObject idlePoint;
    [SerializeField]
    private float detectRangeIdlePoint = 5f;
    [SerializeField]
    private float detectRangeIdlePointFar = 40f;

    private float ignorePlayerTime;



    //Zmienne odpowiedzialne za Chase
    [SerializeField]
    private float chaseMaxSpeed = 2f;

    [SerializeField]
    public GameObject player;
    [SerializeField]
    private float detectRangePlayer = 20f;



    //Zmienne odpowiedzialne za Retreat 
    [SerializeField]
    private float retreatMaxSpeed = 1f;



    //Tymczasowe
    [SerializeField]
    private float acc = 0.005f;
    [SerializeField]
    private float torqueacc = 250f;

    private float maxspeed = 0f;
    private GameObject currentTarget;



    //Inne/Gameplayowe
    [SerializeField]
    public int hp = 100;
    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private bool debug = true;

    // ***** UPDATE *****



    void FixedUpdate()
    {
        if (hp > 0)
        {
            if (ignorePlayerTime > 0)
                ignorePlayerTime = ignorePlayerTime - Time.fixedDeltaTime * 100f;

            switch (state)
            {
                case State.IDLE:
                    Idle();
                    break;
                case State.CHASE:
                    Chase();
                    break;
                case State.RETREAT:
                    Retreat();
                    break;
            }
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        SetState(State.RETREAT);
    }



    // ***** UPDATE *****



    // ***** PORUSZANIE SIE *****



    void AccelerateForward()
    {
        if (rigidbody.velocity.magnitude <= maxspeed)
        {
            rigidbody.AddRelativeForce(Vector3.forward * acc * Time.fixedDeltaTime * 10000);
        }
        else
        {
            rigidbody.AddRelativeForce(-Vector3.forward * acc * Time.fixedDeltaTime * 10000);
        }

        //Debug.Log("Velocity: " + rigidbody.velocity.magnitude + "/" + maxspeed);
    }

    void StrafeTowardsFocusTarget()
	{
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            //Debug.Log("Rotating towards: " + currentTarget + " - " + q);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * torqueacc);
        }
    }

    void FixVelocity()
	{
        float t = rigidbody.velocity.magnitude;
        Vector3 v = Vector3.forward;
        v = rigidbody.rotation.normalized * v;
        v *= t;

        
        rigidbody.velocity = v;
    }

    void DetectPlayerOrIdlePoint()
    {
        //Debug.Log("From Player: " + (player.transform.position - transform.position).magnitude + "/" + detectRangePlayer);
        //Debug.Log("From Idlepoint: " + (player.transform.position - transform.position).magnitude + "/" + detectRangeIdlePoint);

        if ((currentTarget.transform.position - transform.position).magnitude < detectRangeIdlePoint)
        {
            //Debug.Log("Setting to IDLE");
            SetState(State.IDLE);
        }
        else if (ignorePlayerTime <= 0 && (player.transform.position - transform.position).magnitude < detectRangePlayer)
        {
            //Debug.Log("Setting to CHASE");
            SetState(State.CHASE);
        }
    }

    void DetectPlayer()
    {
        //Debug.Log("From Player: " + (player.transform.position - transform.position).magnitude + "/" + detectRangePlayer);

        if (ignorePlayerTime <= 0 && (player.transform.position - transform.position).magnitude < detectRangePlayer)
        {
            //Debug.Log("Setting to CHASE");
            SetState(State.CHASE);
        }

    }

    void DetectNotTooFar()
	{
        //Debug.Log("From Idlepoint: " + (player.transform.position - transform.position).magnitude + "/" + detectRangeIdlePointFar);
        if ((idlePoint.transform.position - transform.position).magnitude > detectRangeIdlePointFar)
        {
            //Debug.Log("Setting to RETREAT (too far)");
            ignorePlayerTime = 400f;
            SetState(State.RETREAT);
        }
    }

    void DetectNotLostTarget()
	{
        //Debug.Log("From Idlepoint: " + (player.transform.position - transform.position).magnitude + "/" + detectRangePlayer);
        if ((currentTarget.transform.position - transform.position).magnitude > detectRangePlayer)
        {
            //Debug.Log("Setting to RETREAT (lost target)");
            SetState(State.RETREAT);
        }
    }

    // ***** PORUSZANIE SIE *****



    // ***** MASZYNA STANOW *****



    void SetState(State newstate)
    {
        switch (newstate)
        {
            case State.IDLE:
                maxspeed = idleMaxSpeed;
                currentTarget = null;
                state = newstate;
                break;
            case State.CHASE:
                maxspeed = chaseMaxSpeed;
                currentTarget = player;
                state = newstate;
                break;
            case State.RETREAT:
                maxspeed = retreatMaxSpeed;
                currentTarget = idlePoint;
                state = newstate;
                break;
            default:
                //Debug.Log("Dlaczego Default?");
                break;
        }
    }

    void Idle()
    {
        //Debug.Log("========== IDLE ==========");
        AccelerateForward();
        StrafeTowardsFocusTarget();
        FixVelocity();
        DetectPlayer();
        //Debug.Log("Ignore Time: " + ignorePlayerTime);
        //Debug.Log("========== IDLE ==========");
    }

    void Retreat()
    {
        //Debug.Log("========== RETREAT ==========");
        AccelerateForward();
        StrafeTowardsFocusTarget();
        FixVelocity();
        DetectPlayerOrIdlePoint();
        //Debug.Log("Ignore Time: " + ignorePlayerTime);
        //Debug.Log("========== RETREAT ==========");
    }

    void Chase()
    {
        //Debug.Log("========== CHASE ==========");
        AccelerateForward();
        StrafeTowardsFocusTarget();
        FixVelocity();
        DetectNotTooFar();
        DetectNotLostTarget();
        //Debug.Log("Ignore Time: " + ignorePlayerTime);
        //Debug.Log("========== CHASE ==========");
    }



    // ***** MASZYNA STANOW *****



    // ***** DEBUG *****



    void OnDrawGizmos()
    {
        if (debug)
        {
            if (currentTarget != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, currentTarget.transform.position);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + rigidbody.velocity.normalized);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRangePlayer);
        }
    }

    public void OnHit(int hitPoints)
	{
        hp -= hitPoints;
        if (hp <= 0)
            OnDestroy();
	}

    public void OnDestroy()
	{
    }


    // ***** DEBUG *****
}
