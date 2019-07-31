using System;
using System.Collections.Generic;
using System.Text;

namespace WarCardGame
{
    public class Player
    {
        public string Name { get; }
        private Queue<Card> Deck;

        public Player(string name = "Unknown")
        {
            Name = name;
            Deck = new Queue<Card>();
        }

        public int CardsLeft() => Deck.Count;

        public void AddCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Deck.Enqueue(card);
            }
        }

        public void AddCard(Card c) => Deck.Enqueue(c);

        public Card Draw => Deck.Dequeue();
    }
}
