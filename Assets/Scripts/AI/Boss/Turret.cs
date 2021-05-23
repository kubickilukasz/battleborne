using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Boss' critical part - Turret, which attacks player
*/
public class Turret : BossPart
{
	/**
	* Minimum distance between turret and target, after which turret can start shooting
	*/
    [SerializeField]
    protected float minDistanceStartShooting;
    /**
    * Bullet that enemy shoots
    */
    [SerializeField]
    private GameObject bullet;
    /**
    * Firing force of the bullet
    */
    [SerializeField]
    private float dirMultiplier;  

    /**
    * Component responsible for detecing targets - mainly to determine if an enemy is clear to shoot
    */
    protected EnemyShooting enemyShooting;
    protected EnemyTimer fireTimer;

	/**
	* Cooldown before shooting next bullet
	*/
    [SerializeField]
    private float fireCooldown;
    
    /**
    * Shoots a bullet to player's direction
    */
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
