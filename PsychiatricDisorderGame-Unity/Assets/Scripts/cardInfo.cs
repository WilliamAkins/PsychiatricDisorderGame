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
        //randomly generate the string used to find the correct card image
        suitNum = Random.Range(0, 4);
        cardNum = Random.Range(1, 14);
        string cardName = "card_" + suit[suitNum] + "_" + cardNum;

        Debug.Log(cardName);

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("card_faces/" + cardName);

        //store this new card within the played cards list
        GameData.updateCardPlayedList(suitNum, cardNum);
    }
}
