using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
Class responsible for states of games in UI system.
*/
public class StateGame : MonoBehaviour
{
    /**
    Enum represents state of menu
    */
    public enum StateMenu{
        Play,
        Paused,
        Respawn,
        GameOver
    }

    [SerializeField]
    StateMenu stateMenu = StateMenu.Play; /// Current state of game

    [SerializeField]
    float timeTransition = 1f; /// Time to change state

    [SerializeField]
    public UnityEvent onChangeState; /// On change state event

    void Start()
    {
        UpdateState();
    }

    /**
    Update time scale, cursor visible, cursor lockstate depends of state of game 
    */
    public void UpdateState(){

        switch(stateMenu){
            case StateMenu.Play:
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            break;
            case StateMenu.Paused:
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            break;
            case StateMenu.Respawn:
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            break;
            case StateMenu.GameOver:
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            break;
        }

    }

    /**
    Change to PlayState
    */
    public void PlayState(){
        if(stateMenu != StateMenu.Play && stateMenu != StateMenu.GameOver){
            StartCoroutine(ChangeState(StateMenu.Play));
        }
    }

    /**
    Change to PausedState
    */
    public void PausedState(){
        if(stateMenu != StateMenu.Paused && stateMenu != StateMenu.Respawn && stateMenu != StateMenu.GameOver){
            StartCoroutine(ChangeState(StateMenu.Paused));
        }
    }

    /**
    Change to RespawnState
    */
    public void RespawnState(){
        if(stateMenu != StateMenu.Respawn && stateMenu != StateMenu.Paused && stateMenu != StateMenu.GameOver){
            StartCoroutine(ChangeState(StateMenu.Respawn));
        }
    }

    /**
    Change to GameOverState
    */
    public void GameOverState(){
        if(stateMenu != StateMenu.GameOver){
            StartCoroutine(ChangeState(StateMenu.GameOver));
        }
    }

    public StateMenu GetStateMenu(){
        return stateMenu;
    }

    /**
    Wait some time and change state
    */
    IEnumerator ChangeState(StateMenu newState){
        yield return new WaitForSecondsRealtime(timeTransition);
        stateMenu = newState;
        UpdateState();
        onChangeState.Invoke();
    }
}
