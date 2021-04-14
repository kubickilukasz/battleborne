using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour{

    [SerializeField]
    private float foo;

    Rigidbody rigidbody;
    
    [SerializeField]
    TestSC mojTestSC;


    public class Klasa{
        public float foo;
    }

    Klasa lll;

    void Awake(){
        rigidbody = GetComponent<Rigidbody>();
        if(rigidbody == null){
            Destroy(gameObject);
        }
    }

    public void Testtets(){

    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ja pierwszy");

       // GetComponent<Test>().Testtets();
        
    }

    // Update is called once per frame
    void FixedUpdate(){
        Debug.Log(transform.position.x);

        rigidbody.AddForce(new Vector3(Input.GetAxis("Horizontal") * foo * Time.fixedDeltaTime,  Input.GetAxis("Vertical") * foo  * Time.fixedDeltaTime, 0));

    }

    void LateUpdate(){
        
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "city"){

        }
    }


     void OnCollisionEnter(Collision other)
    {
      
    }

    

}
