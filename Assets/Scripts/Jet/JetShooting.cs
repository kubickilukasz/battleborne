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
            if(ammunition > 0){
            GameObject bulletTrans = Instantiate(bullet,transform.position,Quaternion.identity) as GameObject;
            Camera cam = GetComponentInChildren<Camera>();
            Vector3 direction = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,cam.farClipPlane)) - transform.position;
            bulletTrans.GetComponent<Bullet>().Init(direction);
            ammunition--;
            }
            else{
                //Tutaj komunikat z GUI o braku amunicji
            }

        }
    }

    public void AddAmmo(float ammo){
        ammunition+=ammo;
    }
}
