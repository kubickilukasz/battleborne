using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject jetPrefab;

    public GameObject jetReference;


    void LateUpdate()
    {
        Respawn();   
    }

    public void Respawn()
    {
        if(jetReference == null)
        {
            jetReference = Instantiate(jetPrefab,transform.position,Quaternion.identity) as GameObject;
        }   
    }
}
