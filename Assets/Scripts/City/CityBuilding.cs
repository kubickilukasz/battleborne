using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CityBuilding : MonoBehaviour
{
    //Inne/Gameplayowe
    [SerializeField]
    private int hp = 100;
    private int maxHp;
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private bool debug = false;
    private City city = null;

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

    public int MaxHealth
    {
        get
        {
            return maxHp;
        }
    }

    public int Health
	{
        get
		{
            return hp;
		}
        set
		{
            if (value >= 0 && value <= maxHp)
                hp = value;
            else if (value > maxHp)
                hp = maxHp;
            else
                hp = 0;
		}
	}



    public City Town
    {
        get
        {
            return city;
        }
        set
        {
            city = value;
        }
    }



    void FixedUpdate()
    {
        
    }

    void Start()
    {
        collider = GetComponent<Collider>();
        maxHp = hp;

        buildingDestroyedEvent = new UnityEvent();
        buildingDestroyedEvent.AddListener(ListenerPlaceholder.ListenDestroyBuilding);
    }



    public void OnHit(int hitPoints)
    {
        hp -= hitPoints;
        if (city != null)
        {
            city.OnHit(hitPoints);
            city.DisplayHP();
        }
    }



    public void OnDestroy()
    {
        city.OnDestroyBuilding(this);
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
