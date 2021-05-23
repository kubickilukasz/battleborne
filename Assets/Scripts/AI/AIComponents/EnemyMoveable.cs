using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Auxiliary component class responsible for moving enemy's body
*/
public class EnemyMoveable
{   
    public EnemyMoveable(GameObject go) {
        rigidbody = go.GetComponent<Rigidbody>();
        speed = 0f;
    }

    /**
    * Current speed of an enemy
    */
    private float speed;
    private Rigidbody rigidbody;

    /**
    * Sets enemy's speed to selected direction
    * @param direction Selected direction
    * @param acceleration Chosen acceleration
    * @param maxSpeed Maximum speed that can be reached by an enemy
    * @param debugSpeed Should current speed be displayed to debug console
    */
    public void Accelerate(Vector3 direction, float acceleration, float maxSpeed, bool debugSpeed)
    {
        if (speed <= maxSpeed)
            speed += acceleration * Time.fixedDeltaTime * 4f;
        else
            speed -= acceleration * Time.fixedDeltaTime * 4f;

        rigidbody.velocity = direction * speed;
        if(debugSpeed) Debug.Log("Speed: " + speed + " or " + rigidbody.velocity.magnitude + "/" + maxSpeed);
    }

    /**
    * Rotates enemy to selected direction
    * @param idlepos Selected direction
    */
    public void StrafeTowardsConstPos(Vector3 idlepos)
    {
        if(rigidbody != null) {
            if (idlepos != null)
            {
                Vector3 direction = (idlepos - rigidbody.transform.position).normalized;
                Quaternion quaternion = Quaternion.LookRotation(direction);
                rigidbody.transform.rotation = Quaternion.RotateTowards(rigidbody.transform.rotation, quaternion, Time.fixedDeltaTime * 100f);
            }

            float t = rigidbody.velocity.magnitude;
            Vector3 v = Vector3.forward;
            v = rigidbody.rotation.normalized * v;
            v *= t;

            rigidbody.velocity = v;
        }
    }
    
    /**
    * Rotates enemy to selected object
    * @param target Selected object
    * @param extraVec Extra displacement
    */
    public void StrafeTowardsObject(GameObject target, Vector3 extraVec)
	{
        if(target)
            StrafeTowardsConstPos(target.transform.position + extraVec);
    }

    /**
    * Checks if there are obstacles in selected direction
    * @param direction Selected direction
    * @param crashDangerRange Range in which ale obstacles will be checked
    * @return Is enemy on crash course - returns true, if there are potential obstacles
    */
    public bool CheckCrashCourse(Vector3 direction, float crashDangerRange)
	{
        RaycastHit hit;
        if (Physics.Raycast(rigidbody.transform.position, rigidbody.transform.TransformDirection(direction), out hit, crashDangerRange))
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
            if(!hit.transform.gameObject.GetComponent<Bullet>())
                return true;
        }
        
           return false;
    }
}
