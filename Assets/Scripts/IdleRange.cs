using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRange : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 50f);
    }
}