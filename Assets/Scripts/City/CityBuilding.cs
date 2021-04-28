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
    private GameObject particle;
    [SerializeField]
    private bool debug = false;
    private City city = null;

    private Collider collider;
    UnityEvent buildingDestroyedEvent;

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



    // ***** UPDATE *****



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



    // ***** UPDATE *****



    // ***** GAMEPLAYOWE *****



    public void OnHit(int hitPoints)
    {
        hp -= hitPoints;
        if (city != null) city.OnHit(hitPoints);

        if (hp <= 0)
        {
            buildingDestroyedEvent.Invoke();
            if (city != null) city.OnDestroyBuilding(this);
            Destroy(gameObject);
        }

        if (city != null) city.DisplayHP();
    }



    // ***** GAMEPLAYOWE *****



    // ***** EVENT *****



    void OnTriggerEnter(Collider coll)
    {
        // TODO: Np. Jak wchodzisz na obszar miasta
    }

    public void OnDestroy()
    {

    }



    // ***** EVENT *****
}
