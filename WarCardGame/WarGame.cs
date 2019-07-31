using System;
using System.Collections.Generic;
using System.Text;

namespace WarCardGame
{
    public class WarGame
    {
        public readonly DeckBuilder _DeckBuilder = new DeckBuilder();
        public readonly bool ProgressOutPut;
        public readonly int TieDrawCount, TruceValue;

        private List<Player> Players;
        private Player P1 => Players[0];
        private Player P2 => Players[1];

        private int RoundCounter;

        public WarGame(Player p1, Player p2, 
            int tieDrawCount = 1,       //How many card to put aside in a tie
            bool pogressOutPut = true,  //Output inforation each round
            int truceVal = 25000000)    //When to stop playing
        {
            //Set Variables
            TieDrawCount = Math.Max(tieDrawCount, 1);
            ProgressOutPut = pogressOutPut;
            TruceValue = truceVal;

            Players = new List<Player>()
            {
                p1,
                p2
            };

            //Run Game
            SplitDeck();
            StartGame();
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
        }

        private void StartGame()
        {
            Console.WriteLine("Starting Game of War ->");
            while (true)
            {
                War();
                if (OutOfCards())
                {
                    Console.WriteLine("After " +
                        RoundCounter + " Rounds - " +
                        GetPlayWithCards().Name + " Won!!! ");
                    return;
                }
                else if(RoundCounter >= TruceValue)
                {
                    Console.WriteLine("After " +
                        RoundCounter + " Rounds - Both players grew tired and called a truce.");
                    return;
                }
            }
        }

        //Takes a deck and divides it between 2 players
        private void SplitDeck()
        {
            Queue<Card> Deck = new Queue<Card>(_DeckBuilder.GetShuffledDeck());
            int counter = 0;

            foreach (Card card in Deck)
            {
                Players[counter].AddCard(card);
                counter = (counter + 1) % Players.Count;
            }
        }


        private Player War()
        {
            if (OutOfCards()) return GetPlayWithCards();
            RoundCounter++;

            //Add new pair of cards to prize
            List<Card> prizeCards = new List<Card>
            {
                P1.Draw,
                P2.Draw
            };
            Player winner;

            //Last 2 elements of the prize are the ones we are comparing 
            int p1Index = prizeCards.Count - 2, p2Index = p1Index + 1;

            //Print out what cards are compared
            if (ProgressOutPut)
            {
                Console.Write(
                    "Round " + RoundCounter + ": " +
                    prizeCards[p1Index].ToString() +
                    " vs " +
                    prizeCards[p2Index].ToString());
            }

            //Get the winner
            if (prizeCards[p1Index].Value == prizeCards[p2Index].Value)
            {
                if (ProgressOutPut)
                {
                    Console.WriteLine(" - Tie");
                }
                winner = (DrawExtra(ref prizeCards)) ? War() : GetPlayWithCards();
            }
            else 
            {
                winner = (prizeCards[p1Index].Value > prizeCards[p2Index].Value) ? P1 : P2;
                if (ProgressOutPut)
                {
                    Console.WriteLine(" - " + winner.Name + " wins");
                }
            }
            
            //Add all the prizeCards to the winner's Deck 
            winner.AddCards(prizeCards);
            return winner;
        }

        private bool DrawExtra(ref List<Card> prizeCards)
        {
            for (int counter = 0; counter < TieDrawCount; counter++)
            {
                //Check if there is a card to draw
                if (OutOfCards()) return false;
                
                //Add new pair of cards to prize
                prizeCards.Add(P1.Draw);
                prizeCards.Add(P2.Draw);
            }
            return true;
        }

        //Given one player has no cards, return the player with cards
        private Player GetPlayWithCards() => (P1.CardsLeft() > 0) ? P1 : P2;

        //Check if there is a card to draw
        private bool OutOfCards() => (Math.Min(P1.CardsLeft(), P2.CardsLeft()) < 1);
    }
}
