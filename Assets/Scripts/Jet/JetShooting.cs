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

    [Header("ShootingDelay")]

    [SerializeField]
    private float maxDelay;
    
    private float delay = 0f;

    private bool shot = false;

    void Update()
    {
        Shoot();        
    }

    void Shoot()
    {
        ResetDelay();
        if(Input.GetMouseButton(0) && !shot)
        {
            if(ammunition > 0)
            {
                delay = maxDelay;
                shot = true;
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

#region PrivateMethods
    private void ResetDelay()
    {
        if(shot && delay > 0)
        {
            delay -= Time.deltaTime;
        }
        if(delay < 0)
        {
            shot = false;
            delay = 0f;
        }
    }
#endregion


}
