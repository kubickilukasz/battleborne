using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimer
{
    public EnemyTimer(float _value, float _maxValue) {
        maxValue = _maxValue;
        value = _value;
    }

    private float maxValue;
    private float value;

    public void UpdateTimer() {
        if (value > 0)
            value = value - Time.fixedDeltaTime * 100f;
    }
    
    public bool IsTimerZero()
    {
        if (value > 0) return false;
        else return true;
    }

    public void ResetTimer()
    {
        value = maxValue;
    }

    public void DebugTimer()
    {
        Debug.Log(value + "/" + maxValue);
    }
}
