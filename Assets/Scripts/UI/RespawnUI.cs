using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

/**
Class responsible for respawn menu
*/
public class RespawnUI : MonoBehaviour
{   
    [SerializeField]
    StateGame stateGame; /// Reference to StateGame

    [SerializeField]
    JetSpawn jetSpawn; /// Reference to JetSPawn

    [SerializeField]
    TextMeshProUGUI text; /// Reference to text info

    [SerializeField]
    float timeToRespawn = 5f;  /// Time to respawn jet

    float timer = 0;
    bool counting = false;
    bool lockButton = true;

    void Start()
    {   
        timer = timeToRespawn;
        stateGame.onChangeState.AddListener(OnChange);
    }

    void Update()
    {

        if(jetSpawn.jetReference == null){
            stateGame.RespawnState();
        }

        if(counting){
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StringBuilder textBuilder = new StringBuilder("Back to fight!");
                text.SetText(textBuilder.ToString());
                lockButton = false;
            }
            else
            {
                StringBuilder textBuilder = new StringBuilder("Wait ");
                textBuilder.Append((int)(timer+1));
                text.SetText(textBuilder.ToString());
                lockButton = true; 
            }
        }
    }

    /**
    Try respawn jet 
    */
    public void Respawn(){
        if(!lockButton){
            counting = false;
            jetSpawn.Respawn();
            stateGame.PlayState();
        }
    }

    /**
    If state game was changed, check if it is Respawn state and try to countdown respawn 
    */
    void OnChange()
    {
        if(stateGame.GetStateMenu() == StateGame.StateMenu.Respawn && counting == false)
        {
            timer = timeToRespawn;
            counting = true;
            lockButton = true;
        }
    }


}
