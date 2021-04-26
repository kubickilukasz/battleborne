using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetShooting : MonoBehaviour
{
    [SerializeField]
    private float ammunition = 150f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddAmmo(float ammo){
        ammunition+=ammo;
    }
}
