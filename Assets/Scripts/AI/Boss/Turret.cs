using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : BossPart
{
    [SerializeField]
    protected float minDistanceStartShooting;
    [SerializeField]
    private GameObject bullet;
    private float fireCooldownTime;
    [SerializeField]
    private float fireCooldown = 0f;
    [SerializeField]
    private float dirMultiplier;  
    protected bool CanStartShooting()
    {     
        RaycastHit hit;
        if (Physics.Raycast(transform.position, bossParent.player.transform.position - transform.position, out hit, minDistanceStartShooting))
        {
            if(hit.transform)
            {
                if(hit.transform.gameObject.GetComponent<JetMovement>() || hit.transform.gameObject.GetComponent<AIPlayerPlaceholder>()) {
                    if(debug) Debug.Log("I hit " + hit.transform.gameObject.GetComponent<AIPlayerPlaceholder>());
                    return true;
                }
            }
            else
            {
                if(debug) Debug.Log("??? " + hit);
            }
        }
        else
        {
            if(debug) Debug.Log("Nic nie raycast");
        }

        return false;
    }

    protected void Shoot()
    {
        GameObject bulletTrans = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        Vector3 direction = (bossParent.player.transform.position - transform.position) * dirMultiplier;
        bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
    }
    public bool IsPlayerInRange()
    {
        if ((bossParent.player.transform.position - transform.position).magnitude < minDistanceStartShooting)
            return true;
        else
            return false;
    }

    public void ShootIfInRange() {
        if (fireCooldownTime > 0) {
            fireCooldownTime = fireCooldownTime - Time.fixedDeltaTime * 100f;
        }
        else if(CanStartShooting() && IsPlayerInRange()) {
            if(debug) Debug.Log("Tur cond 2");
            Shoot();
            fireCooldownTime = fireCooldown;
        }
    }
    void FixedUpdate()
    {
        if (bossParent && !neutralized) {
            ShootIfInRange();
        }
        base.FixedUpdate();
    }


}
