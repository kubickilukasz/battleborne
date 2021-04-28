using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class City : MonoBehaviour
{
    //Inne/Gameplayowe
    [SerializeField]
    private List<GameObject> buildingObjects = new List<GameObject>();
    private List<CityBuilding> buildings = new List<CityBuilding>();
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
        foreach (GameObject buildingObject in buildingObjects)
            buildings.Add(buildingObject.GetComponent<CityBuilding>());

        foreach (CityBuilding building in buildings)
        {
            building.Town = this;
        }

        cityDestroyedEvent = new UnityEvent();
        cityDestroyedEvent.AddListener(ListenerPlaceholder.ListenDestroyCity);
    }



    // ***** UPDATE *****



    // ***** GAMEPLAYOWE *****



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



    // ***** EVENT *****



    void OnTriggerEnter(Collider coll)
    {
        // Nie wiem jak wejdzie sie w kolizej
    }

    public void OnDestroy()
    {

    }



    // ***** EVENT *****
}
