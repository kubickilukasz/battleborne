using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

/**
Show information about combo 
*/
public class ComboUI : MonoBehaviour
{
    [SerializeField]
    /// Reference to text information 
    TextMeshProUGUI text; 

    [SerializeField]
    /// Reference to JetSpawn
    JetSpawn jetSpawn; 

    [SerializeField]
    /// Speed of shock of text 
    float speed = 2f; 

    [SerializeField]
    /// Speed of shock of text 
    float scale = 1.2f; 

    JetPoints jetPoints;

    float earlierCombo = 0;

    float state = 0;

    void Start()
    {
        
    }

    void Update()
    {
        state -= Time.deltaTime * speed;
        if(jetPoints == null && jetSpawn != null && jetSpawn.jetReference != null)
        {
            jetPoints = jetSpawn?.jetReference?.GetComponent<JetPoints>();
        }
        else
        {
            StringBuilder textBuilder = new StringBuilder("x");
            textBuilder.Append(jetPoints.GetCombo());
            //textBuilder.Append(" points");
            text.SetText(textBuilder.ToString());
            if(earlierCombo != jetPoints.GetCombo()){
                earlierCombo = jetPoints.GetCombo();
                state = 1;
            }

            if(earlierCombo == 0){
                text.SetText("");
            }
        }
    }
}
