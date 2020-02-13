using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIHandler : MonoBehaviour
{
    private TextMeshProUGUI _txtPoints;
    
    // Start is called before the first frame update
    private void Start()
    {
        _txtPoints = transform.Find("TopPanel/txtPoints").GetComponent<TextMeshProUGUI>();

        //initially update the points text field on launch
        //UpdateTxtPoints();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    //public void UpdateTxtPoints()
    //{
    //    _txtPoints.text = GameData.Points.Genetic.Get() + " Points";
    //}
}
