using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : Spawn
{
    public override void OnTriggerSpawn()
    {
        int count = spawnList.Count;

        if(count >= 1) {
            SpawnSingle(spawnList[Random.Range(0, count-1)], transform.position, transform.rotation);
        }
    }
    void OnDrawGizmos() {
        if(debugGizmos)
		{
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 2.0f);
        }
    }
}
