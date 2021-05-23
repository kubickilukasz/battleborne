using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

/**
Show GameOver window
*/
public class EndUI : MonoBehaviour
{

    [SerializeField]
    JetSpawn jetSpawn; /// Reference to JetSpawn

    [SerializeField]
    TextMeshProUGUI textM; /// Reference to text information

    [SerializeField]
    StateGame stateGame; /// Reference to StateGame

    [SerializeField]
    City city; /// Reference to City

    void Start(){
        stateGame.onChangeState.AddListener(View);
        city.cityDestroyedEvent.AddListener(GameOver);
    }

    /**
    Turn state game to GameOver
    */
    public void GameOver(){
        stateGame.GameOverState();
    }

    /**
    Show result of game
    */
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
