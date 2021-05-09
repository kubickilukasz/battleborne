using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public abstract class AIEnemy : MonoBehaviour
{
    protected Rigidbody rigidbody;

    public float maxSpeed { get; protected set; }
    public float acc { get; protected set; }
    public float rotateSpeed { get; protected set; }

    public float detectRange { get; protected set; }
    public float ignoreTargetTime { get; protected set; }
    public float imprecisionTime { get; protected set; }
    public float dodgingTime { get; protected set; }

    [SerializeField]
    public GameObject player;
    [SerializeField]
    protected float playerDetectRange;

    [SerializeField]
    protected float crashDangerRange;

    [SerializeField]
    protected float spawnIgnorePlayerTime;
    [SerializeField]
    protected float cycleImprecisonBoxSize;
    [SerializeField]
    protected float nextImprecisionTime;
    protected Vector3 randomImprecision;
    [SerializeField]
    protected float nextDodgingTime;

    [SerializeField]
    protected float minAngleStartShooting;
    [SerializeField]
    protected float minDistanceStartShooting;
    [SerializeField]
    private GameObject bullet;
        [SerializeField]
    private float dirMultiplier;
    private float fireCooldownTime;
    [SerializeField]
        private float fireCooldown;
        

    [SerializeField]
    public int health;
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    protected bool debug = true;

    protected GameObject target;



    void FixedUpdate()
    {
        if (health > 0)
        {
            if (ignoreTargetTime > 0)
                ignoreTargetTime = ignoreTargetTime - Time.fixedDeltaTime * 100f;

            if (dodgingTime > 0)
                dodgingTime = dodgingTime - Time.fixedDeltaTime * 100f;

            if (imprecisionTime > 0)
                imprecisionTime = imprecisionTime - Time.fixedDeltaTime * 100f;
            else
            {
                GenerateRandomImprecision();
                imprecisionTime = nextImprecisionTime;
            }

            if(fireCooldownTime > 0)
                fireCooldownTime = fireCooldownTime - Time.fixedDeltaTime * 100f;

            UpdateStateMethods();
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        randomImprecision = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        SetupStartValues();
    }



    protected void Accelerate(Vector3 dir)
    {
        Vector3 newone = dir * acc* Time.fixedDeltaTime * 100;

        if (rigidbody.velocity.magnitude <= maxSpeed)
        {
            rigidbody.AddRelativeForce(newone);
        }
        else
        {
            rigidbody.AddRelativeForce(-newone);
        }
        if(debug) Debug.Log(rigidbody.velocity.magnitude);
    }

    private void FixVelocity()
    {
        float t = rigidbody.velocity.magnitude;
        Vector3 v = Vector3.forward;
        v = rigidbody.rotation.normalized * v;
        v *= t;

        rigidbody.velocity = v;
    }

    protected void StrafeTowardsFocusTarget(Vector3 extra)
	{
        if (target != null)
        {
            Vector3 direction = (target.transform.position + extra - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.fixedDeltaTime * 100f);
        }
        FixVelocity();
    }

    protected void StrafeTowardsConstPos(Vector3 idlepos, Vector3 extra)
    {
        if (idlepos != null)
        {
            Vector3 direction = (idlepos + extra - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.fixedDeltaTime * 100f);
        }
        FixVelocity();
    }

    private void GenerateRandomImprecision()
	{
        randomImprecision.x = Random.Range(-cycleImprecisonBoxSize, cycleImprecisonBoxSize);
        randomImprecision.y = Random.Range(-cycleImprecisonBoxSize, cycleImprecisonBoxSize);
        randomImprecision.z = Random.Range(-cycleImprecisonBoxSize, cycleImprecisonBoxSize);
    }



    protected bool CheckRaycast(Vector3 dir)
	{
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(dir), out hit, crashDangerRange))
        {
            if(!hit.transform.gameObject.GetComponent<Bullet>())
                return true;
        }
        return false;
	}


    protected bool IsPlayerInRange()
    {
        if (ignoreTargetTime <= 0 && (player.transform.position - transform.position).magnitude < playerDetectRange)
            return true;
        else
            return false;
    }

    protected bool CanStartShooting()
    {     
        if(target) {
            float angle = Vector3.Angle(transform.forward, target.transform.position-transform.position);
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, minDistanceStartShooting))
            {
                if(angle <= minAngleStartShooting && hit.transform && !hit.transform.gameObject.GetComponent<JetMovement>() && !hit.transform.gameObject.GetComponent<AIPlayerPlaceholder>())
                    return true;
            }
        }
        return false;
    }

    protected void Shoot()
    {
        if(fireCooldownTime <= 0.0f)
        {
            Debug.Log(bullet);
            GameObject bulletTrans = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            Debug.Log(bulletTrans);
            Vector3 direction = transform.forward * dirMultiplier;
            bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
            fireCooldownTime = fireCooldown;
        }
    }



    protected abstract void SetupStartValues();

    protected abstract void UpdateStateMethods();


    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag != "Ammo")
        {
            Debug.Log(other.collider.gameObject);
            health = 0;
        }
    }
    public void OnHit(int hitPoints)
    {
        health -= hitPoints;
    }

    public void OnDestroy()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
