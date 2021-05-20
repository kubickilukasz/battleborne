using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public abstract class AIEnemy : MonoBehaviour
{
    [SerializeField]
    public bool debugStates;
    [SerializeField]
    public bool debugSpeed;
    [SerializeField]
    public int health;
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private float cycleImprecisonBoxSize;
    
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float dirMultiplier;

    [SerializeField]
    public JetSpawn jetSpawn;
	[SerializeField]
    public City city;

    protected EnemyImprecision enemyImprecision;
    protected EnemyMoveable enemyMoveable;
    protected EnemyShooting enemyShooting;

    void FixedUpdate()
    {
        if (health > 0)
        {
            UpdateTimers();
            UpdateStateMethods();
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        enemyImprecision = new EnemyImprecision(gameObject, new Vector3(cycleImprecisonBoxSize, cycleImprecisonBoxSize, cycleImprecisonBoxSize));
        enemyMoveable = new EnemyMoveable(gameObject);
        enemyShooting = new EnemyShooting(gameObject);

        SetupStartValues();
    }

    protected abstract void SetupStartValues();
    protected abstract void UpdateStateMethods();
    protected abstract void UpdateTimers();

    public void Shoot() 
    {
        GameObject bulletTrans = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
        Vector3 direction = gameObject.transform.forward * dirMultiplier;
        bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
    }

    public void ShootDirection(Vector3 preDir) 
    {
        GameObject bulletTrans = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
        Vector3 direction = preDir * dirMultiplier;
        bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag != "Ammo")
            health = 0;
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
