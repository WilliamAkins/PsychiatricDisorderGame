using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIHandler : MonoBehaviour
{
    TextMeshProUGUI txtPoints;
    
    // Start is called before the first frame update
    private void Start()
    {
        txtPoints = transform.Find("TopPanel/txtPoints").GetComponent<TextMeshProUGUI>();

        //initially update the points text field on launch
        updateTxtPoints();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void updateTxtPoints()
    {
        txtPoints.text = GameData.returnPoints() + " Points";
    }
}
