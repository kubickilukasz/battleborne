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
    public float torqueacc { get; protected set; }

    public float detectRange { get; protected set; }
    public float ignoreTargetTime { get; protected set; }
    public float imprecisionTime { get; protected set; }
    public float dodgingTime { get; protected set; }

    [SerializeField]
    protected GameObject player;
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
    private Vector3 randomImprecision;
    [SerializeField]
    protected float nextDodgingTime;

    [SerializeField]
    public int hp;
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    protected bool debug = true;

    protected GameObject target;



    void FixedUpdate()
    {
        if (hp > 0)
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

            UpdateStateMethods();
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        randomImprecision = new Vector3(0.0f, 0.0f, 0.0f);
        rigidbody = GetComponent<Rigidbody>();
        SetupStartValues();
    }



    protected void AccelerateForward()
    {
        Vector3 newone = Vector3.forward * acc* Time.fixedDeltaTime * 100;

        if (rigidbody.velocity.magnitude <= maxSpeed)
        {
            rigidbody.AddRelativeForce(newone);
        }
        else
        {
            rigidbody.AddRelativeForce(-newone);
        }
    }

    private void FixVelocity()
    {
        float t = rigidbody.velocity.magnitude;
        Vector3 v = Vector3.forward;
        v = rigidbody.rotation.normalized * v;
        v *= t;

        rigidbody.velocity = v;
    }

    protected void StrafeTowardsFocusTarget()
	{
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * torqueacc);
        }
        FixVelocity();
    }

    protected void StrafeTowardsConstPos(Vector3 idlepos)
    {
        if (idlepos != null)
        {
            Vector3 direction = (idlepos - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * torqueacc);
        }
        FixVelocity();
    }

    protected void CycleAroundTarget()
	{
        if (target.transform.position != null)
        {
            Vector3 direction = (target.transform.position + randomImprecision - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * torqueacc);
        }
        FixVelocity();
    }

    protected void CycleAroundConstPos(Vector3 idlepos)
    {
        if (idlepos != null)
        {
            Vector3 direction = (idlepos + randomImprecision - transform.position).normalized;
            Quaternion q = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * torqueacc);
        }

        FixVelocity();
    }

    private void GenerateRandomImprecision()
	{
        randomImprecision.x = Random.Range(-cycleImprecisonBoxSize, cycleImprecisonBoxSize);
        randomImprecision.y = Random.Range(-cycleImprecisonBoxSize, cycleImprecisonBoxSize);
        randomImprecision.z = Random.Range(-cycleImprecisonBoxSize, cycleImprecisonBoxSize);
    }



    protected bool CheckRaycast()
	{
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, crashDangerRange))
        {
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



    protected abstract void SetupStartValues();

    protected abstract void UpdateStateMethods();



    public void OnHit(int hitPoints)
    {
        hp -= hitPoints;
    }

    public void OnDestroy()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
