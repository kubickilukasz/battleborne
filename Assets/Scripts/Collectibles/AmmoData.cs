using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AmmoData", menuName = "battleborne/AmmoData", order = 0)]
/**
Class used for storing data about amount of ammunition in an ammo collectible
*/
public class AmmoData : ScriptableObject 
{
    public float ammo = 20f; ///Amount of ammunition in the collectible
}

