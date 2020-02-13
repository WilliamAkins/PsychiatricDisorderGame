using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class cardInfo : MonoBehaviour
{
    [SerializeField] private int suitNum;
    [SerializeField] private int cardNum;

    private readonly string[] _suit = { "clubs", "diamonds", "hearts", "spades" };

    private void Start()
    {
        //using the do, while loop, prevent multiple of the same card from spawning
        bool matchFound = false;

        do
        {
            //randomly generate the string used to find the correct card image
            suitNum = Random.Range(0, 4);
            cardNum = Random.Range(1, 14);

            for (int i = 0; i < CardsPlayed.GetCount(); i++)
            {
                if (CardsPlayed.Get()[i][0] == suitNum && CardsPlayed.Get()[i][1] == cardNum)
                {
                    Debug.Log("there was a card match, resolving issue.");
                    matchFound = true;
                    break;
                }
                else if (i + 1 == CardsPlayed.GetCount())
                {
                    //if all cards which were played were checked and no matches were flagged then set the bool to false
                    matchFound = false;
                }
            }
        } while (matchFound);

        string cardName = "card_" + _suit[suitNum] + "_" + cardNum;

        Debug.Log(cardName);

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("card_faces/" + cardName);

        //store this new card within the played cards list
        CardsPlayed.UpdateList(suitNum, cardNum, this.gameObject);

        //determine if the card that has been spawned is higher or lower than the previous card
        if (!IsFirstRound)
        {
            //check whether the cards value, ignoring the suit is the same as or higher than the card that was played before this one
            if (CardsPlayed.GetPrevCard()[1] >= CardsPlayed.GetPrevCard(1)[1])
            {
                Debug.Log("New card is higher than previous");
                Guess.NewCardHigher = true;
            }
            else
            {
                Debug.Log("New card is lower than previous");
                Guess.NewCardHigher = false;
            }
        }
    }
}
