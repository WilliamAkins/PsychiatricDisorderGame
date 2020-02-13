using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeGuessHandler : MonoBehaviour
{
    private string[] suit = new string[] { "clubs", "diamonds", "hearts", "spades" };

    private Image prevCardImg;

    private Button btnHigher;
    private Button btnLower;

    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = new Vector3(0.0f, -1000.0f, 0.0f);
        
        prevCardImg = transform.Find("bgLeft/card_main/card_face").GetComponent<Image>();

        btnHigher = transform.Find("btnHigher").GetComponent<Button>();
        btnLower = transform.Find("btnLower").GetComponent<Button>();

        btnHigher.onClick.AddListener(() => guessBtnPressed(true));
        btnLower.onClick.AddListener(() => guessBtnPressed(false));

        List<int> prevCard = GameData.CardsPlayed.GetPrevCard();
        Debug.Log(prevCard[0]);
        string cardName = "card_" + suit[prevCard[0]] + "_" + prevCard[1];
        prevCardImg.sprite = Resources.Load<Sprite>("card_faces/" + cardName);

        StartCoroutine(LerpGameObjectY(gameObject, 0.0f, 1.0f, false));
    }

    private void guessBtnPressed(bool guessedHigher) //guessedHigher should either be false for lower, or true for higher
    {
        if (GameData.State.Get() == GameData.State.GameState.MakeGuess)
        {
            if (!guessedHigher)
            {
                Debug.Log("You guessed lower.");
            }
            else
            {
                Debug.Log("You guessed higher.");
            }

            GameData.Guess.Higher = guessedHigher;

            //move to the next state
            GameData.State.Increment();

            StopAllCoroutines();
            StartCoroutine(LerpGameObjectY(gameObject, -1000.0f, 1.0f, true));
        }
    }

    private IEnumerator LerpGameObjectY(GameObject c, float finalY, float speed, bool destroyObject)
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

        if (destroyObject)
            Destroy(transform.parent.gameObject);
    }
}
