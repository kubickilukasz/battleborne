using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Every building get random texture on start scene
*/
public class BuildingTexture : MonoBehaviour
{
    [SerializeField]
    /// Data to textures
    BuildingsTextureData data;

    void Start()
    {
        GetComponent<Renderer>().material.SetTexture("_BaseMap", data.GetRandomTexture());
    }

}
