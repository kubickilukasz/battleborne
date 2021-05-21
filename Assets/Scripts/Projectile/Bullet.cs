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

    [SerializeField]
    private BonusPenaltyList bonusPenaltyList;

    [Header("Physics")]

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    [Range(1.5f,4f)]
    private float bulletLifetime;

    Rigidbody rigidbody;

    [Header("Effects")]
    [SerializeField]
    private GameObject cityHitEffect;

    [SerializeField]
    private GameObject alienHitEffect;
    [SerializeField]
    private AudioSource alienHit;

    [SerializeField]
    private GameObject jetHitEffect;
    [SerializeField]
    private AudioSource jetHit;

    [SerializeField]
    private GameObject groundHitEffect;

    private GameObject effect;

#region Sender Info
    private GameObject sender;

    private string senderTag;

    private bool isEnemyBullet = false;
#endregion


    public void Init(Vector3 direction, GameObject sender)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddRelativeForce(direction*bulletSpeed*Time.fixedDeltaTime, ForceMode.Impulse);
        this.sender = sender;
        senderTag = sender.tag;
        if(senderTag != "Jet") isEnemyBullet = true;
        Destroy(gameObject,bulletLifetime);

    }

    void OnTriggerEnter(Collider other)
    {
        Bullet enemyBullet = other.GetComponent<Bullet>();
        if(enemyBullet != null)
            if(enemyBullet.IsEnemyBullet() == isEnemyBullet) return;

        string colliderTag = other.tag;        
        if(colliderTag == "City")
        {
            CityBuilding building = other.GetComponent<CityBuilding>();
            if(building != null)
            {
                building.OnHit(hitPoints);
                effect = Instantiate(cityHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                if(senderTag == "Jet")
                {
                    if(sender != null)
                    {
                        JetPoints points = sender.GetComponent<JetPoints>();
                        points.DecreasePoints(cityHitPenalty);
                        points.ResetCombo();
                    }
                    else
                    {
                        bonusPenaltyList.AddPenalty(cityHitPenalty);
                    }
                }
                Destroy(gameObject);
            }
        }
        else if(colliderTag == "Alien")
        {
            if(colliderTag != senderTag)
            {
                AIEnemy enemy = other.GetComponent<AIEnemy>();
                if(enemy != null)
                {
                    if(sender != null)
                    {
                        JetPoints points = sender.GetComponent<JetPoints>();
                        points.AddPoints(hitBonus);
                        points.StackCombo(comboMultiplier);
                        if(points.isMaxCombo())
                            enemy.OnHit(points.GetBonus()*hitPoints);
                        else
                            enemy.OnHit(hitPoints);
                    }
                    else
                    {
                        bonusPenaltyList.AddBonus(hitBonus);
                        enemy.OnHit(hitPoints);
                    }
                    alienHit.Play();
                    effect = Instantiate(alienHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                    Destroy(gameObject);
                }
            }
        }
        else if(colliderTag == "Jet")
        { 
            if(colliderTag != senderTag)
            {
                JetHealth jet = other.GetComponent<JetHealth>();
                if(jet != null)
                {
                    jet.OnHit(hitPoints);
                    jetHit.Play();
                    effect = Instantiate(jetHitEffect,transform.position,Quaternion.LookRotation(-transform.forward),other.transform) as GameObject;
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            BossPart bossPart = other.GetComponent<BossPart>();
            if(bossPart != null)
            {
                    if(senderTag=="Jet")
                    {
                        Debug.Log(sender);
                        if(sender != null)
                        {
                            JetPoints points = sender.GetComponent<JetPoints>();
                            points.AddPoints(hitBonus);
                            points.StackCombo(comboMultiplier);
                            if(points.isMaxCombo())
                                bossPart.OnHit(points.GetBonus()*hitPoints);
                            else
                                bossPart.OnHit(hitPoints);
                        }
                        else
                        {
                            bonusPenaltyList.AddBonus(hitBonus);
                            bossPart.OnHit(hitPoints);
                        }
                        alienHit.Play();
                        effect = Instantiate(alienHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                        Destroy(gameObject);
                    }
            }
            else
            {
                if(other.tag != "Ammo")
                {
                    InvisibleWall wall = other.GetComponent<InvisibleWall>();
                    Boss boss = other.GetComponent<Boss>(); 
                    if(wall != null) return;
                    if(boss!=null)
                    {
                        Debug.Log(sender);
                        if(senderTag == "Jet")
                        {
                            alienHit.Play();
                            effect = Instantiate(alienHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                            Destroy(gameObject);
                        }
                        else return;
                    }
                    effect = Instantiate(groundHitEffect,transform.position,Quaternion.LookRotation(-transform.forward)) as GameObject;
                    Destroy(gameObject);
                }
            }
        }
    }

    public bool IsEnemyBullet()
    {
        return isEnemyBullet;
    }
}
