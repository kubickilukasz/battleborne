using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting
{
    public EnemyShooting(GameObject go) {
        gameObject = go;
    }
    private GameObject gameObject;

    public bool IsPositionInRange(Vector3 position, float minDistance)
    {
        if ((position - gameObject.transform.position).magnitude < minDistance)
            return true;
        else
            return false;
    }

    public bool IsAngleInRange(Vector3 position, float minAngle)
    {
        float angle = Vector3.Angle(gameObject.transform.forward, position-gameObject.transform.position);
        if(angle <= minAngle)
            return true;
        else
            return false;
    }

    public bool CheckCrashCollisions(Vector3 direction, float shootRange)
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(direction), out hit, shootRange))
        {
            return EnemyShooting_BulletCrash.CrashExceptions(hit);
        }
        return false;
    }
}
