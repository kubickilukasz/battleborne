using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Data, which has several textures to choose from in random order
*/
[CreateAssetMenu(fileName = "BuildingsTexture", menuName = "Buildings/Textures", order = 1)]
public class BuildingsTextureData : ScriptableObject
{
    [SerializeField]
    /// List of available textures
    List<Texture> listOfTexture = new List<Texture>();

    public Texture GetRandomTexture()
    {
        return listOfTexture[Random.Range(0,listOfTexture.Count)];
    }

}
