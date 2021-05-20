using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateGame : MonoBehaviour
{
    public enum StateMenu{
        Play,
        Paused,
        Respawn
    }

    [SerializeField]
    StateMenu stateMenu = StateMenu.Play;

    [SerializeField]
    float timeTransition = 1f;

    [SerializeField]
    public UnityEvent onChangeState;

    void Start()
    {
        UpdateState();
    }

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
        }

    }

    public void PlayState(){
        if(stateMenu != StateMenu.Play){
            StartCoroutine(ChangeState(StateMenu.Play));
        }
    }

    public void PausedState(){
        if(stateMenu != StateMenu.Paused && stateMenu != StateMenu.Respawn){
            StartCoroutine(ChangeState(StateMenu.Paused));
        }
    }

    public void RespawnState(){
        if(stateMenu != StateMenu.Respawn && stateMenu != StateMenu.Paused){
            StartCoroutine(ChangeState(StateMenu.Respawn));
        }
    }

    public StateMenu GetStateMenu(){
        return stateMenu;
    }

    IEnumerator ChangeState(StateMenu newState){
        yield return new WaitForSecondsRealtime(timeTransition);
        stateMenu = newState;
        UpdateState();
        onChangeState.Invoke();
    }
}
