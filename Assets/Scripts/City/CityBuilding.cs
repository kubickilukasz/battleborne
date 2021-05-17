using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CityBuilding : MonoBehaviour
{
    //Inne/Gameplayowe
    [SerializeField]
    private int hp = 100;

    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private bool debug = false;

    private Collider collider;
    UnityEvent buildingDestroyedEvent;

    void Update()
    {
        if (hp <= 0)
        {
            buildingDestroyedEvent.Invoke();
            Destroy(gameObject);
        }
    }

    public int MaxHealth { get; private set; }

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

    public City city { get; set; }


    public void Init(City newcity)
    {
        MaxHealth = hp;
        city = newcity;
    }

    void Start()
    {
        collider = GetComponent<Collider>();

        buildingDestroyedEvent = new UnityEvent();
        buildingDestroyedEvent.AddListener(ListenerPlaceholder.ListenDestroyBuilding);
    }



    public void OnHit(int hitPoints)
    {
        Health -= hitPoints;
        if (city != null)
        {
            city.OnHit(hitPoints);
        }
    }



    public void OnDestroy()
    {
        city.OnDestroyBuilding(this);
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
