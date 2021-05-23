using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Simple component to rotate attached object
*/
public class RotateYourself : MonoBehaviour
{

    [SerializeField]
    /// Speed of rotation
    private float speed = 1;

    void Update()
    {
        transform.RotateAroundLocal(Vector3.up, speed * Time.deltaTime);
    }
}
