using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Hitting")]
    [SerializeField]
    private int hitPoints;

    [Header("Physics")]
    Rigidbody rigidbody;

    [SerializeField]
    private float bulletSpeed;


    public void Init(Vector3 direction)
    {

        rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddRelativeForce(direction*bulletSpeed*Time.fixedDeltaTime, ForceMode.Impulse);

        Destroy(gameObject,5f);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "City")
        {
            // CityBuilding building = other.GetComponent<CityBuilding>();
            // if(building != null)
            // {
            //     building.OnHit(hitPoints);
            //     Destroy(gameObject);
            // }
        }
        else if(other.tag == "Alien")
        {
            AIEnemy enemy = other.GetComponent<AIEnemy>();
            if(enemy != null)
            {
                enemy.OnHit(hitPoints);
                Destroy(gameObject);
            }
        }
        else if(other.tag == "Jet")
        {
            JetShooting jet = other.GetComponent<JetShooting>();
            if(jet != null)
            {
                jet.OnHit(hitPoints);
                Destroy(gameObject);
            }
        }
    }

}
