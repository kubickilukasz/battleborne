using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [SerializeField]
    StateGame stateGame;

    [SerializeField]
    RectTransform aim;

    [SerializeField]
    RectTransform menuInGame;

    [SerializeField]
    RectTransform cityHealthBar;

    void Start()
    {
        stateGame.onChangeState.AddListener(UpdateState);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(stateGame.GetStateMenu() == StateGame.StateMenu.Play){
                stateGame.PausedState();
            }else{
                stateGame.PlayState();
            }
        }
    }

    public void UpdateState(){

        switch(stateGame.GetStateMenu()){
            case StateGame.StateMenu.Play:
            aim?.gameObject.SetActive(true);
            menuInGame?.gameObject.SetActive(false);
            cityHealthBar?.gameObject.SetActive(true);
            break;
            case StateGame.StateMenu.Paused:
            aim?.gameObject.SetActive(false);
            menuInGame?.gameObject.SetActive(true);
            cityHealthBar?.gameObject.SetActive(false);
            break;
        }

    }


}
