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


    public void Init(Vector3 direction){

        rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddForce(direction*bulletSpeed*Time.fixedDeltaTime, ForceMode.Impulse);

        Destroy(gameObject,5f);

    }

    void OnTriggerEnter(Collider other) {
        // if(other.tag == "city"){
        //     //
        // }
        // else if(other.tag == "alien"){
            AIEnemy enemy = other.GetComponent<AIEnemy>();
            if(enemy != null){
                enemy.OnHit(hitPoints);
                Destroy(gameObject);
            }
        //}
    }

}
