using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class ComboUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    JetSpawn jetSpawn;

    [SerializeField]
    float speed = 2f;

    [SerializeField]
    float scale = 1.2f;

    JetPoints jetPoints;

    float earlierCombo = 0;

    float state = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
