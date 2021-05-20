using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    private Transform target;

    [SerializeField]
    private GameObject spawnPoint;

    [Header("Camera Values")]
    [SerializeField]
    private float smoothSpeed;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float rotSpeed;

    [SerializeField]
    private float fovDelta;
    [SerializeField]
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
            Vector3 desiredPos = target.position - target.TransformDirection(offset);//target.position + offset;
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

    private void BoostingFieldOfView()
    {
        JetMovement movement = spawn?.jetReference.GetComponent<JetMovement>();
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
