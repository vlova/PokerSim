using System;

namespace PokerSim.Cards
{
    public class Card
    {
        public Card(CardValue value, Suit suit)
        {
            Value = value;
            Suit = suit;
        }

        public CardValue Value { get; set; }

        public Suit Suit { get; set; }

        public override bool Equals(object obj)
        {
            var card = obj as Card;
            return card != null &&
                   Value == card.Value &&
                   Suit == card.Suit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Suit);
        }

        public override string ToString()
        {
            return $"{Value}:{Suit}";
        }
    }
}
