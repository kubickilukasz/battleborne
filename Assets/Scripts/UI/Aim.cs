using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
Class responsible for controll viewfinder in play mode
*/
public class Aim : MonoBehaviour
{

    [SerializeField]
    /// Reference to JetSpawn
    JetSpawn jetSpawn; 

    [SerializeField]
    /// Reference to left part of viewfinder
    RectTransform leftAim; 

    [SerializeField]
    /// Reference to right part of viewfinder
    RectTransform rightAim; 

    [SerializeField]
    /// Reference to image, which represents Health Bar
    Image healthBar; 

    [SerializeField]
    /// Reference to image, which represents Ammo Bar
    Image ammoBar; 

    [Space()]

    [SerializeField]
    /// Smooth of viewfinder
    float smooth; 

    [SerializeField]
    /// Max speed of viewfinder
    float maxSpeed; 

    [SerializeField]
    /// Position of X in focus mode
    float focusPosX = 70f; 

    [SerializeField]
    /// Position of X in recoil mode
    float recoilPosX = 150f; 

    [SerializeField]
    /// Is viewfinder focusing
    bool isFocusing = true; 

    [SerializeField]
    /// max range of ray to calculate position of viewfinder
    float rangeRayToCalculateAim; 

    [SerializeField]
    /// Reference to Camera
    Camera jetCamera;

    [SerializeField]
    /// Mask for ray to calculate position fo viewfinder
    LayerMask maskForRay; 

    [SerializeField]
    /// mooth of viewfinder
    float smoothOfAimRay; 

    float refCurrentVelocity;
    Vector3 tempPosition;
    RectTransform rectTransform;
    Vector2 positionAim;
    Vector2 positionAimRef;

    void Start()
    {
        tempPosition = new Vector3(leftAim.anchoredPosition.x,0,0);
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        isFocusing = !Input.GetMouseButtonDown(0);
        if(isFocusing)
        {
            tempPosition = new Vector3(Mathf.SmoothDamp(tempPosition.x, focusPosX, ref refCurrentVelocity, smooth, maxSpeed),0,0);
        }else{
            tempPosition = new Vector3(Mathf.SmoothDamp(tempPosition.x, recoilPosX, ref refCurrentVelocity, 0, maxSpeed),0,0);
        }
        leftAim.anchoredPosition = -tempPosition;
        rightAim.anchoredPosition = tempPosition;

        if(jetSpawn != null && jetSpawn.jetReference != null){

            JetShooting jetShooting     =   jetSpawn?.jetReference?.GetComponent<JetShooting>();
            JetHealth jetHealth         =   jetSpawn?.jetReference?.GetComponent<JetHealth>();

            if(jetShooting != null || jetHealth != null )
            {
                healthBar.fillAmount = jetHealth.GetHealth() / (float)jetHealth.GetMaxHealth();
                ammoBar.fillAmount = jetShooting.GetAmmo() / jetShooting.maxAmmo;

                RaycastHit hit;
                if(Physics.Raycast(jetShooting.transform.position, jetShooting.transform.forward, out hit, rangeRayToCalculateAim, maskForRay)){
                    positionAim = (Vector2)jetCamera.WorldToScreenPoint(hit.point) - (jetCamera.pixelRect.size / 2);
                }else{
                    positionAim = (Vector2)jetCamera.WorldToScreenPoint(jetShooting.transform.position + jetShooting.transform.forward * (rangeRayToCalculateAim * 0.25f)) - (jetCamera.pixelRect.size / 2); 
                }
                rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, positionAim, ref positionAimRef, smoothOfAimRay);

            }
        }

        
    }



}
