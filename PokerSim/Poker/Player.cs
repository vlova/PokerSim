using PokerSim.Cards;
using System;
using System.Collections.Generic;

namespace PokerSim
{
    public class Player
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public decimal PlayerPot { get; set; }

        public List<Card> Cards { get; set; }

        public IPlayerStrategy Strategy { get; set; }

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
