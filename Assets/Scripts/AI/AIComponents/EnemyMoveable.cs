using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveable
{   
    public EnemyMoveable(GameObject go) {
        rigidbody = go.GetComponent<Rigidbody>();
    }

    private Rigidbody rigidbody;

    public void Accelerate(Vector3 direction, float acceleration, float maxSpeed, bool debugSpeed)
    {
        Vector3 newone = direction * acceleration * Time.fixedDeltaTime * 100;

        if (rigidbody.velocity.magnitude <= maxSpeed)
        {
            rigidbody.AddRelativeForce(newone);
        }
        else
        {
            rigidbody.AddRelativeForce(-newone);
        }

        if(debugSpeed) Debug.Log("Speed: " + rigidbody.velocity.magnitude + "/" + maxSpeed);
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
