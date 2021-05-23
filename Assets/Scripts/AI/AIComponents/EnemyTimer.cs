using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Auxiliary component class that represents timer that decreases from selected value to 0
*/
public class EnemyTimer
{
    public EnemyTimer(float _value, float _maxValue) {
        maxValue = _maxValue;
        value = _value;
    }

    private float maxValue;
    private float value;

    /**
    * Updates the timer - decreases timer values gradually to 0
    */
    public void UpdateTimer() {
        if (value > 0)
            value = value - Time.fixedDeltaTime * 100f;
    }
    
    /**
    * Checks it timer has reached 0
    * @return True if timer has reached 0
    */
    public bool IsTimerZero()
    {
        if (value > 0) return false;
        else return true;
    }

    /**
    * Resets the timer
    */
    public void ResetTimer()
    {
        value = maxValue;
    }

    /**
    * Debugs the timer to debug console
    */
    public void DebugTimer()
    {
        Debug.Log(value + "/" + maxValue);
    }
}
