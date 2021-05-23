using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusPenaltyList", menuName = "battleborne/BonusPenaltyList", order = 0)]
public class BonusPenaltyList : ScriptableObject
{
    private List<int> bonusesAndPenalties;

    /**
    Method used for stacking penalties earned right before dying
    @param penalty - amount of points to decrease for the penalty
    */
    public void AddPenalty(int penalty)
    {
        bonusesAndPenalties.Add(-penalty);
    }

    /**
    Method used for adding bonuses earned right before dying
    @param bonus - amount of points to add for the bonus
    */
    public void AddBonus(int bonus)
    {
        bonusesAndPenalties.Add(bonus);
    }

    public List<int> GetList()
    {
        return bonusesAndPenalties;
    }

    /**
    Method used to clear the list after all bonuses and penalties have been received
    */
    public void ClearList()
    {
        bonusesAndPenalties.Clear();
    }
}
