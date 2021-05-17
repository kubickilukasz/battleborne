using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxSpeed { get; set; }
    public float acc { get; set; }
    [SerializeField]
	private float maxSpeedNormal;
	[SerializeField]
	private float accNormal;

    private Rigidbody rigidbody;

    [SerializeField]
    public GameObject player;
    [SerializeField]
    private bool debug = false;

    private List<BossPart> bossParts;

    public int Health
    {
        get
        {
            int temp = 0;
            foreach (BossPart bp in bossParts)
            {
                if (bp != null)
                {
                    temp += bp.Health;
                }
            }
            return temp;
        }
    }

    public int MaxHealth { get; protected set; }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        maxSpeed = maxSpeedNormal;
        acc = accNormal;

        BossPart[] bossPartTab = GetComponentsInChildren<BossPart>();
        bossParts = new List<BossPart>(bossPartTab);
        foreach (BossPart bp in bossParts)
        {
            bp.Init(this);
            this.MaxHealth += bp.MaxHealth;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Health <= 0)
        {
            //? TODO: Osierocanie SingleSpawnow
            Destroy(gameObject);
        }

        if(debug) DisplayHP();
        Accelerate(Vector3.forward);
    }

    public void DisplayHP()
	{
        if (debug)
        {
            Debug.Log("========== BOSS STATUS ==========");
            Debug.Log("HP: " + this.Health + "/" + this.MaxHealth);
            Debug.Log("========== BOSS STATUS ==========");
        }
    }

    protected void Accelerate(Vector3 dir)
    {
        Vector3 newone = dir * acc * Time.fixedDeltaTime * 100;

        if (rigidbody.velocity.magnitude <= maxSpeed)
        {
            rigidbody.AddRelativeForce(newone);
        }
        else
        {
            rigidbody.AddRelativeForce(-newone);
        }
        if(debug) Debug.Log(rigidbody.velocity.magnitude);
    }

    public void OnHit(int hitPoints)
    {
        DisplayHP();
    }
}
