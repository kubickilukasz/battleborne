using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/**
* Representation of the city
*/
public class City : MonoBehaviour
{
    
    /**
    * List of buildings in the city
    */
    private List<CityBuilding> buildings;
    /**
    * Debug variable to check city's health
    */
    [SerializeField]
    private bool debug = false;

    /**
    * Event fired when city will be destroyed
    */
    public UnityEvent cityDestroyedEvent;

    /**
    * Current health of the city
    */
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

    /**
    * Maximum health of the city
    */
    public int MaxHealth { get; protected set; }

    
    void Start()
    {
        CityBuilding[] buildingsTable = GetComponentsInChildren<CityBuilding>();
        buildings = new List<CityBuilding>(buildingsTable);
        foreach (CityBuilding cb in buildings)
        {
            cb.Init(this);
            this.MaxHealth += cb.MaxHealth;
        }
    }



    /**
    * Behaviour after one of city's building has been destroyed
    *
    * @param building Building destroyed
    */
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

    /**
    * Displays HP of the city if debugging
    */
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

    /**
    * Retruns random building from the city (for enemies)
    *
    * @return Random, alive building
    */
    public GameObject GetRandomBuilding()
    {
        if(buildings != null && buildings.Count > 0)
            return buildings[Random.Range(0, buildings.Count-1)].gameObject;

        return null;
    }
}
