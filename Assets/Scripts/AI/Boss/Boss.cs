using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static bool isBossAlive = false;
    public float maxSpeed;
	public float acceleration;

    [SerializeField]
	private float maxSpeedNormal;
	[SerializeField]
	private float accelerationNormal;

    private Rigidbody rigidbody;

    [SerializeField]
    public JetSpawn jetSpawn;
    [SerializeField]
    public City city;
    [SerializeField]
    private bool debugSpeed = false;
    [SerializeField]
    private bool debugHp = false;
    [SerializeField]
    private bool debugIdlePos = false;


    [SerializeField]
    private GameObject deathParticle;


    protected Vector3 idlepos;
    [SerializeField]
    protected float cycleImprecisonBoxSize;



    protected EnemyImprecision enemyImprecision;
    protected EnemyMoveable enemyMoveable;
    protected EnemyShooting enemyShooting;

    protected List<BossPart> bossParts;

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

    // Update is called once per frame
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

    public void DisplayHP()
	{
        if(debugHp) {
            Debug.Log("========== BOSS STATUS ==========");
            Debug.Log("HP: " + this.Health + "/" + this.MaxHealth);
            Debug.Log("========== BOSS STATUS ==========");
        }
    }

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
