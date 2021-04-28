using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityHealthBar : MonoBehaviour
{

    [SerializeField]
    City city;

    [SerializeField]
    Image cityHealthBar;

    void Start()
    {

    }

    void Update()
    {
        cityHealthBar.fillAmount = city.Health / city.MaxHealth;
    }



}
