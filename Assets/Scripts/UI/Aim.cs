using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{

    [SerializeField]
    JetMovement movement; // TODO zmienić na JetShooting

    [SerializeField]
    RectTransform leftAim;

    [SerializeField]
    RectTransform rightAim;

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

    float refCurrentVelocity;
    Vector3 tempPosition;

    void Start(){
        tempPosition = new Vector3(leftAim.anchoredPosition.x,0,0);
    }

    void Update()
    {
        if(isFocusing){
            tempPosition = new Vector3(Mathf.SmoothDamp(tempPosition.x, focusPosX, ref refCurrentVelocity, smooth, maxSpeed),0,0);
        }else{
            tempPosition = new Vector3(Mathf.SmoothDamp(tempPosition.x, recoilPosX, ref refCurrentVelocity, smooth, maxSpeed),0,0);
        }
        leftAim.anchoredPosition = -tempPosition;
        rightAim.anchoredPosition = tempPosition;
    }



}
