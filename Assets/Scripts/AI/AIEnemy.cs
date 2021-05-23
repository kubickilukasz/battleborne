using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
* Base class that represents enemies' behaviour
*/
[RequireComponent(typeof(Rigidbody))]
public abstract class AIEnemy : MonoBehaviour
{
    /**
    * Debug variable to check enemy's state
    */
    [SerializeField]
    public bool debugStates;
    /**
    * Debug variable to check enemy's speed
    */
    [SerializeField]
    public bool debugSpeed;
    /**
    * Health of an enemy
    */
    [SerializeField]
    public int health;
    /**
    * Particle after death of an enemy
    */
    [SerializeField]
    private GameObject deathParticle;
    /**
    * Size of imprecison box/cube - used mainly with idle state
    */
    [SerializeField]
    private float cycleImprecisonBoxSize;
    /**
    * Damage that deals enemy if collided with an object
    */
    [SerializeField]
    private int explosionHitPoints;
    
    /**
    * Bullet that enemy shoots
    */
    [SerializeField]
    private GameObject bullet;
    /**
    * Firing force of the bullet
    */
    [SerializeField]
    private float dirMultiplier;

    /**
    * Reference to jet's spawnpoint
    */
    [SerializeField]
    public JetSpawn jetSpawn;
    /**
    * Reference to city
    */
	[SerializeField]
    public City city;

    /**
    * Audio played when alien shoots
    */
    [SerializeField]
    private AudioSource alienShot;

    /**
    * Component responsible for generating random imprecision
    */
    protected EnemyImprecision enemyImprecision;
    /**
    * Component responsible for motion control of an enemy
    */
    protected EnemyMoveable enemyMoveable;
    /**
    * Component responsible for detecing targets - mainly to determine if an enemy is clear to shoot
    */
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

    /**
    * Sets up start values of further instances of this class
    */
    protected abstract void SetupStartValues();
    
    /**
    * Determines behaviour of an enemy, depending on his state of further instances of this class
    */
    protected abstract void UpdateStateMethods();
    
    /**
    * Updates timers of further instances of this class
    */
    protected abstract void UpdateTimers();

    /**
    * Shoots a bullet forward
    */
    public void Shoot() 
    {
        GameObject bulletTrans = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
        Vector3 direction = gameObject.transform.forward * dirMultiplier;
        bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
        //alienShot.Play();
    }

    /**
    * Shoots a bullet to selected direction
    *
    * @param preDir Selected direction
    */
    public void ShootDirection(Vector3 preDir) 
    {
        GameObject bulletTrans = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
        Vector3 direction = preDir.normalized * dirMultiplier;
        bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
        alienShot.Play();
    }

    /**
    * Determines what should happen after collision with an object
    *
    * @param other Object's collison
    */
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag != "Ammo")
        {
            if(other.collider.tag == "City")
            {
                CityBuilding building = other.collider.GetComponent<CityBuilding>();
                if(building != null)
                {
                    building.OnHit(explosionHitPoints);
                }
            }
            health = 0;
        }
    }
    
    /**
    * Deals damage to an enemy.
    *
    * @param hitPoints Damage
    */
    public void OnHit(int hitPoints)
    {
        health -= hitPoints;
    }

    public void OnDestroy()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
