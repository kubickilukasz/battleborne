using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{

    [SerializeField]
    JetSpawn jetSpawn;

    [SerializeField]
    RectTransform leftAim;

    [SerializeField]
    RectTransform rightAim;

    [SerializeField]
    Image healthBar;

    [SerializeField]
    Image ammoBar;

    [Space()]

    [SerializeField]
    float smooth;

    [SerializeField]
    float maxSpeed;

    [SerializeField]
    float focusPosX = 70f;

    [SerializeField]
    float recoilPosX = 150f;

    [SerializeField]
    bool isFocusing = true;

    [SerializeField]
    float rangeRayToCalculateAim;

    [SerializeField]
    Camera jetCamera;

    [SerializeField]
    LayerMask maskForRay;

    [SerializeField]
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
