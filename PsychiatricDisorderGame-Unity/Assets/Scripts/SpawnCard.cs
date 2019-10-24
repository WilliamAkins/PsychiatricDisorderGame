using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    [SerializeField]
    private GameObject cardTemplate;

    private GameObject currentCard;

    private float timer = 0.0f;

    private Vector3 initalCardPos = new Vector3(-7.0f, 2.2f, 0.0f);
    private Vector3 cardRestPos;
    
    // Start is called before the first frame update
    private void Start()
    {
        cardRestPos = initalCardPos;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameData.returnCurrentState() == GameData.GameState.SpawnCard)
        {
            pickCard();
        }

        if (GameData.returnCurrentState() == GameData.GameState.MoveCard)
        {
            timer += 1.0f * Time.deltaTime;

            //begin to move the card into its rest position
            if (timer > 2.0f)
            {
                currentCard.transform.position = Vector3.Lerp(currentCard.transform.position, cardRestPos, 0.1f);
                currentCard.transform.localScale = Vector3.Lerp(currentCard.transform.localScale, new Vector3(0.3f, 0.3f, 0.3f), 0.1f);

                if (currentCard.transform.position == cardRestPos)
                {
                    //ensure that this card will always be behind any new cards
                    currentCard.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    currentCard.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;

                    //ensure the scale once the lerp finishes is exactly 0.3f
                    currentCard.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                    cardRestPos = new Vector3(cardRestPos.x + 3.5f, cardRestPos.y, 0.0f);

                    if (GameData.returnNumOfCardsPlayed() == 5)
                    {
                        cardRestPos = new Vector3(initalCardPos.x, cardRestPos.y - 4.6f, 0.0f);
                    }

                    //reset the timer for the next card
                    timer = 0.0f;

                    //now that the card has stopped moving, move to the next state
                    GameData.incrementState();
                    Debug.Log("Card has reached its destination");
                }
            }
        }
    }

    private void pickCard()
    {
        //Instantiate a blank template before any card data is assigned to it.
        currentCard = Instantiate(cardTemplate, Vector3.zero, Quaternion.identity);
        currentCard.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        //since a card has been spawned, move to the next state
        GameData.incrementState();
    }
}
