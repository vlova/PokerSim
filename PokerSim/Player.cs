using PokerSim.Cards;
using System;
using System.Collections.Generic;

namespace PokerSim
{
    class Player
    {
        public Guid Id { get; set; }
        public List<Card> Cards { get; set; }

        public Player()
        {
            this.Cards = new List<Card>();
            this.Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }

}
