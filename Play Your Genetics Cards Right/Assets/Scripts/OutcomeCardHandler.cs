using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameData;

public class OutcomeCardHandler : MonoBehaviour
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

    private int _cardId;

    private CardPileHandler _cardPileHandler;

    private IEnumerator _lerpCoroutine;

    //store a reference to the clickable continue button
    private Button _btnContinue;

    private GameObject _pileCard = null;

    //private float _lerpTimer;

    //private bool _btnPressed;

    //private Vector3 _cardRestPos = Vector3.zero;

    //private int outcomeType = -1; //-1 = undefined, 0 = negative, 1 = positive

    // Start is called before the first frame update
    private void Start()
    {
        Card card = null;
        if (CardType.Get() == CardType.Type.Genetics)
        {
            _cardId = CardOutcomes.Genetic.OutcomesReceivedCount - CardOutcomes.Genetic.Pile.OutcomesViewed;
            CardOutcomes.Genetic.Pile.OutcomesViewed++;
        
            _pileCard = CardOutcomes.Genetic.Pile.Get()[_cardId];

            card = CardOutcomes.Genetic.Received()[_cardId];

            //add the points from the outcome to the new genetic total
            Points.Genetic.Increment(card.Points);
        }
        else
        {
            _cardId = CardOutcomes.Environment.OutcomesReceivedCount - CardOutcomes.Environment.Pile.OutcomesViewed;
            CardOutcomes.Environment.Pile.OutcomesViewed++;
        
            _pileCard = CardOutcomes.Environment.Pile.Get()[_cardId];

            card = CardOutcomes.Environment.Received()[_cardId];

            //add the points from the outcome to the new environment total
            Points.Environment.Increment(card.Points);
        }

        _cardPileHandler = _pileCard.transform.parent.parent.GetComponent<CardPileHandler>();

        //set the initial position to that of the card on the pile
        transform.position = _pileCard.transform.position;
        transform.localScale = _pileCard.transform.localScale;
        transform.rotation = _pileCard.transform.rotation;

        _pileCard.SetActive(false);

        Image bgMain = GetComponent<Image>();
        //begin by setting the graphics colours depending on whether the outcome was positive or negative
        //Image bgMain = transform.Find("bgMain").GetComponent<Image>();
        //Image bottomPanel = bgMain.transform.Find("bottomPanel").GetComponent<Image>();
        Image btnContinueBackground = transform.Find("btnContinue").GetComponent<Image>();

        //get references to all the text boxes
        TextMeshProUGUI txtHeading = transform.Find("txtHeading").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtSubHeading = transform.Find("txtSubHeading").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtBody = transform.Find("txtBody").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtPoints = transform.Find("txtPoints").GetComponent<TextMeshProUGUI>();

        string pointsPrefix = "";
        //determine whether the player has received a positive or negative outcome
        if (card.IsPositive)
        {
            //set the popup panel colours
            bgMain.color = positivePrimaryColour;
            //bottomPanel.color = positiveSecondaryColour;
            btnContinueBackground.color = positiveButtonColour;

            pointsPrefix = "+";
        }
        else
        {
            //set the popup panel colours
            bgMain.color = negativePrimaryColour;
            //bottomPanel.color = negativeSecondaryColour;
            btnContinueBackground.color = negativeButtonColour;
        }

        //populate the outcome text with the data from the JSON files
        txtHeading.text = card.Heading;
        txtSubHeading.text = card.SubHeading;
        txtBody.text = card.Description;
        txtPoints.text = pointsPrefix + card.Points + " Points";

        //display the card pile card, and update the position of the text overlay
        if (_cardId < 4) _cardPileHandler.EnableCard(_cardId + 1, true);
        _cardPileHandler.UpdateCardText(true, (_cardId == 4) ? 3 : 4);

        //find the GUI Handler class and update the GUI
        //GameObject.Find("MainGUI").GetComponent<GUIHandler>().UpdateTxtPoints();

        _btnContinue = transform.Find("btnContinue").GetComponent<Button>();
        _btnContinue.onClick.AddListener(BtnContinuePressed);

        //lerp the card to the middle of the screen
        StartCoroutine(LerpCardTransform(gameObject, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.7f, 0.7f, 0.7f), Quaternion.identity, 1.0f, false));
    }

    private void BtnContinuePressed()
    {
        //_btnPressed = true;
        //_lerpTimer = 0.0f;

        //_cardRestPos = new Vector3(-1500.0f, 0.0f, 0.0f);
        StopAllCoroutines();


        StartCoroutine(LerpCardTransform(gameObject, new Vector3(-1500.0f, 0.0f, 0.0f), new Vector3(0.7f, 0.7f, 0.7f), Quaternion.identity, 1.0f, true));
    }

    private void RemoveCard()
    {
        //update the final card that is in the pile
        if (_cardId == 0) _cardPileHandler.EnableCard(_cardId, true);

        //show the next outcome card or move onto the next stage if all the outcome cards are shown
        if (_cardId > 0)
        {
            //show next outcome card
            State.Set(State.GameState.ShowGuessPopup);
        }
        else
        {
            //move onto the next stage, allowing the next set of playing cards to be shown
            if (CardsPlayed.TotalCardsPlayedCount < 12)
            {
                //reset the game back to the initial state and increment the card type
                State.Set(State.GameState.SpawnPlayingCard);
                CardType.Increment();

                IsFirstRound = true;
            }
            else
            {
                //if all the playing cards, genetic and environment have been played then show the results
                State.Set(State.GameState.ShowResults);
            }
        }

        Destroy(transform.parent.gameObject);
    }

    private IEnumerator LerpCardTransform(GameObject c, Vector3 finalPos, Vector3 finalScale, Quaternion finalRot, float speed, bool removeCard)
    {
        float lerpTimer = 0.0f;
        Transform cardTransform = c.transform;

        while (lerpTimer < speed)
        {
            lerpTimer += Time.deltaTime;

            float deltaT = lerpTimer / speed;

            //perform the lerps
            cardTransform.localPosition = Vector3.Lerp(cardTransform.localPosition, finalPos, deltaT);
            cardTransform.localScale = Vector3.Lerp(cardTransform.localScale, finalScale, deltaT);
            cardTransform.localRotation = Quaternion.Lerp(cardTransform.localRotation, finalRot, deltaT);

            //apply the transformations
            c.transform.position = cardTransform.position;
            c.transform.localScale = cardTransform.localScale;
            c.transform.rotation = cardTransform.rotation;

            yield return null;
        }

        //remove the card and change the game state
        if (removeCard) RemoveCard();
    }
}