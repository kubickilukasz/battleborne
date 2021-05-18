using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusPenaltyList", menuName = "battleborne/BonusPenaltyList", order = 0)]
public class BonusPenaltyList : ScriptableObject
{
    private List<int> bonusesAndPenalties;

    public void AddPenalty(int penalty)
    {
        bonusesAndPenalties.Add(-penalty);
    }
    public void AddBonus(int bonus)
    {
        bonusesAndPenalties.Add(bonus);
    }

    public List<int> GetList()
    {
        return bonusesAndPenalties;
    }

    public void ClearList()
    {
        bonusesAndPenalties.Clear();
    }
}
