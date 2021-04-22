using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRange : MonoBehaviour
{
    [SerializeField]
    private bool debug = false;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 5f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 50f);
        }
    }
}