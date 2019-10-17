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
        if (GameData.returnCurrentState() == GameData.GameState.showOutcomePopup && !GameData.returnIsFirstRound())
        {
            GameObject makeGuessPopup = Instantiate(Resources.Load("popup_outcome"), Vector3.zero, Quaternion.identity) as GameObject;

            GameData.incrementState();
        }
        else if (GameData.returnCurrentState() == GameData.GameState.showOutcomePopup && GameData.returnIsFirstRound()) //allow the usual game state progression order to be bypassed for the first round
        {
            //instantly go to making a guess without showing an outcome
            GameData.setCurrentState(GameData.GameState.ShowGuessPopup);
            GameData.setIsFirstRound(false);
        }

        //spawns the MakeGuess popup
        if (GameData.returnCurrentState() == GameData.GameState.ShowGuessPopup)
        {
            GameObject makeGuessPopup = Instantiate(Resources.Load("popup_MakeGuess"), Vector3.zero, Quaternion.identity) as GameObject;

            GameData.incrementState();
        }
    }
}
