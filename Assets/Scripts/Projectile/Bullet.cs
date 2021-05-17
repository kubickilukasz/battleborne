using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Hitting")]
    [SerializeField]
    private int hitPoints;

    [SerializeField]
    private int cityHitPenalty;

    [SerializeField]
    private int hitBonus;

    [SerializeField]
    private float comboMultiplier;

    [Header("Physics")]

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    [Range(1.5f,4f)]
    private float bulletLifetime;

    [SerializeField]
    private GameObject invisibleWall;
    Rigidbody rigidbody;

    [Header("Effects")]
    [SerializeField]
    private GameObject cityHitEffect;

    [SerializeField]
    private GameObject alienHitEffect;

    [SerializeField]
    private GameObject jetHitEffect;

    [SerializeField]
    private GameObject groundHitEffect;

    private GameObject effect;

    private GameObject sender;


    public void Init(Vector3 direction, GameObject sender)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddRelativeForce(direction*bulletSpeed*Time.fixedDeltaTime, ForceMode.Impulse);
        this.sender = sender;

        Destroy(gameObject,bulletLifetime);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "City")
        {
            CityBuilding building = other.GetComponent<CityBuilding>();
            if(building != null)
            {
                building.OnHit(hitPoints);
                effect = Instantiate(cityHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                if(sender.tag == "Jet")
                {
                    JetPoints points = sender.GetComponent<JetPoints>();
                    points.DecreasePoints(cityHitPenalty);
                    points.ResetCombo();
                }
                Destroy(gameObject);
            }
        }
        else if(other.tag == "Alien")
        {
            if(other.tag != sender.tag)
            {
                AIEnemy enemy = other.GetComponent<AIEnemy>();
                if(enemy != null)
                {
                    
                    JetPoints points = sender.GetComponent<JetPoints>();
                    points.AddPoints(hitBonus);
                    points.StackCombo(comboMultiplier);
                    if(points.isMaxCombo())
                        enemy.OnHit(points.GetBonus()*hitPoints);
                    else
                        enemy.OnHit(hitPoints);
                    effect = Instantiate(alienHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                    Destroy(gameObject);
                }
            }
        }
        else if(other.tag == "Jet")
        { 
            if(other.tag != sender.tag)
            {
                JetHealth jet = other.GetComponent<JetHealth>();
                if(jet != null)
                {
                    jet.OnHit(hitPoints);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            BossPart boss = other.GetComponent<BossPart>();
            if(boss != null && sender.tag == "Jet")
            {
                    JetPoints points = sender.GetComponent<JetPoints>();
                    points.AddPoints(hitBonus);
                    points.StackCombo(comboMultiplier);
                    if(points.isMaxCombo())
                        boss.OnHit(points.GetBonus()*hitPoints);
                    else
                        boss.OnHit(hitPoints);
                    effect = Instantiate(alienHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                    Destroy(gameObject);
            }
            else
            {
                if(other.tag != "Ammo")
                {
                    InvisibleWall wall = other.GetComponent<InvisibleWall>();
                    if(wall != null) return;
                    effect = Instantiate(groundHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                    Destroy(gameObject);
                }
            }
        }
    }

}
