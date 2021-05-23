using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Boss' critical part - Hangar, which spawns enemies
*/
public class Hangar : BossPart
{
    /**
    * Spawnpoint of the hangar - point of guarding for guardians
    */
    private Spawn spawn;
    /**
    * Spawnpoint of the hangar - point of spawning all enemies
    */
    private Spawn secondSpawn;

    /**
    * Initialization of the boss' part
    * @param boss Reference to the boss the part will belong to
    */
    public override void Init (Boss boss) {
        base.Init(boss);

        spawn = GetComponentInChildren<Spawn>();
        Spawn[] secondSpawnPot = spawn.GetComponentsInChildren<Spawn>();
        secondSpawn = secondSpawnPot[1];

        spawn.jetSpawn = bossParent.jetSpawn;
        spawn.city = bossParent.city;
        secondSpawn.jetSpawn = bossParent.jetSpawn;
        secondSpawn.city = bossParent.city;
    }
    

    void FixedUpdate()
    {
        if (!neutralized && hp <= 0 ) 
            Destroy(spawn.gameObject);
        
        base.FixedUpdate();
    }
}
