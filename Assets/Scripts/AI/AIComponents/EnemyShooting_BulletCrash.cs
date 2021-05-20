using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting_BulletCrash
{
    public static bool CrashExceptions(RaycastHit hit) {
        if(hit.transform) {
            if(
                !hit.transform.gameObject.GetComponent<CityBuilding>() ||
                !hit.transform.gameObject.GetComponent<JetMovement>() ||
                !hit.transform.gameObject.GetComponent<AIPlayerPlaceholder>()
            )
                return true;
        }

        return false;
    }
}
