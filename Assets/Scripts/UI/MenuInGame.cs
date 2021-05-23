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
    StateGame stateGame; /// Reference to StateGame

    [SerializeField]
    RectTransform aim; /// Reference to viewfinder UI object

    [SerializeField]
    RectTransform menuInGame; /// Reference to paused menu UI object

    [SerializeField]
    RectTransform cityHealthBar; /// Reference to UI object of health bar of city

    [SerializeField]
    RectTransform points; /// Reference to UI object of points

    [SerializeField]
    RectTransform combo; /// Reference to UI object of showing combo

    [SerializeField]
    RectTransform respawn; /// Reference to UI object of respawn

    [SerializeField]
    RectTransform end; /// Reference to UI object of GameOver

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
