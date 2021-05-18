using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ShowPoints : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    JetSpawn jetSpawn;

    JetPoints jetPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(jetPoints == null)
        {
            jetPoints = jetSpawn?.jetReference?.GetComponent<JetPoints>();
        }
        else
        {
            StringBuilder textBuilder = new StringBuilder();
            textBuilder.Append(jetPoints.GetPoints());
            textBuilder.Append(" points");
            text.SetText(textBuilder.ToString());
        }
    }
}
