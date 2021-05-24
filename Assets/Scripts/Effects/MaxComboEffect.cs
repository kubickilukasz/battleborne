using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Class creates combo and shield effect
*/
public class MaxComboEffect : MonoBehaviour
{

    /**
    * Reference to object, which represents combo effect
    */
    [SerializeField]
    GameObject effect;

    JetPoints jetPoints;

    void Start()
    {
        jetPoints = GetComponent<JetPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        effect.SetActive(jetPoints.isMaxCombo());
    }
}
