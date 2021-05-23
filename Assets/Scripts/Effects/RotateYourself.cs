using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Simple component to rotate attached object
*/
public class RotateYourself : MonoBehaviour
{

    [SerializeField]
    private float speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAroundLocal(Vector3.up, speed * Time.deltaTime);
    }
}
