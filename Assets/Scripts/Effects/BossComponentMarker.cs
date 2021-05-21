using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossComponentMarker : MonoBehaviour
{
    [SerializeField]
    BossPart bossPart;

    void Update()
    {
        if(bossPart.Health <= 0){
            Destroy(gameObject);
        }
    }
}
