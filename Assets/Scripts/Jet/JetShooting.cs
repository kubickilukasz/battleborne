using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetShooting : MonoBehaviour
{
    [Header("Ammunition")]
    [SerializeField]
    private float ammunition = 150f;

    [Header("Bullet")]
    [SerializeField]
    private GameObject bullet;

    void FixedUpdate()
    {
        Shoot();        
    }

    void Shoot(){
        if(Input.GetMouseButtonDown(0)){

            GameObject bulletTrans = Instantiate(bullet,transform.position,Quaternion.identity) as GameObject;
            Vector3 direction = Input.mousePosition - transform.position;
            bulletTrans.GetComponent<Bullet>().Init(direction);
            ammunition--;

        }
    }

    public void AddAmmo(float ammo){
        ammunition+=ammo;
    }
}
