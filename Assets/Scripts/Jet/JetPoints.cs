using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/**
Class that stores the data and performs actions reffering to jet's points
*/
public class JetPoints : MonoBehaviour
{

    /// Event invoked when max combo amount has been reached
    public UnityEvent maxComboEvent;

    private bool maxComboEventInvoked = false;
    /// Event invoked when combo has been reset
    public UnityEvent comboFinishEvent;

    [Header("Points")]
    [SerializeField]
    /// Points collected during the game
    private int points;

    [Header("Combo")]
    [SerializeField]
    /// Maximum combo amount
    private float maxCombo; 

    [SerializeField]
    [Range(1f,1.5f)]
    /// Combo multiplier
    private float defaultMultiplier;

    [SerializeField]
    /// Point bonus given while being in max combo state
    private int comboBonus;

    private float combo = 0.0f;

    private float mostRecentCombo;

#region Timer
    [SerializeField]
    /// Indicator for how long the max combo state should stay on
    private float timerSeconds;

    private float timer = 0.0f;

    [SerializeField]
    /// Indicator for how long combo can remain unchanged before resetting (e.g user stops hitting enemies)
    private float comboResetTimerSeconds;

    private float comboTimer = 0.0f;
#endregion


    void Update()
    {
        if(isMaxCombo())
        {
            if(!maxComboEventInvoked)
            {
                maxComboEvent.Invoke();
                maxComboEventInvoked = true;
            }
            Timer();
        }
        else
        {
            ComboResetTimer();
            timer = 0.0f;
        }
    }

#region PublicMethods

    /**
    Method used for adding points to the current score
    @param value Amount of received points
    */
    public void AddPoints(int value)
    {
        if(combo >= maxCombo)
            points += comboBonus*value;
        else
            points += value;
    }

    /**
    Method used for decreasing points from the current score
    @param value Amount of points to decrease
    */
    public void DecreasePoints(int value)
    {
        points -= value;
        if(points < 0)
            points = 0;
    }

    /**
    Method used for combo stacking 
    @param multiplier Combo multiplier to stack up
    */
    public void StackCombo(float multiplier)
    {
        comboTimer = 0;
        combo += defaultMultiplier * multiplier;
        if(combo > maxCombo)
            combo = maxCombo;
        mostRecentCombo = combo;
    }

    /**
    Method used to reset combo stack
    */
    public void ResetCombo()
    {
        combo = 0.0f;
    }

    /**
    Method used for indicating whether max combo state is on or not
    @return Returns true if max combo state is on, false otherwise
    */
    public bool isMaxCombo()
    {
        return combo == maxCombo ? true : false;
    }

    public float GetCombo()
    {
        return combo;
    }
    public int GetPoints()
    {
        return points;
    }

    public int GetBonus()
    {
        return comboBonus;
    }

    /**
    Method used to restore points after respawning
    */
    public void RestorePoints(int restored)
    {
        points = restored;
    }

#endregion

#region PrivateMethods

    /**
    Method used to count down time of max combo state
    */
    private void Timer()
    {
        timer += Time.deltaTime;
        if(timer >= timerSeconds)
        {
            ResetCombo();
            maxComboEventInvoked = false;
            comboFinishEvent.Invoke();
            timer = 0.0f;
        }
    }

    /**
    Method used to count down inactivity of the current combo. Resets it when the given time has passed.
    */
    private void ComboResetTimer()
    {
        if(combo == mostRecentCombo)
        {
            comboTimer += Time.deltaTime;
            if(comboTimer >= comboResetTimerSeconds)
            {
                ResetCombo();
                comboTimer = 0.0f;
            }
        }
        else comboTimer = 0.0f;
    }

#endregion
}
