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
        if(other.tag == "Jet"){
                JetShooting jetShot = other.GetComponent<JetShooting>();
                jetShot.AddAmmo(ammoData.ammo);
                Destroy(gameObject);
            }
        }    
}
