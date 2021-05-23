using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

/**
Class responsible for showing points in game
*/
public class ShowPoints : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI text; /// Reference to text information

    [SerializeField]
    JetSpawn jetSpawn; /// Reference to JetSpawn

    [SerializeField]
    float speed = 2f; /// Speed of the shock points

    [SerializeField]
    float scale = 1.2f; /// Max shock size of text

    JetPoints jetPoints;
    float earlierPoints = 0;
    float state = 0;

    void Update()
    {
        state -= Time.deltaTime * speed;
        
        if(jetPoints == null)
        {
            if(jetSpawn != null && jetSpawn.jetReference != null){
                jetPoints = jetSpawn?.jetReference?.GetComponent<JetPoints>();
            }
        }
        else
        {
            text.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, state);
            StringBuilder textBuilder = new StringBuilder();
            textBuilder.Append(jetPoints.GetPoints());
            textBuilder.Append(" points");
            text.SetText(textBuilder.ToString());
            if(earlierPoints != jetPoints.GetPoints()){
                earlierPoints = jetPoints.GetPoints();
                state = 1;
            }
        }
    }
}
