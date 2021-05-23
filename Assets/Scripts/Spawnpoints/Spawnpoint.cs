using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Type of Spawn - Spawn point
*/
public class Spawnpoint : Spawn
{
    /**
    * Method for activating a spawn area
    */
    public override void OnTriggerSpawn()
    {
        int count = spawnList.Count;

        if(count >= 1) {
            SpawnSingle(spawnList[Random.Range(0, count-1)], transform.position, transform.rotation);
        }
    }

    /**
    * Responsible for drawing a range of spawning
    */
    void OnDrawGizmos() {
        if(debugGizmos)
		{
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 2.0f);
        }
    }
}
