using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static GameData;

public class OutcomeHandler : MonoBehaviour
{
    [SerializeField] private Color positivePrimaryColour;
    [SerializeField] private Color positiveSecondaryColour;

    [SerializeField] private Color negativePrimaryColour;
    [SerializeField] private Color negativeSecondaryColour;

    //store a reference to the clickable continue button
    private Button btnContinue;

    private bool _lerpPopup = false;
    //private float _lerpTimer = 0.0f;

    private GameObject _pileCard = null;
    private int _pileCardId = 0;

    private void Start()
    {
        //determine whether the player guessed correctly
        if ((Guess.Higher && Guess.NewCardHigher) || (Guess.Higher == false && Guess.NewCardHigher == false))
            Guess.Correct = true;
        else
            Guess.Correct = false;

        GameObject btnContinueGo = transform.Find("btnContinue").gameObject;
        btnContinue = btnContinueGo.GetComponent<Button>();
        btnContinue.onClick.AddListener(BtnContinuePressed);

        
        List<Card> cards;

        btnContinue = btnContinueGo.GetComponent<Button>();
        TextMeshProUGUI txtHeading = transform.Find("txtHeading").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI txtSubHeading = transform.Find("txtSubHeading").GetComponent<TextMeshProUGUI>();
        //depending on whether the player chose correctly, display the correct outcome
        if (Guess.Correct)
        {
            //retrieve a list of all good outcomes
            if (CardType.Get() == CardType.Type.Genetics)
            {
                cards = CardOutcomes.Get().GeneticCards.GoodCards;
            }
            else
            {
                cards = CardOutcomes.Get().EnvironmentCards.GoodCards;
            }
                
            
            GetComponent<Image>().color = positivePrimaryColour;
            btnContinueGo.GetComponent<Image>().color = positiveSecondaryColour;

            txtHeading.text = "Good Choice!";
            txtSubHeading.text = "You Receive a good " + CardType.Get() + " card.";
        }
        else
        {
            //retrieve a list of all bad outcomes
            if (CardType.Get() == CardType.Type.Genetics)
            {
                cards = CardOutcomes.Get().GeneticCards.BadCards;
            }
            else
            {
                cards = CardOutcomes.Get().EnvironmentCards.BadCards;
            }
            
            GetComponent<Image>().color = negativePrimaryColour;
            btnContinueGo.GetComponent<Image>().color = negativeSecondaryColour;

            txtHeading.text = "Bad Choice";
            txtSubHeading.text = "You Receive a bad " + CardType.Get() + " card.";
        }
        

        //choose an outcome card for the player
        Card newCard = cards[Random.Range(0, cards.Count)];
        newCard.IsPositive = Guess.Correct;

        //store the new card
        if (CardType.Get() == CardType.Type.Genetics)
        {
            CardOutcomes.Genetic.Add(newCard);
        }
        else
        {
            CardOutcomes.Environment.Add(newCard);
        }

        //set the spawn position of the popup and move it to the centre of the screen
        transform.localPosition = new Vector3(0.0f, -1500.0f, 0.0f);
        StartCoroutine(LerpCardY(gameObject, 0.0f, 2.0f));
    }

    private void BtnContinuePressed()
    {
        if (CardType.Get() == CardType.Type.Genetics)
        {
            _pileCardId = CardOutcomes.Genetic.OutcomesReceivedCount - 1;
            _pileCard = CardOutcomes.Genetic.Pile.Get()[_pileCardId];
        }
        else
        {
            _pileCardId = CardOutcomes.Environment.OutcomesReceivedCount - 1;
            _pileCard = CardOutcomes.Environment.Pile.Get()[_pileCardId];
        }
        
        //stop the current coroutines and move the card into the pile card position
        Transform pileCardT = _pileCard.transform;
        StopAllCoroutines();
        StartCoroutine(LerpCardTransform(gameObject, pileCardT.position, pileCardT.localScale, pileCardT.rotation, 1.0f));
    }

    private void DestroyCard()
    {
        _pileCard.transform.parent.parent.GetComponent<CardPileHandler>().UpdateCard(true, _pileCardId, GetComponent<Image>().color);
        _pileCard.SetActive(true);
           
        //reset the state to the initial one
        State.Set(State.GameState.ShowGuessPopup);

        Destroy(transform.parent.gameObject);
    }

    private IEnumerator LerpCardTransform(GameObject c, Vector3 finalPos, Vector3 finalScale, Quaternion finalRot, float speed)
    {
        float lerpTimer = 0.0f;
        Transform cardTransform = c.transform;

        while (lerpTimer < speed)
        {
            lerpTimer += Time.deltaTime;

            float deltaT = lerpTimer / speed;

            //perform the lerps
            cardTransform.position = Vector3.Lerp(cardTransform.position, finalPos, deltaT);
            cardTransform.localScale = Vector3.Lerp(cardTransform.localScale, finalScale, deltaT);
            cardTransform.rotation = Quaternion.Lerp(cardTransform.rotation, finalRot, deltaT);

            //apply the transformations
            c.transform.position = cardTransform.position;
            c.transform.localScale = cardTransform.localScale;
            c.transform.rotation = cardTransform.rotation;

            yield return null;
        }

        DestroyCard();
    }

    private IEnumerator LerpCardY(GameObject c, float finalY, float speed)
    {
        float lerpTimer = 0.0f;
        Vector3 pos = c.transform.localPosition;

        while (lerpTimer < speed)
        {
            lerpTimer += Time.deltaTime;

            pos = Vector3.Lerp(pos, new Vector3(pos.x, finalY, pos.z), lerpTimer / speed);
            c.transform.localPosition = pos;

            yield return null;
        }
    }
}
