using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        Image bottomPanel = bgMain.transform.Find("bottomPanel").GetComponent<Image>();
        Image btnContinueBackground = bgMain.transform.Find("btnContinue").GetComponent<Image>();

        //get references to all the text boxes
        TextMeshProUGUI txtHeading = bgMain.transform.Find("txtHeading").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtSubHeading = bgMain.transform.Find("txtSubHeading").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtBody = bgMain.transform.Find("txtBody").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtPoints = bottomPanel.transform.Find("txtPoints").GetComponent<TextMeshProUGUI>();

        //determine whether the player has recieved a positive or negetive outcome
        if ((GameData.returnGuessedHigher() == true && GameData.returnNewCardIsHigher() == true) || (GameData.returnGuessedHigher() == false && GameData.returnNewCardIsHigher() == false))
            outcomeType = 1;
        else
            outcomeType = 0;

        if (outcomeType == 1)
        {
            //set the popup panel colours
            bgMain.color = positivePrimaryColour;
            bottomPanel.color = positiveSecondaryColour;
            btnContinueBackground.color = positiveButtonColour;

            int outcomeNum = Random.Range(0, GameData.returnOutcome().positiveOutcomes.Count);

            //store a local reference of the positive outcome
            PositiveOutcome positiveOutcome = GameData.returnOutcome().positiveOutcomes[outcomeNum];

            //populate the outcome text with the data from the JSON files
            txtHeading.text = positiveOutcome.Heading;
            txtSubHeading.text = positiveOutcome.SubHeading;
            txtBody.text = positiveOutcome.Description;
            txtPoints.text = "+" + positiveOutcome.Points.ToString() + " Points";

            //add the points from the outcome to the new total
            GameData.addToPoints(positiveOutcome.Points);
        }
        else
        {
            //set the popup panel colours
            bgMain.color = negativePrimaryColour;
            bottomPanel.color = negativeSecondaryColour;
            btnContinueBackground.color = negativeButtonColour;

            //populate the cards text with the data from the JSON files
            int outcomeNum = Random.Range(0, GameData.returnOutcome().negetiveOutcomes.Count);

            //store a local reference of the positive outcome
            NegetiveOutcome negetiveOutcome = GameData.returnOutcome().negetiveOutcomes[outcomeNum];

            //populate the outcome text with the data from the JSON files
            txtHeading.text = negetiveOutcome.Heading;
            txtSubHeading.text = negetiveOutcome.SubHeading;
            txtBody.text = negetiveOutcome.Description;
            txtPoints.text = negetiveOutcome.Points.ToString() + " Points";

            //add the points from the outcome to the new total
            GameData.addToPoints(negetiveOutcome.Points);
        }

        //find the GUI Handler class and update the GUI
        GameObject.Find("MainGUI").GetComponent<GUIHandler>().updateTxtPoints();

        btnContinue = transform.Find("bgMain/btnContinue").GetComponent<Button>();
        btnContinue.onClick.AddListener(() => btnContinuePressed());
    }

    private void btnContinuePressed()
    {
        //increment the game state and destory the outcome popup
        GameData.incrementState();

        Destroy(gameObject);
    }
}
