using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject jetPrefab;

    public GameObject jetReference;

    private int jetPoints;

    [SerializeField]
    private int respawnPenalty;

    [SerializeField]
    private BonusPenaltyList bonusPenaltyList;

    void LateUpdate()
    {
        StorePoints();
        //Respawn();   
    }

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

    private void StorePoints()
    {
        if(jetReference)
        {
            jetPoints = jetReference.GetComponent<JetPoints>().GetPoints(); 
        }
    }
}
