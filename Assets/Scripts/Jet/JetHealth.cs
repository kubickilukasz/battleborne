using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(JetPoints))]
/**
Class that stores the data and performs actions referring to jet's health
*/
public class JetHealth : MonoBehaviour
{

    public UnityEvent onDestroyEvent;

    [Header("Jet Health")]
    [SerializeField]
    private int health; /// Current amount of health of the jet

    [SerializeField]
    private int maxHealth = 100; /// Maximum amount of health of the jet

    private bool invincibility = false;

    [Header("Explosion")]
    [SerializeField]
    private GameObject explosion; /// Explosion effect instantiated after dying

    [SerializeField]
    private AudioSource kaboom; /// Sound effect for the explosion

    [SerializeField]
    private int explosionHitPoints; /// Amount of hit points dealt on explosion (to buildings, enemies)

    private JetPoints combo;

    void Start()
    {
        health = maxHealth;
        combo = GetComponent<JetPoints>();
    }

    void Update()
    {
        NoHealthExplode();
        SetInvincibility();
    }


    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag != "Ammo")
        {
            if(other.collider.tag == "City")
            {
                CityBuilding building = other.collider.GetComponent<CityBuilding>();
                if(building != null)
                {
                    building.OnHit(explosionHitPoints);
                }
                health = 0;
            }
            else if(other.collider.tag == "Alien")
            {
                AIEnemy enemy = other.collider.GetComponent<AIEnemy>();
                if(enemy!= null)
                {
                    enemy.OnHit(explosionHitPoints);
                }
                if(!invincibility)
                    health = 0;
            }
            else health = 0;
        }
    }

#region PrivateMethods


    /**
    Method that checks whether the amount of health reached 0 and performs  the explosion
    */
    private void NoHealthExplode()
    {
        if(health <= 0)
        {
            GameObject boom = Instantiate(explosion,transform.position,Quaternion.identity) as GameObject;
            kaboom.Play();
            GetComponent<Renderer>().enabled = false;
            onDestroyEvent.Invoke();
            Destroy(gameObject);
        }
    }

    /**
    Method used for setting immortality of the jet based on the combo value
    */
    private void SetInvincibility()
    {
        if(combo.isMaxCombo())
        {
            invincibility = true;
        }
        else
        {
            invincibility = false;
        }
    }
#endregion


#region PublicMethods

    public int GetHealth()
    {
        return health;        
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    /**
    Method used when jet gets hit by enemy's bullet, hits the ground etc.
    */
    public void OnHit(int hitPoints)
    {
        if(!invincibility)
            health -= hitPoints;
    }

#endregion

}
