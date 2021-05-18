using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class JetPoints : MonoBehaviour
{

    public UnityEvent maxComboEvent;
    private bool maxComboEventInvoked = false;

    public UnityEvent comboFinishEvent;

    [Header("Points")]
    [SerializeField]
    private int points;

    [Header("Combo")]
    [SerializeField]
    private float maxCombo;

    [SerializeField]
    [Range(1f,1.5f)]
    private float defaultMultiplier;

    [SerializeField]
    private int comboBonus;

    private float combo = 0.0f;

#region Timer
    [SerializeField]
    private float timerSeconds;

    private float timer = 0.0f;
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
            timer = 0.0f;
        }
    }

#region PublicMethods
    public void AddPoints(int value)
    {
        if(combo >= maxCombo)
            points += comboBonus*value;
        else
            points += value;
    }

    public void DecreasePoints(int value)
    {
        points -= value;
        if(points < 0)
            points = 0;
    }

    public void StackCombo(float multiplier)
    {
        combo += defaultMultiplier * multiplier;
        if(combo > maxCombo)
            combo = maxCombo;
        
    }

    public void ResetCombo()
    {
        combo = 0.0f;
    }

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

    public void RestorePoints(int restored)
    {
        points = restored;
    }

#endregion

#region PrivateMethods

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

#endregion
}
