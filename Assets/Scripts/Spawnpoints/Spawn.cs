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

    [SerializeField]
    protected GameObject player;

    [SerializeField]
    protected bool debug = false;

    protected void SpawnSingle(GameObject toSpawn, Vector3 pos, Quaternion rot)
    {
        GameObject spawned = Instantiate(toSpawn, pos, rot) as GameObject;
        
        AIEnemy enemyScript = spawned.GetComponent<AIEnemy>();
        AIGuardian guardScript = spawned.GetComponent<AIGuardian>();

        if (guardScript && transform.parent)
            guardScript.guardingObject = transform.parent.gameObject;

        if(enemyScript && player)
            enemyScript.player = player;
    }

    public abstract void OnTriggerSpawn();

    void Start()
    {
    }

    void Update()
    {
        if(forceSpawn)
        {
            TriggerSpawn();
            forceSpawn = false;
        }
    }

    public void TriggerSpawn()
	{
        OnTriggerSpawn();

        Spawn[] spawns = GetComponentsInChildren<Spawn>();
        Spawn myspawn = GetComponent<Spawn>();
        List<Spawn> spawnlist = new List<Spawn>(spawns);
        spawnlist.Remove(myspawn);

        foreach (Spawn sp in spawnlist)
        {
            sp.TriggerSpawn();
        }
        
        if (destroyAfterSpawn) Destroy(gameObject);
	}
}
