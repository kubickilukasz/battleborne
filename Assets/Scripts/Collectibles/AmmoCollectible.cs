using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
/**
Class responsible for storing data of an ammo collectible and performing certain actions when it's collected
*/
public class AmmoCollectible : MonoBehaviour
{

    [Header("AmmoData")]
    public AmmoData ammoData; ///Serialized object containing data about the ammunition in this collectible

    
    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Jet")
        {
                JetShooting jetShot = other.GetComponent<JetShooting>();
                jetShot.AddAmmo(ammoData.ammo);
                Destroy(gameObject);
        }
        else
        {
            if(other.tag != "Alien")
            {
                Bullet bullet = other.GetComponent<Bullet>();
                InvisibleWall wall = other.GetComponent<InvisibleWall>();
                if(bullet == null && wall == null)
                    Destroy(gameObject);
            }
        }
    }    
}
