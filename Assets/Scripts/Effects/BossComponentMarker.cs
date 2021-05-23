using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Represents marker of the boss
*/
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
