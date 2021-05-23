using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/**
Class responsible for the whole bullet mechanics. Stores data that is used when bullet hits a certain target.
*/
public class Bullet : MonoBehaviour
{
    [Header("Hitting")]
    [SerializeField]
    /// Amount of hit points each object will receive when hit by the bullet
    private int hitPoints; 

    [SerializeField]
    /// Point penalty for hitting the building(jet only)
    private int cityHitPenalty;

    [SerializeField]
    /// Point bonus for hitting the enemy(jet only)
    private int hitBonus;

    [SerializeField]
    /// Multiplier used to stack combo(jet only)
    private float comboMultiplier;

    [SerializeField]
    /// Reference to list with bonuses and penalties
    private BonusPenaltyList bonusPenaltyList;

    [Header("Physics")]

    [SerializeField]
    /// Determines how fast bullet will move
    private float bulletSpeed;

    [SerializeField]
    [Range(1.5f,4f)]
    /// Determines how long bullet will remain on the scene before being destroyed(unless it hit something in meantime)
    private float bulletLifetime;

    Rigidbody rigidbody;

    [Header("Effects")]
    [SerializeField]
    /// Effect instantiated when building is hit
    private GameObject cityHitEffect;

    [SerializeField]
    ///Visual effect instantiated when alien enemy is hit
    private GameObject alienHitEffect;
    [SerializeField]
    /// Sound effect instantiated when alien enemy is hit
    private AudioSource alienHit;

    [SerializeField]
    /// Visual effect instantiated when jet is hit
    private GameObject jetHitEffect;
    [SerializeField]
    /// Sound effect instantiated when jet is hit
    private AudioSource jetHit;

    [SerializeField]
    /// Visual effect instantiated when the ground is hit
    private GameObject groundHitEffect;

    private GameObject effect;

#region Sender Info
    private GameObject sender;

    private string senderTag;

    private bool isEnemyBullet = false;
#endregion

    /**
    Method used for initializing the bullet, invoked when shot
    */
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

    /**
    Method that determines whether hit object is enemy's bullet. Used when enemies shoot their bullets so that the collision is ignored
    @return Returns true if the bullet belongs to enemy, false otherwise
    */
    public bool IsEnemyBullet()
    {
        return isEnemyBullet;
    }
}
