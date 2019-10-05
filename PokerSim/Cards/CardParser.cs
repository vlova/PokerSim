using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerSim.Cards
{
    public static class CardParser
    {
        private static Dictionary<char, Suit> CharsToSuits = new Dictionary<char, Suit> {
            { '♥', Suit.Hearts},
            { '♢', Suit.Diamonds },
            { '♣', Suit.Clovers },
            { '♤', Suit.Spades }
        };

        private static Dictionary<char, CardValue> CharsToCardValues = new Dictionary<char, CardValue> {
            { 'A', CardValue.Ace},
            { 'K', CardValue.King},
            { 'Q', CardValue.Queen},
            { 'J', CardValue.Jack}
        };

        public static Card ParseCard(string str)
        {
            if (!(str.Length == 2 || str.Length == 3)) throw new ArgumentException("Incorrect length", nameof(str));
            var suitChar = str.Last();
            if (!CharsToSuits.ContainsKey(suitChar))
            {
                throw new ArgumentException($"Expected to find suit, but found {suitChar}", nameof(str));
            }

            var suit = CharsToSuits[suitChar];
            var cardValueStr = str.Substring(0, length: str.Length - 1);
            var cardValue = ParseCardValue(cardValueStr);
            return new Card(cardValue, suit);
        }

        public static IEnumerable<Card> ParseCards(string str)
        {
            if (str.Length == 0)
            {
                return Enumerable.Empty<Card>();
            }

            return str.Split(' ').Select(ParseCard);
        }

        private static CardValue ParseCardValue(string cardValueStr)
        {
            if (cardValueStr == "10")
            {
                return CardValue.Ten;
            }

            if (cardValueStr.Length != 1)
            {
                throw new ArgumentException($"Expected to find card value, but found {cardValueStr}", nameof(cardValueStr));
            }

            var cardValueChar = cardValueStr[0];
            if (char.IsDigit(cardValueChar))
            {
                return (CardValue)int.Parse(cardValueChar + "");
            }

            if (!CharsToCardValues.ContainsKey(cardValueChar))
            {
                throw new ArgumentException($"Expected to find card value, but found {cardValueStr}", nameof(cardValueStr));
            }

            return CharsToCardValues[cardValueChar];
        }
    }
}
