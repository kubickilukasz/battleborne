using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerPlaceholder : MonoBehaviour
{
    public static AIPlayerPlaceholder instance;

    public float speed;

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
        //Zmienilem jump na q/e w input manager
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime, Input.GetAxis("Jump") * speed * Time.fixedDeltaTime, Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime));
    }
}
