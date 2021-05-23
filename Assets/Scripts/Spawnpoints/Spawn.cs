using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Base class for Spawn Points and Spawn Areas
*/
public abstract class Spawn : MonoBehaviour
{
    /**
    * Should this spawn bee destroyed after spawning an object
    */
    [SerializeField]
    private bool destroyAfterSpawn = false;
    /**
    * Special indicator for spawning object instantly
    */
    [SerializeField]
    public bool forceSpawn = false;

    /**
    * List of objects that can be spawned
    */
    [SerializeField]
    protected List<GameObject> spawnList;



	protected EnemyTimer respawnTimer;
    /**
    * Cooldown before spawning another object
    */
    [SerializeField]
    protected float respawnCooldown;



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
    * List of children
    */
    private List<Spawn> spawnChildren;

    /**
    * Debug variable to check range of spawning
    */
    [SerializeField]
    protected bool debugGizmos;
    /**
    * Debug variable to check delay before spawning an object
    */
    [SerializeField]
    protected bool debugTimer;

    /**
    * Spawns an object
    * @param toSpawn Object to spawn
    * @param pos Position to spawn at
    * @param rot Rotation to spawn with
    */
    protected void SpawnSingle(GameObject toSpawn, Vector3 pos, Quaternion rot)
    {
        if(toSpawn.GetComponent<Boss>() && !Boss.isBossAlive)
        {
            Boss.isBossAlive = true;
            GameObject spawned = Instantiate(toSpawn, pos, rot) as GameObject;

            Boss bossScript = spawned.GetComponent<Boss>();
            if (bossScript) {
                bossScript.jetSpawn = jetSpawn;
                bossScript.city = city;
            }
        }
        else if(!toSpawn.GetComponent<Boss>())
        {
            GameObject spawned = Instantiate(toSpawn, pos, rot) as GameObject;
            
            AIEnemy enemyScript = spawned.GetComponent<AIEnemy>();
            if (enemyScript) {
                enemyScript.jetSpawn = jetSpawn;
                enemyScript.city = city;
            }

            AIGuardian guardScript = spawned.GetComponent<AIGuardian>();
            if (guardScript && transform.parent)
                guardScript.guardingObject = transform.parent.gameObject;
        }
    }

    /**
    * Base method for activating a spawn point/area
    */
    public abstract void OnTriggerSpawn();

    void Start()
    {
        Spawn[] spawns = GetComponentsInChildren<Spawn>();
        Spawn myspawn = GetComponent<Spawn>();
        spawnChildren = new List<Spawn>(spawns);
        spawnChildren.Remove(myspawn);

        respawnTimer = new EnemyTimer(respawnCooldown, respawnCooldown);
    }

    void FixedUpdate()
    {
        if(forceSpawn)
        {
            OnTriggerSpawn();
            forceSpawn = false;
        }

        respawnTimer.UpdateTimer();
        if(debugTimer) respawnTimer.DebugTimer();
        if(respawnTimer.IsTimerZero())
        {
            OnTriggerSpawn();
            respawnTimer.ResetTimer();
        }

    }
}
