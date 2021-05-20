using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : BossPart
{
    [SerializeField]
	private float maxSpeedSlow;
	[SerializeField]
	private float accelerationSlow;
    // Start is called before the first frame update

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
