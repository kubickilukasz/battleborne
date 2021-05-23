using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
Class responsible for showing information about health of city
*/
public class CityHealthBar : MonoBehaviour
{

    [SerializeField]
    City city; /// Reference to City

    [SerializeField]
    Image cityHealthBar; /// Reference to image, which represents health bar

    void Update()
    {
        cityHealthBar.fillAmount = city.Health / (float)city.MaxHealth;
    }



}
