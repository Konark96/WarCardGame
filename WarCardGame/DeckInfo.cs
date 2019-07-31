using System;
using System.Collections.Generic;
using System.Text;

namespace WarCardGame
{
    public enum Suits
    {
        Club,
        Diamond,
        Spade,
        Heart
    };

    public class Card
    {
        public const int ACE_VAL = 14;
        public int Value { get; }
        public Suits Suit { get; }

        public Card(Suits suit, int value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            string valueToString;
            switch (Value)
            {
                case ACE_VAL:
                    valueToString = "Ace";
                    break;
                case (ACE_VAL-1):
                    valueToString = "King";
                    break;
                case (ACE_VAL-2):
                    valueToString = "Queen";
                    break;
                case (ACE_VAL-3):
                    valueToString = "Jack";
                    break;
                default:
                    valueToString = Value.ToString();
                    break;
            }
            return valueToString;
        }
    }

    public class DeckBuilder
    {
        private static readonly object Padlock = new object();

        private static object Instance = null;
        private static List<Card> StockDeck = new List<Card>();

        public DeckBuilder()
        {
            lock (Padlock)
            {
                //Do not need multiple instances of class because functionality is always the same.
                if (Instance == null)
                {
                    Instance = new object();
                    Array suitVals = Enum.GetValues(typeof(Suits));
                    for (int val = 2; val <= Card.ACE_VAL; val++)
                    {
                        foreach (Suits suit in suitVals)
                        {
                            StockDeck.Add(new Card(suit, val));
                        }
                    }
                }
            }
        }

        //Uses a "modern" Fisher–Yates shuffle
        public List<Card> GetShuffledDeck()
        {
            Random rand = new Random();
            List<Card> deck = new List<Card>();

            int index;
            Card temp;

            //Shuffle Deck with each card having an equal chance of getting any position.
            for (int maxRange = StockDeck.Count - 1; maxRange >= 0; maxRange--)
            { 
                index = rand.Next(maxRange);
                temp = StockDeck[index];
                StockDeck.RemoveAt(index);
                StockDeck.Add(temp);

                //Deep Copy
                deck.Add(new Card(temp.Suit, temp.Value));
            }

            return deck;
        }
    }
}
