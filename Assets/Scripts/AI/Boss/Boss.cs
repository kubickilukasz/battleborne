using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Represents boss' AI
*/
public class Boss : MonoBehaviour
{
    /**
    * Determines if boss is currently on map
    */
    public static bool isBossAlive = false;
    public float maxSpeed;
	public float acceleration;

	/**
	* Maximum speed of boss when it has an engine alive
	*/
    [SerializeField]
	private float maxSpeedNormal;
	/**
	* Acceleration of boss when it has an engine alive
	*/
	[SerializeField]
	private float accelerationNormal;

    private Rigidbody rigidbody;

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
    * Debug variable to check boss' speed
    */
    [SerializeField]
    private bool debugSpeed = false;
    /**
    * Debug variable to check boss' health
    */
    [SerializeField]
    private bool debugHp = false;
    /**
    * Debug variable to check boss' next position to reach
    */
    [SerializeField]
    private bool debugIdlePos = false;

    /**
    * Particle after death of an enemy
    */
    [SerializeField]
    private GameObject deathParticle;

    /**
    * Next position to reach by the boss
    */
    protected Vector3 idlepos;
    /**
    * Size of imprecison box - used to set new position to reach
    */
    [SerializeField]
    protected float cycleImprecisonBoxSize;


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

    /**
    * List of parts of the boss
    */
    protected List<BossPart> bossParts;

    /**
    * Current health of the boss
    */
    public int Health
    {
        get
        {
            int temp = 0;
            foreach (BossPart bp in bossParts)
            {
                if (bp != null)
                {
                    temp += bp.Health;
                }
            }
            return temp;
        }
    }

    /**
    * Maximum health of the boss
    */
    public int MaxHealth { get; protected set; }


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        maxSpeed = maxSpeedNormal;
        acceleration = accelerationNormal;
        idlepos = transform.position;

        enemyImprecision = new EnemyImprecision(gameObject, new Vector3(cycleImprecisonBoxSize, 0, cycleImprecisonBoxSize));
        enemyMoveable = new EnemyMoveable(gameObject);
        enemyShooting = new EnemyShooting(gameObject);

        BossPart[] bossPartTab = GetComponentsInChildren<BossPart>();
        bossParts = new List<BossPart>(bossPartTab);
        foreach (BossPart bp in bossParts)
        {
            bp.Init(this);
            this.MaxHealth += bp.MaxHealth;
        }
    }

    void FixedUpdate()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }

        DisplayHP();
        
        enemyMoveable.Accelerate(Vector3.forward, acceleration, maxSpeed, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(idlepos + enemyImprecision.randomImprecision);
        GenerateNewImprecisionIfReached();
        if(debugIdlePos) Debug.Log("IdlePos: " + (idlepos + enemyImprecision.randomImprecision));
    }

    /**
    * Displays HP of the boss if debugging
    */
    public void DisplayHP()
	{
        if(debugHp) {
            Debug.Log("========== BOSS STATUS ==========");
            Debug.Log("HP: " + this.Health + "/" + this.MaxHealth);
            Debug.Log("========== BOSS STATUS ==========");
        }
    }

    /**
    * Generates new position to follow if reached the previous one
    */
	private void GenerateNewImprecisionIfReached()
	{
        if(enemyShooting.IsPositionInRange(idlepos + enemyImprecision.randomImprecision, 2f))
			enemyImprecision.GenerateRandomImprecision();
	}

    public void OnDestroy()
    {
        isBossAlive = false;
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
