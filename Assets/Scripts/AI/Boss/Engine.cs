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
            bossParent.acc = accelerationSlow;
            bossParent.maxSpeed = maxSpeedSlow;
        }
        base.FixedUpdate();
    }
}
