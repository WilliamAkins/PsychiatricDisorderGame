using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameData
{
    public enum GameState { SpawnCard, MoveCard, showOutcomePopup, handleOutcome, ShowGuessPopup, MakingAGuess }
    private static GameState currentState = GameState.SpawnCard;

    private static Outcome outcomes; //stores the data that was loaded from outcomes JSON
    
    private static List<List<int>> cardsPlayed = new List<List<int>>(); //stores a list of all the different cards which have already been played
    private static bool newCardIsHigher = false; //says whether the new card is higher or lower than the previous one played
    private static bool guessedHigher = false; //says whether the player guessed lower or higher | false = lower, true = higher
    private static bool isFirstRound = true; //says whether we are on the first round of showing cards and therefore there is only 0 - 1 cards in the game
    private static int points = 0; //the points that the player has


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

    public static void setOutcomes(Outcome newOutcomes)
    {
        outcomes = newOutcomes;
    }

    public static Outcome returnOutcome()
    {
        return outcomes;
    }

    //update the 2D list of cards which have been played
    public static void updateCardPlayedList(int cardSuit, int cardNum)
    {
        cardsPlayed.Add(new List<int>());

        cardsPlayed.Last().Add(cardSuit);
        cardsPlayed.Last().Add(cardNum);
    }

    public static int returnNumOfCardsPlayed()
    {
        return cardsPlayed.Count;
    }

    public static List<int> returnPrevCardPlayed()
    {
        return cardsPlayed.Last();
    }

    public static List<int> returnSecondLastCardPlayed() //needed for comparison to the previous card that was played
    {
        return cardsPlayed[cardsPlayed.Count - 2];
    }

    public static List<List<int>> returnCardsPlayed()
    {
        return cardsPlayed;
    }

    public static void makeGuess(bool newGuess)
    {
        guessedHigher = newGuess;
    }

    public static bool returnGuessedHigher()
    {
        return guessedHigher;
    }

    public static void setIsFirstRound(bool newIsFirstRound)
    {
        isFirstRound = newIsFirstRound;
    }

    public static bool returnIsFirstRound()
    {
        return isFirstRound;
    }

    public static bool returnNewCardIsHigher()
    {
        return newCardIsHigher;
    }

    public static void setNewCardIsHigher(bool cardBool)
    {
        newCardIsHigher = cardBool;
    }

    public static void addToPoints(int num)
    {
        points += num;
    }

    public static void setPoints(int num)
    {
        points = num;
    }

    public static int returnPoints()
    {
        return points;
    }
}