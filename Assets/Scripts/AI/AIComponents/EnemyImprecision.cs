using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImprecision
{
    public EnemyImprecision(GameObject go, Vector3 _cycleImprecisonBox) {
        randomImprecision = Vector3.zero;
        cycleImprecisonBox = _cycleImprecisonBox;
    }
    private Vector3 cycleImprecisonBox;
    public Vector3 randomImprecision;

    public void GenerateRandomImprecision()
	{
        randomImprecision.x = Random.Range(-cycleImprecisonBox.x, cycleImprecisonBox.x);
        randomImprecision.y = Random.Range(-cycleImprecisonBox.y, cycleImprecisonBox.y);
        randomImprecision.z = Random.Range(-cycleImprecisonBox.z, cycleImprecisonBox.z);
    }


}
