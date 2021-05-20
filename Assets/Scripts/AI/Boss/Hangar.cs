using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : BossPart
{
    [SerializeField]
    private float respawnCooldown;
    private float respawnTime = 0f;

    void SpawnEnemy()
    {
        if (respawnTime > 0) {
            if(debug) Debug.Log("Next Spawn in " + respawnTime);
            respawnTime = respawnTime - Time.fixedDeltaTime * 100f;
        }
        else {
            if(debug) Debug.Log("Hangar Spawning");
            Spawn spawn = GetComponentInChildren<Spawn>();
            spawn.forceSpawn = true;
            respawnTime = respawnCooldown;
        }
    }

    void FixedUpdate()
    {
        if (!neutralized) {
            SpawnEnemy();
        }
        base.FixedUpdate();
    }
}
