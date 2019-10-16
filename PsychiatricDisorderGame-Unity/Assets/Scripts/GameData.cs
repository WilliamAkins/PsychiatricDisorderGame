using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameData
{
    public enum GameState { SpawnCard, MoveCard, showOutcomePopup, handleOutcome, ShowGuessPopup, MakingAGuess }
    private static GameState currentState = GameState.SpawnCard;
    
    //stores a list of all the different cards which have already been played
    private static List<List<int>> cardsPlayed = new List<List<int>>();

    private static int guess = -1; //says whether the player guessed lower or higher, 0 = lower, 1 = higher, -1 = None

    public static void incrementState()
    {
        currentState = currentState + 1;
        
        //should the current state exceed the allowed states then reset to state 0
        if ((int)currentState >= 6)
        {
            currentState = 0;
        }

        Debug.Log("State changed to " + currentState);
    }

    //allow a specific state to be set
    public static void setCurrentState(GameState newState)
    {
        currentState = newState;
    }

    public static GameState returnCurrentState()
    {
        return currentState;
    }

    //update the 2D list of cards which have been played
    public static void updateCardPlayedList(int cardSuit, int cardNum)
    {
        cardsPlayed.Add(new List<int>());
        cardsPlayed[cardsPlayed.Count - 1].Add(cardSuit);
        cardsPlayed[cardsPlayed.Count - 1].Add(cardNum);
    }

    public static int returnNumOfCardsPlayed()
    {
        return cardsPlayed.Count;
    }

    public static List<int> returnPrevCardPlayed()
    {
        return cardsPlayed.Last();
    }

    public static void makeGuess(int newGuess)
    {
        guess = newGuess;
    }

    public static int returnGuess()
    {
        return guess;
    }
}