using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class JetShooting : MonoBehaviour
{

    public UnityEvent onDestroyEvent;

    [Header("Ammunition")]
    [SerializeField]
    private float ammunition = 150f;

    public float maxAmmo = 300f;

    [Header("Bullet")]
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float dirMultiplier;

    [Header("Jet Health")]
    [SerializeField]
    private int health;

    [SerializeField]
    private int maxHealth = 100;

    [Header("Explosion")]
    [SerializeField]
    private GameObject explosion;


    void Start()
    {
        health = maxHealth;
    }

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
                bulletTrans.GetComponent<Bullet>().Init(direction);
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

    public int GetHealth()
    {
        return health;        
    }

    public void SetMaxAmmo()
    {
        ammunition = maxAmmo;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void OnHit(int hitPoints)
    {
        health -= hitPoints;
        if(health <= 0)
        {
            GameObject boom = Instantiate(explosion,transform.position,Quaternion.identity) as GameObject;
            GetComponent<Renderer>().enabled = false;
            onDestroyEvent.Invoke();
            Destroy(gameObject,2f);
        }
    }
#endregion

}
