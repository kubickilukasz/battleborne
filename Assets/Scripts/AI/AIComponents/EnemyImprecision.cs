using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Auxiliary component class responsible for generating imprecise position, 
* that is used to achive hovering effect and set semi-random dodging position
*/
public class EnemyImprecision
{
    public EnemyImprecision(GameObject go, Vector3 _cycleImprecisonBox) {
        randomImprecision = Vector3.zero;
        cycleImprecisonBox = _cycleImprecisonBox;
    }

    /**
    * Maximum size of imprecision box
    */
    private Vector3 cycleImprecisonBox;

    /**
    * Generated imprecise position
    */
    public Vector3 randomImprecision;

    /**
    * Generates new imprecise position
    */
    public void GenerateRandomImprecision()
	{
        randomImprecision.x = Random.Range(-cycleImprecisonBox.x, cycleImprecisonBox.x);
        randomImprecision.y = Random.Range(-cycleImprecisonBox.y, cycleImprecisonBox.y);
        randomImprecision.z = Random.Range(-cycleImprecisonBox.z, cycleImprecisonBox.z);
    }


}
