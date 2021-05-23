using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Boss' critical part - Engine
*/
public class Engine : BossPart
{
	/**
	* Maximum speed of boss when engine has been destroyed
	*/
    [SerializeField]
	private float maxSpeedSlow;
	/**
	* Acceleration of boss when engine has been destroyed
	*/
	[SerializeField]
	private float accelerationSlow;

    void FixedUpdate()
    {
        if (!neutralized && hp <= 0 && bossParent) {
            neutralized = true;
            Instantiate(deathParticle, transform.position, transform.rotation);
            Instantiate(smokeParticle, transform.position - transform.forward * 4f, transform.rotation, gameObject.transform);

            bossParent.acceleration = accelerationSlow;
            bossParent.maxSpeed = maxSpeedSlow;
        }
    }
}
