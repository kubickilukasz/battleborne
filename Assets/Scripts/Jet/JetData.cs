using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "JetData", menuName = "battleborne/JetData", order = 0)]
public class JetData : ScriptableObject 
{
    public float ammo;
    public float health;  

    
    void OnValidate()
    {

        ammo = 150f;
        health = 100f;

    }
}

