using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameData;

public class SpawnPopups : MonoBehaviour
{
    [SerializeField] private GameObject popupShowOutcome;
    [SerializeField] private GameObject popupMakeGuess;
    [SerializeField] private GameObject popupCardOutcome;
    [SerializeField] private GameObject popupResults;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //spawns the outcome popup
        if (State.Get() == State.GameState.ShowOutputPopup && !IsFirstRound)
        {
            GameObject showOutcomePopup = Instantiate(popupShowOutcome, Vector3.zero, Quaternion.identity) as GameObject;

            State.Increment();
        }
        //allow the usual game state progression order to be bypassed for the first round
        else if (State.Get() == State.GameState.ShowOutputPopup && IsFirstRound)
        {
            //instantly go to making a guess without showing an outcome
            State.Set(State.GameState.ShowGuessPopup);
            IsFirstRound = false;
        }

        //spawns the MakeGuess popup
        if (State.Get() == State.GameState.ShowGuessPopup && CardsPlayed.GetCount() < 6)
        {
            GameObject makeGuessPopup = Instantiate(popupMakeGuess, Vector3.zero, Quaternion.identity) as GameObject;

            State.Increment();
        }

        //if all the playing cards have been played then begin the next stage
        if (State.Get() == State.GameState.ShowGuessPopup && CardsPlayed.GetCount() == 6)
        {
            Instantiate(popupCardOutcome, Vector3.zero, Quaternion.identity);
            
            State.Set(State.GameState.MoveOutputCard);
        }

        //if all cards outcomes have been shown, now display the results
        if (State.Get() == State.GameState.ShowResults)
        {
            Instantiate(popupResults, Vector3.zero, Quaternion.identity);
            
            State.Increment();
        }

        //return to menu if in the return to menu state
        if (State.Get() == State.GameState.ReturnToMenu)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
