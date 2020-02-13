using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameData.CardOutcomes;

public class CardPileHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] environmentCards;
    [SerializeField] private TextMeshProUGUI txtEnvironmentCards;

    [SerializeField] private GameObject[] geneticCards;
    [SerializeField] private TextMeshProUGUI txtGeneticCards;
    
    // Start is called before the first frame update
    private void Start()
    {
        Environment.Pile.Setup(environmentCards);
        Genetic.Pile.Setup(geneticCards);

        //begin with each environment card being disabled
        foreach (var c in environmentCards)
        {
            c.SetActive(false);
        }
        txtEnvironmentCards.gameObject.SetActive(false);

        //begin with each genetic card being disabled
        foreach (var c in geneticCards)
        {
            c.SetActive(false);
        }
        txtGeneticCards.gameObject.SetActive(false);
    }

    public void UpdateCard(bool showCardHeading, int id, Color cardColour)
    {
        //update both the card text and colour

        if (GameData.CardType.Get() == GameData.CardType.Type.Genetics)
        {
            geneticCards[id].SetActive(true);
            geneticCards[id].GetComponent<Image>().color = cardColour;

            if (txtGeneticCards)
            {
                txtGeneticCards.gameObject.SetActive(showCardHeading);
                txtGeneticCards.gameObject.transform.position = geneticCards[id].transform.position;
            }
        }
        else
        {
            environmentCards[id].SetActive(true);
            environmentCards[id].GetComponent<Image>().color = cardColour;
            
            if (txtEnvironmentCards)
            {
                txtEnvironmentCards.gameObject.SetActive(showCardHeading);
                txtEnvironmentCards.gameObject.transform.position = environmentCards[id].transform.position;
            }
        }
    }

    public void UpdateCardText(bool showCardHeading, int id)
    {
        //only update the card text and not the colour

        if (GameData.CardType.Get() == GameData.CardType.Type.Genetics)
        {
            if (txtGeneticCards)
            {
                txtGeneticCards.gameObject.SetActive(showCardHeading);
                txtGeneticCards.gameObject.transform.position = geneticCards[id].transform.position;
            }
        }
        else
        {
            if (txtEnvironmentCards)
            {
                txtEnvironmentCards.gameObject.SetActive(showCardHeading);
                txtEnvironmentCards.gameObject.transform.position = environmentCards[id].transform.position;
            }
        }
    }

    public void EnableCard(int id, bool shouldEnable)
    {
        if (GameData.CardType.Get() == GameData.CardType.Type.Genetics)
        {
            geneticCards[id].SetActive(shouldEnable);
        }
        else
        {
            environmentCards[id].SetActive(shouldEnable);
        }
    }
}