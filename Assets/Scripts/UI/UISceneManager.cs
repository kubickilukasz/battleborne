using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
Class responsible for control Scenes
*/
public class UISceneManager : MonoBehaviour
{
    /**
    Change to current scene
    @param id - id scene
    */
    public void ChangeScene(int id)
    {
        SceneManager.LoadScene(id, LoadSceneMode.Single);
    } 

    /**
    Exit Game
    */
    public void ExitApp()
    {
        Application.Quit();
    }
}
