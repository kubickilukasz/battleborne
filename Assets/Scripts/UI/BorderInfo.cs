using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BorderInfo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    InvisibleWall invisibleWall;

    [SerializeField]
    StateGame stateGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stateGame.GetStateMenu() == StateGame.StateMenu.Play){
            text.SetText("Back to FIGHT!\n"+(int)(invisibleWall.GetTimerSeconds() - invisibleWall.GetTimer()));
        }else{
            text.SetText("");
        }
    }

    public void ShowInfo(){
        text.gameObject.SetActive(true);
    }

    public void HideInfo(){
        text.gameObject.SetActive(false);
    }
}
