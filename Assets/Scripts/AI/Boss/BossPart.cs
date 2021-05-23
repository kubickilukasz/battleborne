using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Represents boss' critical parts
*/
public class BossPart : MonoBehaviour
{
    /**
    * Current health of the boss' part
    */
    [SerializeField]
    protected int hp = 100;

    /**
    * Particle after neutralizing boss' part
    */
    [SerializeField]
    protected GameObject deathParticle;
    /**
    * Particle after neutralizing boss' part
    */
    [SerializeField]
    protected GameObject smokeParticle;

    /**
    * Determines if boss' part has been neutrailize
    */
    protected bool neutralized = false;

    /**
    * Reference to Boss the part belongs to
    */
    protected Boss bossParent;

    /**
    * Current health of the boss' part
    */
    public int Health
	{
        get
		{
            return hp;
		}
        set
		{
            if (value >= 0 && value <= MaxHealth)
                hp = value;
            else if (value > MaxHealth)
                hp = MaxHealth;
            else
                hp = 0;
		}
	}

    /**
    * Maximum health of the boss' part
    */
    public int MaxHealth { get; private set; }

    /**
    * Initialization of the boss' part
    * @param boss Reference to the boss the part will belong to
    */
    public virtual void Init (Boss boss) {
        MaxHealth = hp;
        bossParent = boss;
    }

    protected void FixedUpdate()
    {
        if (!neutralized && hp <= 0 ) {
            neutralized = true;
            Instantiate(deathParticle, transform.position, transform.rotation);
            Instantiate(smokeParticle, transform.position, transform.rotation, gameObject.transform);
        }
    }

    /**
    * Deals damage to a boss' part.
    *
    * @param hitPoints Damage
    */
    public void OnHit(int hitPoints)
    {
        Health -= hitPoints;
    }
}
