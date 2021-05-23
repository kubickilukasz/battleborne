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
    JetSpawn jetSpawn; /// Reference to JetSpawn

    [SerializeField]
    RectTransform leftAim; /// Reference to left part of viewfinder

    [SerializeField]
    RectTransform rightAim; /// Reference to right part of viewfinder

    [SerializeField]
    Image healthBar; /// Reference to image, which represents Health Bar

    [SerializeField]
    Image ammoBar; /// Reference to image, which represents Ammo Bar

    [Space()]

    [SerializeField]
    float smooth; /// Smooth of viewfinder

    [SerializeField]
    float maxSpeed; /// Max speed of viewfinder

    [SerializeField]
    float focusPosX = 70f; /// Position of X in focus mode

    [SerializeField]
    float recoilPosX = 150f; /// Position of X in recoil mode

    [SerializeField]
    bool isFocusing = true; /// Is viewfinder focusing

    [SerializeField]
    float rangeRayToCalculateAim; /// max range of ray to calculate position of viewfinder

    [SerializeField]
    Camera jetCamera; /// Reference to Camera

    [SerializeField]
    LayerMask maskForRay; /// Mask for ray to calculate position fo viewfinder

    [SerializeField]
    float smoothOfAimRay; /// mooth of viewfinder

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
