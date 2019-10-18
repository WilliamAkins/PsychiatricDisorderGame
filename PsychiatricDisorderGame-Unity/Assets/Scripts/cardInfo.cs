using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardInfo : MonoBehaviour
{
    public int suitNum = 0;
    public int cardNum = 0;

    private string[] suit = new string[] { "clubs", "diamonds", "hearts", "spades" };

    private void Start()
    {
        //using the do, while loop, prevent muliple of the same card from spawning
        bool matchFound = false;
        do
        {
            //randomly generate the string used to find the correct card image
            suitNum = Random.Range(0, 4);
            cardNum = Random.Range(1, 14);

            for (int i = 0; i < GameData.returnNumOfCardsPlayed(); i++)
            {
                if (GameData.returnCardsPlayed()[i][0] == suitNum && GameData.returnCardsPlayed()[i][1] == cardNum)
                {
                    Debug.Log("there was a card match, resolving issue.");
                    matchFound = true;
                    break;
                }
                else if (i + 1 == GameData.returnNumOfCardsPlayed())
                {
                    //if all cards which were played were checked and no matches were flagged then set the bool to false
                    matchFound = false;
                }
            }
        } while (matchFound == true);

        string cardName = "card_" + suit[suitNum] + "_" + cardNum;

        Debug.Log(cardName);

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("card_faces/" + cardName);

        //store this new card within the played cards list
        GameData.updateCardPlayedList(suitNum, cardNum);

        //determine if the card that has been spawned is higher or lower than the previous card
        if (!GameData.returnIsFirstRound())
        {
            //check whether the cards value, ignoring the suit is the same as or higher than the card that was played before this one
            if (GameData.returnPrevCardPlayed()[1] >= GameData.returnSecondLastCardPlayed()[1])
            {
                Debug.Log("New card is higher than previous");
                GameData.setNewCardIsHigher(true);
            }
            else
            {
                Debug.Log("New card is lower than previous");
                GameData.setNewCardIsHigher(false);
            }
        }
    }
}
