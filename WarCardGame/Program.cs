using System;

namespace WarCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            WarGame game = new WarGame(
                new Player("Player 1"), 
                new Player("Player 2"),
                3); //Default pogressOutPut and truceVal currently set.
        }
    }
}
