using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : BossPart
{
    [SerializeField]
    protected float minDistanceStartShooting;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float dirMultiplier;  
    protected EnemyShooting enemyShooting;

    protected EnemyTimer fireTimer;
    [SerializeField]
    private float fireCooldown;
    

    protected void Shoot()
    {
        if(bossParent?.jetSpawn?.jetReference) {
            GameObject bulletTrans = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            Vector3 direction = (bossParent.jetSpawn.jetReference.transform.position - transform.position).normalized * dirMultiplier;
            bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
        }
    }

    void Start() {
        fireTimer = new EnemyTimer(0, fireCooldown);
        enemyShooting = new EnemyShooting(gameObject);
    }

    void FixedUpdate()
    {
        fireTimer.UpdateTimer();

        if (
            bossParent?.jetSpawn?.jetReference &&
            !neutralized &&
            fireTimer.IsTimerZero() &&
            enemyShooting.IsPositionInRange(bossParent.jetSpawn.jetReference.transform.position, minDistanceStartShooting) &&
            enemyShooting.CheckCrashCollisions(Vector3.forward, minDistanceStartShooting)
        )
        {
            Shoot();
            fireTimer.ResetTimer();
        }

        base.FixedUpdate();
    }


}
