using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxComboEffect : MonoBehaviour
{

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
