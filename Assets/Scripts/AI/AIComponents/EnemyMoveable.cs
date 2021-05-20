using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveable
{   
    public EnemyMoveable(GameObject go) {
        rigidbody = go.GetComponent<Rigidbody>();
        speed = 0f;
    }

    private float speed;
    private Rigidbody rigidbody;

    public void Accelerate(Vector3 direction, float acceleration, float maxSpeed, bool debugSpeed)
    {
        if (speed <= maxSpeed)
            speed += acceleration * Time.fixedDeltaTime * 4f;
        else
            speed -= acceleration * Time.fixedDeltaTime * 4f;

        rigidbody.velocity = direction * speed;
        if(debugSpeed) Debug.Log("Speed: " + speed + " or " + rigidbody.velocity.magnitude + "/" + maxSpeed);
    }

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
    
    public void StrafeTowardsObject(GameObject target, Vector3 extraVec)
	{
        if(target)
            StrafeTowardsConstPos(target.transform.position + extraVec);
    }

    public bool CheckCrashCourse(Vector3 direction, float crashDangerRange)
	{
        RaycastHit hit;
        if (Physics.Raycast(rigidbody.transform.position, rigidbody.transform.TransformDirection(direction), out hit, crashDangerRange))
        {
            return EnemyMoveable_CrashIgnore.CrashExceptions(hit);
        }
        return false;
	}
}
