using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    private Transform target; ///Transform of an object that camera is going to follow

    [SerializeField]
    private GameObject spawnPoint; ///Reference to the spawn point of the targer

    [Header("Camera Values")]
    [SerializeField]
    private float smoothSpeed; ///Smoothing speed of the camera
    [SerializeField]
    private Vector3 offset; ///Distance from the target
    [SerializeField]
    private float rotSpeed; ///Camera rotation speed

    [SerializeField]
    private float fovDelta; ///Delta of the Field of View value, used while boosting the jet
    [SerializeField]
    private float maxFOV;  ///Maximum Field of View value that is allowed

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
