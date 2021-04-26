using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletRaycast
{   
    public static void Shoot(Vector3 shootPos, Vector3 shootDir){
        RaycastHit raycastHit = Physics.Raycast(shootPos,shootDir);
        if(raycastHit.collider != null){
            GameObject obj = raycastHit.collider.GetComponent<GameObject>();
            if(obj != null){
                if(obj.tag == "city"){
                    //damage dla miasta jak już będzie zaimplementowany
                }
                else if(obj.tag == "alien"){
                    //TODO:
                    //damage dla aliena
                    obj.OnHit(10);
                }
            }
        }
    }
}
