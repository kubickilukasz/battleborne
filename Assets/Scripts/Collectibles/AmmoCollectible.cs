using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class AmmoCollectible : MonoBehaviour
{
    private JetData jetData;

    

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Jet"){
            jetData = other.GetComponent<JetMovement>().jetData;
            if(jetData != null){
                jetData.ammo+=20f;
                Destroy(gameObject);
            }
        }    
    }
}
