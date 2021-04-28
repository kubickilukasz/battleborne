using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerPlaceholder : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private bool debug = false;

    void Start()
    {
        speed = 5f;
    }

    void FixedUpdate()
    {
        //Sterowanie: WSADQE
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime, Input.GetAxis("UpDown") * speed * Time.fixedDeltaTime, Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime));
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<CityBuilding>())
        {
            coll.gameObject.GetComponent<CityBuilding>().OnHit(50);
        }
        else if (debug && coll.gameObject.GetComponent<City>())
        {
            Debug.Log("========== ENTERING CITY ==========");
        }
    }
}
