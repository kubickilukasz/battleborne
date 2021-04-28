using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerPlaceholder : MonoBehaviour
{
    private static bool _debug = false;
    [SerializeField]
    private bool debug = false;

    void Start()
    {
        _debug = debug;
    }

    void Update()
    {

    }

    public static void ListenDestroyBuilding()
    {
        if (_debug)
        {
            Debug.Log("========== BUILDING DESTROYED ==========");
        }
    }

    public static void ListenDestroyCity()
    {
        if (_debug)
        {
            Debug.Log("========== CITY DESTROYED ==========");
        }
    }
}
