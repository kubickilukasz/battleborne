using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AmmoCollectible : MonoBehaviour
{

    [Header("AmmoData")]
    public AmmoData ammoData;

    
    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Jet")
        {
                JetShooting jetShot = other.GetComponent<JetShooting>();
                if(jetShot.GetAmmo() + ammoData.ammo < jetShot.maxAmmo)
                {
                    jetShot.AddAmmo(ammoData.ammo);
                    Destroy(gameObject);
                }
                else
                {
                    jetShot.SetMaxAmmo();
                    Destroy(gameObject);
                }
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
