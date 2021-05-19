using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveable_CrashIgnore
{
    public static bool CrashExceptions(RaycastHit hit) {
        if(hit.transform) {
            if(!hit.transform.gameObject.GetComponent<Bullet>())
                return true;
        }
        
           return false;
    }
}
