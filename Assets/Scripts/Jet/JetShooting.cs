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

    [SerializeField]
    private AudioSource jetShot;

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
                jetShot.Play();
                bulletTrans.GetComponent<Bullet>().Init(direction,gameObject);
                ammunition--;
            }
        }
    }

#region PublicMethods
    public void AddAmmo(float ammo)
    {
        if(ammunition+ammo < maxAmmo) ammunition+=ammo;
        else ammunition=maxAmmo;
    }

    public float GetAmmo()
    {
        return ammunition;
    }


    public bool isShooting()
    {
        return shot;
    }

    public static JetShooting operator--(JetShooting shooting)
    {
        shooting.ammunition--;
        return shooting;
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
