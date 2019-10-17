using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //store a reference to the clickable continue button
    private Button btnContinue;

    private int outcomeType = -1; //-1 = undefined, 0 = negetive, 1 = positive

    // Start is called before the first frame update
    private void Start()
    {
        //begin by setting the graphics colours depending on whether the outcome was positive or negetive
        Image bgMain = transform.Find("bgMain").GetComponent<Image>();
        Image bottomPanel = transform.Find("bgMain/bottomPanel").GetComponent<Image>();
        Image btnContinueBackground = transform.Find("bgMain/btnContinue").GetComponent<Image>();
        
        if (outcomeType == 0) //if negetive outcome
        {
            bgMain.color = negativePrimaryColour;
            bottomPanel.color = negativeSecondaryColour;
            btnContinueBackground.color = negativeButtonColour;
        }
        else
        {
            bgMain.color = positivePrimaryColour;
            bottomPanel.color = positiveSecondaryColour;
            btnContinueBackground.color = positiveButtonColour;
        }

        btnContinue = transform.Find("bgMain/btnContinue").GetComponent<Button>();
        btnContinue.onClick.AddListener(() => btnContinuePressed());
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void btnContinuePressed()
    {
        //increment the game state and destory the outcome popup
        GameData.incrementState();

        Destroy(gameObject);
    }
}
