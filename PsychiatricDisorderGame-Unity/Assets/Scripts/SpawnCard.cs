using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    [SerializeField]
    private GameObject cardTemplate;

    private GameObject currentCard;
    
    // Start is called before the first frame update
    private void Start()
    {
        //Instantiate a blank template before any card data is assigned to it.
        currentCard = Instantiate(cardTemplate, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void pickCard()
    {

    }
}
