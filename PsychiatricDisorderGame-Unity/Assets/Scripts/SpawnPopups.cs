using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPopups : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //spawns the outcome popup
        if (GameData.returnCurrentState() == GameData.GameState.showOutcomePopup)
        {
            GameObject makeGuessPopup = Instantiate(Resources.Load("popup_outcome"), Vector3.zero, Quaternion.identity) as GameObject;

            GameData.incrementState();
        }

        //spawns the MakeGuess popup
        if (GameData.returnCurrentState() == GameData.GameState.ShowGuessPopup)
        {
            GameObject makeGuessPopup = Instantiate(Resources.Load("popup_MakeGuess"), Vector3.zero, Quaternion.identity) as GameObject;

            GameData.incrementState();
        }
    }
}
