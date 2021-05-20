using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawn : MonoBehaviour
{
    [SerializeField]
    private bool destroyAfterSpawn = false;
    [SerializeField]
    public bool forceSpawn = false;


    [SerializeField]
    protected List<GameObject> spawnList;



	protected EnemyTimer respawnTimer;
    [SerializeField]
    protected float respawnCooldown;



    [SerializeField]
    public JetSpawn jetSpawn;
    [SerializeField]
    public City city;
    private List<Spawn> spawnlist;

    [SerializeField]
    protected bool debugGizmos;
    [SerializeField]
    protected bool debugTimer;

    protected void SpawnSingle(GameObject toSpawn, Vector3 pos, Quaternion rot)
    {
        GameObject spawned = Instantiate(toSpawn, pos, rot) as GameObject;

        AIEnemy enemyScript = spawned.GetComponent<AIEnemy>();
        if (enemyScript) {
            enemyScript.jetSpawn = jetSpawn;
            enemyScript.city = city;
        }

        Boss bossScript = spawned.GetComponent<Boss>();
        if (bossScript) {
            bossScript.jetSpawn = jetSpawn;
            bossScript.city = city;
        }

        AIGuardian guardScript = spawned.GetComponent<AIGuardian>();
        if (guardScript && transform.parent)
            guardScript.guardingObject = transform.parent.gameObject;
    }

    public abstract void OnTriggerSpawn();

    void Start()
    {
        Spawn[] spawns = GetComponentsInChildren<Spawn>();
        Spawn myspawn = GetComponent<Spawn>();
        spawnlist = new List<Spawn>(spawns);
        spawnlist.Remove(myspawn);

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
