using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class SpawnCard : MonoBehaviour
{
    [SerializeField]
    private GameObject cardTemplate;

    private GameObject currentCard;

    private Vector3 initalCardPos = new Vector3(-3.5f, 2.2f, 0.0f);
    private Vector3 cardRestPos;

    private bool _removingCards = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        cardRestPos = initalCardPos;
    }

    // Update is called once per frame
    private void Update()
    {
        //spawn a new playing card but set a limit of 6 on screen at once, then move the card to the centre of the screen
        if (State.Get() == State.GameState.SpawnPlayingCard && CardsPlayed.GetCount() < 6 && CardsPlayed.TotalCardsPlayedCount < 12)
        {
            PickCard();
        }

        //remove all the playing cards off the screen
        if (State.Get() == State.GameState.SpawnPlayingCard && !_removingCards && CardsPlayed.Get().Count > 0)
        {
            _removingCards = true;

            RemovePlayingCards();
        }
    }

    private void PickCard()
    {
        //Instantiate a blank template before any card data is assigned to it.
        currentCard = Instantiate(cardTemplate, new Vector3(0.0f, 20.0f, 0.0f), Quaternion.identity);
        currentCard.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        StartCoroutine(LerpCardY(currentCard, 0.0f, 2.0f, false, true));

        //since a card has been spawned, move to the next state
        State.Increment();
    }

    private void RemovePlayingCards()
    {
        List<GameObject> cards = CardsPlayed.GetAsGameObject();

        foreach (var c in cards)
        {
            StartCoroutine(LerpCardY(c, 20.0f, 3.0f, true, false));
        }
    }

    private void DestroyCards()
    {
        List<GameObject> cards = CardsPlayed.GetAsGameObject();

        //removal all the cards
        foreach (var c in cards)
        {
            Destroy(c);
        }
        CardsPlayed.RemoveAll();

        _removingCards = false;

        //reset card pos
        cardRestPos = initalCardPos;
    }

    private IEnumerator LerpCardTransform(GameObject c, Vector3 finalPos, Vector3 finalScale, float speed)
    {
        float lerpTimer = 0.0f;
        Transform cardTransform = c.transform;
        //Transform t = c.transform;

        while (lerpTimer < speed)
        {
            lerpTimer += Time.deltaTime;

            float deltaT = lerpTimer / speed;

            //perform the lerps
            cardTransform.localPosition = Vector3.Lerp(cardTransform.localPosition, finalPos, deltaT);
            cardTransform.localScale = Vector3.Lerp(cardTransform.localScale, finalScale, deltaT);

            //apply the transformations
            c.transform.position = cardTransform.position;
            c.transform.localScale = cardTransform.localScale;

            yield return null;
        }

        //ensure that this card will always be behind any new cards
        currentCard.GetComponent<SpriteRenderer>().sortingOrder = 1;
        currentCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;

        //ensure the scale once the lerp finishes is exactly 0.3f
        currentCard.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        //move the position for where the next card will spawn
        cardRestPos = new Vector3(cardRestPos.x + 3.5f, cardRestPos.y, 0.0f);
        if (CardsPlayed.GetCount() == 3)
            cardRestPos = new Vector3(initalCardPos.x, cardRestPos.y - 4.6f, 0.0f);

        //now that the card has stopped moving, move to the next state
        State.Increment();
        Debug.Log("Card has reached its destination");
    }

    private IEnumerator LerpCardY(GameObject c, float destY, float speed, bool destroyCard, bool lerpCard)
    {
        float lerpTimer = 0.0f;
        var pos = c.transform.position;

        while (lerpTimer + 0.5f < speed)
        {
            lerpTimer += Time.deltaTime;

            pos = Vector3.Lerp(pos, new Vector3(pos.x, destY, pos.y), lerpTimer / speed);
            c.transform.position = pos;

            yield return null;
        }

        if (lerpCard)
        {
            StartCoroutine(LerpCardTransform(c, cardRestPos, new Vector3(0.3f, 0.3f, 0.3f), 2.0f));
        }

        if (destroyCard)
        {
            //once the lerp has finished destroy the cards
            DestroyCards();
        }
    }
}
