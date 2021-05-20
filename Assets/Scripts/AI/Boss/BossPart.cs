using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour
{
    [SerializeField]
    protected int hp = 100;

    [SerializeField]
    protected GameObject deathParticle;
    [SerializeField]
    protected GameObject smokeParticle;
    [SerializeField]
    protected bool debug = false;

    protected bool neutralized = false;
    protected Boss bossParent;

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

    public int MaxHealth { get; private set; }

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

    public void OnHit(int hitPoints)
    {
        Health -= hitPoints;
    }
}
