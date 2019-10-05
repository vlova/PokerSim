using PokerSim.Cards;
using System;
using System.Collections.Generic;

namespace PokerSim
{

    class Program
    {
        static void Main(string[] args)
        {
            var playerCheatCards = new List<Card>() {
                new Card(CardValue.Ace, Suit.Clovers),
                //new Card(CardValue.Ace, Suit.Diamonds)
            };

            var deckCheatCards = new List<Card>() {
                //new Card(CardValue.Ace, Suit.Diamonds)
            };

            var simulationAmount = 1000;
            var wonAmount = 0;
            var wonSingleAmount = 0;

            for (var i = 0; i < simulationAmount; i++)
            {
                var playerIndex = 0;
                var session = new PokerSession(amountOfPlayers: 5);
                session.CheatPlayerCards(session.Players[playerIndex], playerCheatCards);
                session.CheatDeckCards(deckCheatCards);
                session.Simulate();
                var won = session.Winners.Contains(session.Players[playerIndex]);
                var wonSingle = won && session.Winners.Count == 1;
                if (won) { wonAmount += 1; }
                if (wonSingle) { wonSingleAmount += 1; }
            }

            Console.WriteLine($"{wonAmount}/{simulationAmount}");
            Console.WriteLine($"{wonSingleAmount}/{simulationAmount}");
        }
    }

}
