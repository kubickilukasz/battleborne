using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Class that stores the data and performs actions reffering jet's shooting ability
*/
public class JetShooting : MonoBehaviour
{


    [Header("Ammunition")]
    [SerializeField]
    /// Amount of jet's ammunition
    private float ammunition = 150f; 

    /// Maximum amount of jet's ammunition
    public float maxAmmo = 300f;

    [Header("Bullet")]
    [SerializeField]
    /// Place for bullet's prefab
    private GameObject bullet;

    [SerializeField]
    /// Multiplier for bullet's direction vector. Used to make it go further.
    private float dirMultiplier;

    [SerializeField]
    /// Sound used when a shooting action is performed
    private AudioSource jetShot;

    [Header("ShootingDelay")]

    [SerializeField]
    /// Delay for shooting. Used so that bullets don't spawn too fast
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
    /**
    Method used for increasing amount of ammunition after receiving a certain amount of it.
    @param ammo Amount of received ammunition
    */
    public void AddAmmo(float ammo)
    {
        if(ammunition+ammo < maxAmmo) ammunition+=ammo;
        else ammunition=maxAmmo;
    }

    public float GetAmmo()
    {
        return ammunition;
    }

    /**
    Method stating whether jet is currently shooting or not
    @return Returns true if jet is shooting, false otherwise
    */
    public bool isShooting()
    {
        return shot;
    }

    /**
    Operator used for reducing the amount of ammunition
    */

    public static JetShooting operator--(JetShooting shooting)
    {
        shooting.ammunition--;
        return shooting;
    }
#endregion

#region PrivateMethods

    /**
    Method used to reset shooting delay
    */
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
