using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject jetPrefab; /// Place for jet's prefab

    public GameObject jetReference; /// Reference to the current jet's object on the scene

    private int jetPoints;

    [SerializeField]
    private int respawnPenalty; /// Point penalty given on each respawn

    [SerializeField]
    private BonusPenaltyList bonusPenaltyList; /// Reference to the list with bonuses and penalties received after dying

    void LateUpdate()
    {
        StorePoints(); 
    }


    /**
    Method that respawns the jet on the map, restores points it had before dying and applies penalties and bonuses
    */
    public void Respawn()
    {
        if(jetReference == null)
        {
            jetReference = Instantiate(jetPrefab,transform.position,Quaternion.identity) as GameObject;
            JetPoints points = jetReference.GetComponent<JetPoints>();
            points.RestorePoints(jetPoints);
            points.DecreasePoints(respawnPenalty);
            List<int> bpList = bonusPenaltyList.GetList();
            foreach (var bp in bpList)
            {
                if(bp > 0) points.AddPoints(bp); 
                else points.DecreasePoints(bp);
            }
            bonusPenaltyList.ClearList();
        }  
    }

    public int GetPoints(){
        return jetPoints;
    }

    /**
    Method that is used for storing the most recent amount of points
    */
    private void StorePoints()
    {
        if(jetReference)
        {
            jetPoints = jetReference.GetComponent<JetPoints>().GetPoints(); 
        }
    }
}
