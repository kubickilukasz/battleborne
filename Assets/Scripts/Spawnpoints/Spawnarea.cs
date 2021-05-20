using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnarea : Spawn
{
    [SerializeField]
    private Vector3 spawnAreaSize;

    public override void OnTriggerSpawn()
    {
        float xspawn = Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2);
        float yspawn = Random.Range(-spawnAreaSize.y/2, spawnAreaSize.y/2);
        float zspawn = Random.Range(-spawnAreaSize.z/2, spawnAreaSize.z/2);
        Vector3 newspawn = new Vector3(transform.position.x + xspawn, transform.position.y + yspawn, transform.position.z + zspawn);
        int count = spawnList.Count;
        if(count >= 1)
        {
            SpawnSingle(spawnList[Random.Range(0, count)], newspawn, transform.rotation);
        }
    }

    void OnDrawGizmos()
	{
        if(debugGizmos)
		{
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, spawnAreaSize.z));
        }
	}
}
