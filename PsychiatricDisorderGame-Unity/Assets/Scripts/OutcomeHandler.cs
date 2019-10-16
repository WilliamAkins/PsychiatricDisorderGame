using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcomeHandler : MonoBehaviour
{
    //the various colours that can be used depending on the outcome
    [SerializeField]
    private Color positivePrimaryColour;
    [SerializeField]
    private Color positiveSecondaryColour;
    [SerializeField]
    private Color positiveButtonColour;

    [SerializeField]
    private Color negativePrimaryColour;
    [SerializeField]
    private Color negativeSecondaryColour;
    [SerializeField]
    private Color negativeButtonColour;

    int outcomeType = -1; //-1 = undefined, 0 = negetive, 1 = positive

    // Start is called before the first frame update
    void Start()
    {
        //begin by setting the graphics colours depending on whether the outcome was positive or negetive
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
