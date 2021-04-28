using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JetHealth : MonoBehaviour
{

    public UnityEvent onDestroyEvent;

    [Header("Jet Health")]
    [SerializeField]
    private int health;

    [SerializeField]
    private int maxHealth = 100;

    [Header("Explosion")]
    [SerializeField]
    private GameObject explosion;

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
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
                // CityBuilding building = other.collider.GetComponent<CityBuilding>();
                // if(building != null)
                // {
                //     building.OnHit(100);
                // }
            }
            else if(other.collider.tag == "Alien")
            {
                AIEnemy enemy = other.collider.GetComponent<AIEnemy>();
                if(enemy!= null)
                {
                    enemy.OnHit(100);
                }
            }
            health = 0;
        }
    }

#region PrivateMethods

    private void NoHealthExplode()
    {
        if(health <= 0)
        {
            GameObject boom = Instantiate(explosion,transform.position,Quaternion.identity) as GameObject;
            GetComponent<Renderer>().enabled = false;
            onDestroyEvent.Invoke();
            Destroy(gameObject);
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
