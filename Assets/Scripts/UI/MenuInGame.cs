using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{

    public enum StateMenu{
        Play,
        Paused
    }

    [SerializeField]
    StateMenu stateMenu = StateMenu.Play;

    [SerializeField]
    RectTransform aim;

    [SerializeField]
    RectTransform menuInGame;

    [SerializeField]
    RectTransform cityHealthBar;

    [SerializeField]
    float timeTransition = 1f;

    void Start()
    {
        UpdateState();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(stateMenu == StateMenu.Play){
                PausedState();
            }else{
                PlayState();
            }
        }
    }

    public void UpdateState(){

        switch(stateMenu){
            case StateMenu.Play:
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            aim?.gameObject.SetActive(true);
            menuInGame?.gameObject.SetActive(false);
            cityHealthBar?.gameObject.SetActive(true);
            break;
            case StateMenu.Paused:
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            aim?.gameObject.SetActive(false);
            menuInGame?.gameObject.SetActive(true);
            cityHealthBar?.gameObject.SetActive(false);
            break;
        }

    }

    public void PlayState(){
        if(stateMenu != StateMenu.Play){
            StartCoroutine(ChangeState(StateMenu.Play));
        }
    }

    public void PausedState(){
        if(stateMenu != StateMenu.Paused){
            StartCoroutine(ChangeState(StateMenu.Paused));
        }
    }

    IEnumerator ChangeState(StateMenu newState){
        yield return new WaitForSecondsRealtime(timeTransition);
        stateMenu = newState;
        UpdateState();
    }

}
