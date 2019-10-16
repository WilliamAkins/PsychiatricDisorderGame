using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popup_makeGuess : MonoBehaviour
{
    private string[] suit = new string[] { "clubs", "diamonds", "hearts", "spades" };

    private Image prevCardImg;

    private Button btnHigher;
    private Button btnLower;

    // Start is called before the first frame update
    private void Start()
    {
        prevCardImg = transform.Find("bgLeft/card_main/card_face").GetComponent<Image>();

        btnHigher = transform.Find("btnHigher").GetComponent<Button>();
        btnLower = transform.Find("btnLower").GetComponent<Button>();

        btnHigher.onClick.AddListener(() => guessBtnPressed(1));
        btnLower.onClick.AddListener(() => guessBtnPressed(0));

        List<int> prevCard = GameData.returnPrevCardPlayed();
        Debug.Log(prevCard[0]);
        string cardName = "card_" + suit[prevCard[0]] + "_" + prevCard[1];
        prevCardImg.sprite = Resources.Load<Sprite>("card_faces/" + cardName);
    }

    private void guessBtnPressed(int guessNum) //guessNum should either be 0 for lower, or 1 for higher
    {
        if (GameData.returnCurrentState() == GameData.GameState.MakingAGuess)
        {
            if (guessNum == 0)
                Debug.Log("You guessed lower.");
            else
                Debug.Log("You guessed higher.");

            GameData.makeGuess(guessNum);

            //move to the next state
            GameData.incrementState();

            Destroy(transform.parent.gameObject);
        }
    }
}
