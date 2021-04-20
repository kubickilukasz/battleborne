using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerPlaceholder : MonoBehaviour
{
    public static AIPlayerPlaceholder instance;

    public float speed;
    [SerializeField]
    private bool debug = true;

    void Awake()
	{
        instance = this;
	}

    void Start()
    {
        speed = 5f;
    }

    void FixedUpdate()
    {
        //Sterowanie: WSADQE
        if (debug)
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime, Input.GetAxis("UpDown") * speed * Time.fixedDeltaTime, Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime));
        }
    }
}
