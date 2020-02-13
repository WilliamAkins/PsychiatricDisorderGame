using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;
using TMPro;

public static class GameData
{
    public static bool IsFirstRound { set; get; } = true; //says whether we are on the first round of showing cards and therefore there is only 0 - 1 cards in the game

    //public static bool GuessedHigher { set; get; } = false; //says whether the player guessed lower or higher | false = lower, true = higher

    //public static bool NewCardIsHigher { set; get; } = false; //says whether the new card is higher or lower than the previous one played

    //public static int CardType { get; set; } = 0; //0 = Genetic Cards, 1 = Environment Cards

   // public static bool GuessCorrect { set; get; } = false;

   public struct CardType
   {
       public enum Type { Genetics, Environment }
       private static Type _currentType = Type.Genetics;

       public static void Set(Type newType) => _currentType = newType;

       public static Type Get() => _currentType;

       public static void Increment()
       {
           _currentType++;
        
           //should the current state exceed the allowed states then reset to state 0
           if ((int)_currentType >= 2) _currentType = 0;

           Debug.Log("Card type changed to " + _currentType);
       }
   }

    public struct State
    {
        //public enum GameState { SpawnCard, MoveCard, ShowOutcomePopup, HandleOutcome, ShowGuessPopup, MakingAGuess }

        public enum GameState { ShowGuessPopup, MakeGuess, SpawnPlayingCard, MovePlayingCard, ShowOutputPopup, MoveOutputPopup, MoveOutputCard, ShowNextOutputCard, ShowResults, EndGame, ReturnToMenu }

        private static GameState _currentState = GameState.SpawnPlayingCard;

        public static void Set(GameState newState) => _currentState = newState;

        public static GameState Get() => _currentState;

        public static void Increment()
        {
            _currentState++;
        
            //should the current state exceed the allowed states then reset to state 0
            if ((int)_currentState >= 11) _currentState = 0;

            Debug.Log("State changed to " + _currentState);
        }
    }

    public struct CardOutcomes
    {
        //stores the data that was loaded from outcomes JSON
        private static Outcome _outcome;

        public static void Set(Outcome outcome) => _outcome = outcome;
        public static Outcome Get() => _outcome;

        public struct Genetic
        {
            private static List<Card> _OutcomesReceived = new List<Card>();

            public static List<Card> Received() => _OutcomesReceived;

            public static void Add(Card newCard) => _OutcomesReceived.Add(newCard);

            public static int OutcomesReceivedCount => _OutcomesReceived.Count;

            public struct Pile
            {
                public static int OutcomesViewed { set; get; } = 1;
                
                private static GameObject[] _cardPile;

                public static void Setup(GameObject[] newCardPile) => _cardPile = newCardPile;

                public static GameObject[] Get() => _cardPile;
            }
        }

        public struct Environment
        {
            private static List<Card> _outcomesReceived = new List<Card>();

            public static List<Card> Received() => _outcomesReceived;

            public static void Add(Card newCard) => _outcomesReceived.Add(newCard);

            public static int OutcomesReceivedCount => _outcomesReceived.Count;

            public struct Pile
            {
                public static int OutcomesViewed { set; get; } = 1;
                
                private static GameObject[] _cardPile;

                public static void Setup(GameObject[] newCardPile) => _cardPile = newCardPile;

                public static GameObject[] Get() => _cardPile;
            }
        }
    }

    public struct CardsPlayed
    {
        //stores a list of all the different cards which have already been played
        private static List<List<int>> _cardsPlayed = new List<List<int>>();
        private static List<GameObject> _cardsPlayedGameObject = new List<GameObject>();

        public static List<List<int>> Get() => _cardsPlayed;

        public static List<GameObject> GetAsGameObject() => _cardsPlayedGameObject;

        public static int TotalCardsPlayedCount { private set; get; }

        //update the 2D list of cards which have been played and store the respective gameobject displaying the card
        public static void UpdateList(int cardSuit, int cardNum, GameObject cardGameObject)
        {
            _cardsPlayed.Add(new List<int>());

            _cardsPlayed.Last().Add(cardSuit);
            _cardsPlayed.Last().Add(cardNum);

            _cardsPlayedGameObject.Add(cardGameObject);

            TotalCardsPlayedCount++;
        }

        public static int GetCount() => _cardsPlayed.Count;

        //Get a specific card, counting backwards from the end
        public static List<int> GetPrevCard(int count = 0) => _cardsPlayed[_cardsPlayed.Count - 1 - count];

        public static void RemoveAll()
        {
            _cardsPlayed.Clear();
            _cardsPlayedGameObject.Clear();
        }
    }

    public struct Guess
    {
        public static bool Higher { set; get; } = false; //says whether the player guessed lower or higher | false = lower, true = higher

        public static bool NewCardHigher { set; get; } = false;

        public static bool Correct { set; get; } = false;
    }

    public struct Points
    {
        private static int _combinedPoints;
        private static int _geneticPoints;
        private static int _environmentPoints;

        public struct Combined
        {
            public static void Set(int points) => _combinedPoints = points;
            public static void Increment(int points) => _combinedPoints += points;
            public static int Get() => _combinedPoints;
        }

        public struct Genetic
        {
            public static void Set(int points) => _geneticPoints = points;

            public static void Increment(int points) => _combinedPoints += points;
            public static int Get() => _combinedPoints;
        }

        public struct Environment
        {
            public static void Set(int points) => _environmentPoints = points;

            public static void Increment(int points) => _environmentPoints += points;
            public static int Get() => _environmentPoints;
        }
    }
}