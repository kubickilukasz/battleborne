using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestEnemy : MonoBehaviour{
    
    [SerializeField]
    float speed;

    [SerializeField]
    Transform target;

    Rigidbody rigidbody;

    [SerializeField]
    GameObject gameObject;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    GameObject [] waypointy;

    [System.Serializable]
    public class TT{
        public float zmienna;
        public float inna;
    }


    [SerializeField]
    List<TT> lists = new List<TT>();

    void Start(){
        rigidbody = GetComponent<Rigidbody>();

        

    }

    // Update is called once per frame
    void FixedUpdate(){


        //if(rigidbody.velocity.sqrMagnitude < 200f ){
            rigidbody.AddRelativeForce(Vector3.forward * speed * Time.fixedDeltaTime);
      //  }

        rigidbody.AddTorque(new Vector3(0,Input.GetAxisRaw("Horizontal"),0) * speed * 0.01f * Time.fixedDeltaTime);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, 100, layerMask);

        foreach(RaycastHit ray in hits){
       //     ray.collider.transform.position
        }

    }

    void OnDrawGizmos(){
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position , (target.transform.position - transform.position).normalized);

    }

}
