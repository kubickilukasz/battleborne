using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTexture : MonoBehaviour
{
    [SerializeField]
    BuildingsTextureData data;

    void Start()
    {
        GetComponent<Renderer>().material.SetTexture("_BaseMap", data.GetRandomTexture());
    }

}
