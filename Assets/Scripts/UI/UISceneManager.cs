using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneManager : MonoBehaviour
{
    public void ChangeScene(int id)
    {
        SceneManager.LoadScene(id, LoadSceneMode.Single);
    } 

    public void ExitApp()
    {
        Application.Quit();
    }
}
