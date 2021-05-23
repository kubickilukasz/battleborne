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
    /// Reference to City
    City city; 

    [SerializeField]
    /// Reference to image, which represents health bar
    Image cityHealthBar; 

    void Update()
    {
        cityHealthBar.fillAmount = city.Health / (float)city.MaxHealth;
    }



}
