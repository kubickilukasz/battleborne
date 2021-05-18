using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BorderInfo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    InvisibleWall invisibleWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText("Back to FIGHT!\n"+(int)(invisibleWall.GetTimerSeconds() - invisibleWall.GetTimer()));
    }

    public void ShowInfo(){
        text.gameObject.SetActive(true);
    }

    public void HideInfo(){
        text.gameObject.SetActive(false);
    }
}
