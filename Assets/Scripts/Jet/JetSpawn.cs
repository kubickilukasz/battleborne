using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Class used for spawn point of the jet. Stores crucial data needed for other objects at the scene and spawns the jet when it's not existing anymore
*/
public class JetSpawn : MonoBehaviour
{
    [SerializeField]
    /// Place for jet's prefab
    private GameObject jetPrefab;

    /// Reference to the current jet's object on the scene
    public GameObject jetReference;

    private int jetPoints;

    [SerializeField]
    /// Point penalty given on each respawn
    private int respawnPenalty;

    [SerializeField]
    /// Reference to the list with bonuses and penalties received after dying
    private BonusPenaltyList bonusPenaltyList;

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
