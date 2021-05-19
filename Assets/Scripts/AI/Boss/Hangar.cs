using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : BossPart
{
    private Spawn spawn;
    private Spawn secondSpawn;

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
