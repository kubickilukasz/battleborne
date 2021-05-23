using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
Class responsible for showing information about cross the board of map. If player don't turn back, jet will be destroyed.
*/
public class BorderInfo : MonoBehaviour
{
    [SerializeField]
    /// Reference to text information
    TextMeshProUGUI text; 

    [SerializeField]
    /// Refernece to InvisibleWall
    InvisibleWall invisibleWall; 

    [SerializeField]
    /// Reference to StateGame to pause the game
    StateGame stateGame; 

    void Update()
    {
        if(stateGame.GetStateMenu() == StateGame.StateMenu.Play){
            text.SetText("Back to FIGHT!\n"+(int)(invisibleWall.GetTimerSeconds() - invisibleWall.GetTimer()));
        }else{
            text.SetText("");
        }
    }

    /**
    Show information about crossing the board
    */
    public void ShowInfo(){
        text.gameObject.SetActive(true);
    }
    
    /**
    Hide information about crossing the board
    */
    public void HideInfo(){
        text.gameObject.SetActive(false);
    }
}
