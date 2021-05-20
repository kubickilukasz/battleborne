using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class EndUI : MonoBehaviour
{

    [SerializeField]
    JetSpawn jetSpawn;

    [SerializeField]
    TextMeshProUGUI textM;

    [SerializeField]
    StateGame stateGame;

    [SerializeField]
    City city;

    void Start(){
        stateGame.onChangeState.AddListener(View);
        city.cityDestroyedEvent.AddListener(GameOver);
    }

    public void GameOver(){
        Debug.Log("fsadf");
        stateGame.GameOverState();
    }

    public void View(){
        if(stateGame.GetStateMenu() == StateGame.StateMenu.GameOver){
            int currentScore = jetSpawn.GetPoints();
            int bestScore = 0;

            if(PlayerPrefs.HasKey("score")){
                bestScore = PlayerPrefs.GetInt("score");
            }

            bestScore = currentScore > bestScore ? currentScore : bestScore;

            StringBuilder textBuilder = new StringBuilder("your best score: ");
            textBuilder.Append(bestScore);
            textBuilder.Append("\nyour current score: ");
            textBuilder.Append(currentScore);

            textM.SetText(textBuilder.ToString());

            PlayerPrefs.SetInt("score", bestScore);
            PlayerPrefs.Save();
        }

    }

}
