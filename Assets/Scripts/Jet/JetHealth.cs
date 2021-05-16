using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(JetPoints))]
public class JetHealth : MonoBehaviour
{

    public UnityEvent onDestroyEvent;

    [Header("Jet Health")]
    [SerializeField]
    private int health;

    [SerializeField]
    private int maxHealth = 100;

    private bool invincibility = false;

    [Header("Explosion")]
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private int explosionHitPoints;

    [SerializeField]
    private int explosionPenalty;

    private JetPoints combo;

    void Start()
    {
        health = maxHealth;
        combo = GetComponent<JetPoints>();
    }

    void Update()
    {
        NoHealthExplode();
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

    private void NoHealthExplode()
    {
        if(health <= 0)
        {
            GameObject boom = Instantiate(explosion,transform.position,Quaternion.identity) as GameObject;
            GetComponent<Renderer>().enabled = false;
            combo.DecreasePoints(explosionPenalty);
            combo.ResetCombo();
            onDestroyEvent.Invoke();
            Destroy(gameObject);
        }
    }

    private void SetInvincibility()
    {
        if(combo.isMaxCombo())
        {
            invincibility = true;
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

    public void OnHit(int hitPoints)
    {
        health -= hitPoints;
    }

#endregion

}
