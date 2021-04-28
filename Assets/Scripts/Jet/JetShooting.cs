using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JetShooting : MonoBehaviour
{


    [Header("Ammunition")]
    [SerializeField]
    private float ammunition = 150f;

    public float maxAmmo = 300f;

    [Header("Bullet")]
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float dirMultiplier;


    void Update()
    {
        Shoot();        
    }

    void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(ammunition > 0)
            {
                GameObject bulletTrans = Instantiate(bullet,transform.position,Quaternion.identity) as GameObject;
                Vector3 direction = transform.forward*dirMultiplier;
                bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
                ammunition--;
            }
        }
    }

#region PublicMethods
    public void AddAmmo(float ammo)
    {
        ammunition+=ammo;
    }

    public float GetAmmo()
    {
        return ammunition;
    }

    public void SetMaxAmmo()
    {
        ammunition = maxAmmo;
    }

#endregion

}
