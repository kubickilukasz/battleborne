using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class City : MonoBehaviour
{
    //Inne/Gameplayowe
    private List<CityBuilding> buildings;
    [SerializeField]
    private bool debug = false;

    private Collider collider;
    UnityEvent cityDestroyedEvent;

    public int Health
    {
        get
        {
            int temp = 0;
            foreach (CityBuilding building in buildings)
            {
                if (building != null) //Co jesli jeden budynek zostanie zniszczony?
                {
                    temp += building.Health;
                }
            }
            return temp;
        }
    }

    public int MaxHealth
    {
        get
        {
            int temp = 0;
            foreach (CityBuilding building in buildings)
            {
                if (building != null)
                {
                    temp += building.MaxHealth;
                }
            }
            return temp;
        }
    }

    // ***** UPDATE *****



    void FixedUpdate()
    {

    }

    void Start()
    {
        CityBuilding[] buildingsTable = GetComponentsInChildren<CityBuilding>();
        buildings = new List<CityBuilding>(buildingsTable);
        foreach (CityBuilding cb in buildings)
            cb.Town = this;

        cityDestroyedEvent = new UnityEvent();
        cityDestroyedEvent.AddListener(ListenerPlaceholder.ListenDestroyCity);
    }



    public void OnHit(int hitPoints)
    {
        //TODO: NP. Update hp bara
    }

    public void OnDestroyBuilding(CityBuilding building)
	{
        buildings.Remove(building);
        if (buildings.Count == 0)
        {
            cityDestroyedEvent.Invoke();
            Destroy(gameObject);
        }

        DisplayHP();
    }

    public void DisplayHP()
	{
        if (debug)
        {
            Debug.Log("========== CITY STATUS ==========");
            Debug.Log("HP: " + this.Health + "/" + this.MaxHealth);
            Debug.Log("Ilosc budynkow: " + this.buildings.Count);
            Debug.Log("========== CITY STATUS ==========");
        }
    }


    public void OnDestroy()
    {

    }
}
