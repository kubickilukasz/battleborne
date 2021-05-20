using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour
{
    [SerializeField]
    protected int hp = 100;

    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private GameObject smokeParticle;
    [SerializeField]
    private float smokeParticleCooldown;
    private float smokeParticleTime = 0f;
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
        }
        else if (neutralized && smokeParticleTime > 0) {
            smokeParticleTime = smokeParticleTime - Time.fixedDeltaTime * 100f;
        }
        else if (neutralized) {
            Instantiate(smokeParticle, transform.position, transform.rotation);
            smokeParticleTime = smokeParticleCooldown;
        }
    }

    public void OnHit(int hitPoints)
    {
        Health -= hitPoints;
        if (bossParent != null)
        {
            bossParent.OnHit(hitPoints);
        }
    }
}
