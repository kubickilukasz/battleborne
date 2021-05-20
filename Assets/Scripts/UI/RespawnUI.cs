using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class RespawnUI : MonoBehaviour
{   
    [SerializeField]
    StateGame stateGame;

    [SerializeField]
    JetSpawn jetSpawn;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    float timeToRespawn = 5f;  

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

    public void Respawn(){
        if(!lockButton){
            counting = false;
            jetSpawn.Respawn();
            stateGame.PlayState();
        }
    }

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
