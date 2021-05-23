using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Class responsible for the behaviour of the camera that follows the player
*/
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    ///Transform of an object that camera is going to follow
    private Transform target;

    [SerializeField]
    ///Reference to the spawn point of the target
    private GameObject spawnPoint;

    [Header("Camera Values")]
    [SerializeField]
    ///Smoothing speed of the camera
    private float smoothSpeed;
    [SerializeField]
    ///Distance from the target
    private Vector3 offset;
    [SerializeField]
    ///Camera rotation speed
    private float rotSpeed;

    [SerializeField]
    ///Delta of the Field of View value, used while boosting the jet
    private float fovDelta;
    [SerializeField]
    ///Maximum Field of View value that is allowed
    private float maxFOV;

    private float defaultFOV;
    private Vector3 velocity = Vector3.zero;

    private Camera camera;

    private JetSpawn spawn;

    void Start()
    {
        spawn = spawnPoint.GetComponent<JetSpawn>();
        camera = GetComponent<Camera>();
        defaultFOV = camera.fieldOfView;
    }

    void Update()
    {
        BoostingFieldOfView();
    }
    void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 desiredPos = target.position - target.TransformDirection(offset); //target.position + offset;
            Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
            Quaternion smoothRot = Quaternion.Lerp(transform.rotation, target.rotation,rotSpeed);
            transform.position = smoothPos;
            transform.rotation = smoothRot;
        }
    }

    void LateUpdate()
    {
        if(target == null)
        {
            if(spawn.jetReference != null)
                target = spawn.jetReference.transform;
        }
    }

/**
    Method that changes camera's Field Of View value, depending on whether the target is boosting or not
 */
    private void BoostingFieldOfView()
    {
        if(spawn.jetReference !=null)
        {
            JetMovement movement = spawn.jetReference.GetComponent<JetMovement>();
            if(movement.IsBoosting())
            {
                if(camera.fieldOfView < maxFOV)
                    camera.fieldOfView+=fovDelta;
                else camera.fieldOfView = maxFOV;
            }
            else
            {
                if(camera.fieldOfView > defaultFOV) camera.fieldOfView-=fovDelta;
                else camera.fieldOfView = defaultFOV;
            }
        }
    }
}
