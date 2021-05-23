using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
Class responsible for control menus and UIs in game
*/
public class MenuInGame : MonoBehaviour
{
    [SerializeField]
    /// Reference to StateGame
    StateGame stateGame;

    [SerializeField]
    /// Reference to viewfinder UI object
    RectTransform aim; 

    [SerializeField]
    /// Reference to paused menu UI object
    RectTransform menuInGame; 

    [SerializeField]
    /// Reference to UI object of health bar of city
    RectTransform cityHealthBar; 

    [SerializeField]
    /// Reference to UI object of points
    RectTransform points; 

    [SerializeField]
    /// Reference to UI object of showing combo
    RectTransform combo; 

    [SerializeField]
    /// Reference to UI object of respawn
    RectTransform respawn; 

    [SerializeField]
    /// Reference to UI object of GameOver
    RectTransform end; 

    void Start()
    {
        stateGame.onChangeState.AddListener(UpdateState);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && stateGame.GetStateMenu() != StateGame.StateMenu.GameOver && stateGame.GetStateMenu() != StateGame.StateMenu.Respawn){
            if(stateGame.GetStateMenu() == StateGame.StateMenu.Play){
                stateGame.PausedState();
            }else{
                stateGame.PlayState();
            }
        }
    }

    /**
    Update active of UI objects depends of GameState 
    */
    public void UpdateState(){

        switch(stateGame.GetStateMenu()){
            case StateGame.StateMenu.Play:
            aim?.gameObject.SetActive(true);
            menuInGame?.gameObject.SetActive(false);
            cityHealthBar?.gameObject.SetActive(true);
            points?.gameObject.SetActive(true);
            combo?.gameObject.SetActive(true);
            respawn?.gameObject.SetActive(false);
            end?.gameObject.SetActive(false);
            break;
            case StateGame.StateMenu.Paused:
            aim?.gameObject.SetActive(false);
            menuInGame?.gameObject.SetActive(true);
            cityHealthBar?.gameObject.SetActive(false);
            points?.gameObject.SetActive(false);
            combo?.gameObject.SetActive(false);
            respawn?.gameObject.SetActive(false);
            end?.gameObject.SetActive(false);
            break;
            case StateGame.StateMenu.Respawn:
            aim?.gameObject.SetActive(false);
            menuInGame?.gameObject.SetActive(false);
            cityHealthBar?.gameObject.SetActive(false);
            points?.gameObject.SetActive(false);
            combo?.gameObject.SetActive(false);
            respawn?.gameObject.SetActive(true);
            end?.gameObject.SetActive(false);
            break;
            case StateGame.StateMenu.GameOver:
            aim?.gameObject.SetActive(false);
            menuInGame?.gameObject.SetActive(false);
            cityHealthBar?.gameObject.SetActive(false);
            points?.gameObject.SetActive(false);
            combo?.gameObject.SetActive(false);
            respawn?.gameObject.SetActive(false);
            end?.gameObject.SetActive(true);
            break;
        }

    }


}
