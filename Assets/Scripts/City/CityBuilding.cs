using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
* Representation of the city's building
*/
public class CityBuilding : MonoBehaviour
{
    /**
    * Health of the building
    */
    [SerializeField]
    private int hp = 100;

    /**
    * Particle after death of a building
    */
    [SerializeField]
    private GameObject deathParticle;

    /**
    * Audio played when building is destroyed
    */
    [SerializeField]
    private AudioSource buildingDestroyed;

    private Collider collider;

    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    /**
    * Maximum health of the building
    */
    public int MaxHealth { get; private set; }

    /**
    * Current health of the building
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
    * Reference to city the building belongs to
    */
    public City city { get; set; }

    /**
    * Initialization of the building
    * @param newcity Reference to the city the building will belong to
    */
    public void Init(City newcity)
    {
        MaxHealth = hp;
        city = newcity;
    }

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    /**
    * Deals damage to a building.
    *
    * @param hitPoints Damage
    */
    public void OnHit(int hitPoints)
    {
        Health -= hitPoints;
    }



    public void OnDestroy()
    {
        city.OnDestroyBuilding(this);
        //buildingDestroyed.Play();
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
