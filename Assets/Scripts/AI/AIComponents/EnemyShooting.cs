using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Auxiliary component class responsible for shooting projectiles
*/
public class EnemyShooting
{
    public EnemyShooting(GameObject go) {
        gameObject = go;
    }
    private GameObject gameObject;

    /**
    * Checks if selected position is in selected range
    * @param position Selected position
    * @param minDistance Selected range
    * @return Is position in range
    */
    public bool IsPositionInRange(Vector3 position, float minDistance)
    {
        if ((position - gameObject.transform.position).magnitude < minDistance)
            return true;
        else
            return false;
    }

    /**
    * Checks if angle between selected pos lesser than selcted angle
    * @param position Selected position
    * @param minAngle Selected angle
    * @return Is angle lesser than selected
    */
    public bool IsAngleInRange(Vector3 position, float minAngle)
    {
        float angle = Vector3.Angle(gameObject.transform.forward, position-gameObject.transform.position);
        if(angle <= minAngle)
            return true;
        else
            return false;
    }

    /**
    * Checks if there are no obstacles before shooting towards selected direction
    * @param direction Selected direction
    * @param shootRange Range to consider obstacles
    * @return Is bullet on crash course - returns true, if there are potential obstacles
    */
    public bool CheckCrashCollisions(Vector3 direction, float shootRange)
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(direction), out hit, shootRange))
        {
            return CrashExceptions(hit);
        }
        return false;
    }


    /**
    * Checks if provided obstacle hit can be ignored
    * @param hit Obstacle to consider
    * @return Should hit be considered
    */
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
