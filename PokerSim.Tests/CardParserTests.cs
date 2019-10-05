using NUnit.Framework;
using PokerSim.Cards;
using System.Linq;

namespace Tests
{
    public class CardParserTests
    {
        [Test]
        public void ParseCardsShouldSuccess()
        {
            var parsedCards = CardParser.ParseCards("2♥ 10♢ A♣ J♤").ToList();
            var expectedCards = new[]
            {
                new Card(CardValue.Two, Suit.Hearts),
                new Card(CardValue.Ten, Suit.Diamonds),
                new Card(CardValue.Ace, Suit.Clovers),
                new Card(CardValue.Jack, Suit.Spades),
            };

            CollectionAssert.AreEqual(expectedCards, actual: parsedCards);
        }
    }
}