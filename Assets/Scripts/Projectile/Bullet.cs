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

    [Header("Effects")]
    [SerializeField]
    private GameObject cityHitEffect;

    [SerializeField]
    private GameObject alienHitEffect;

    [SerializeField]
    private GameObject jetHitEffect;

    [SerializeField]
    private GameObject groundHitEffect;

    private GameObject effect;

    private GameObject sender;

    [SerializeField]
    private GameObject invisibleWall;

    public void Init(Vector3 direction, GameObject sender)
    {

        rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddRelativeForce(direction*bulletSpeed*Time.fixedDeltaTime, ForceMode.Impulse);

        this.sender = sender;

        Destroy(gameObject,5f);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "City")
        {
            CityBuilding building = other.GetComponent<CityBuilding>();
            if(building != null)
            {
                building.OnHit(hitPoints);
                effect = Instantiate(cityHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                Destroy(gameObject);
            }
        }
        else if(other.tag == "Alien")
        {
            if(other.tag != sender.tag)
            {
                AIEnemy enemy = other.GetComponent<AIEnemy>();
                if(enemy != null)
                {
                    enemy.OnHit(hitPoints);
                    effect = Instantiate(alienHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                    Destroy(gameObject);
                }
            }
        }
        else if(other.tag == "Jet")
        { 
            if(other.tag != sender.tag)
            {
                JetHealth jet = other.GetComponent<JetHealth>();
                if(jet != null)
                {
                    jet.OnHit(hitPoints);
                    effect = Instantiate(jetHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if(other.tag != "Ammo")
            {
                InvisibleWall wall = other.GetComponent<InvisibleWall>();
                if(wall != null) return;
                effect = Instantiate(groundHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                Destroy(gameObject);
            }
        }
    }

}
