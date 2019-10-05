using NUnit.Framework;
using PokerSim;
using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class PokerHandDetectorTests
    {
        public static IEnumerable<TestCaseData> DetectHandTestCases = (new[] {
            new DetectHandTestCase{
                Cards = "2♥ 4♢ 6♣ 8♤ 10♥ Q♢ A♥",
                ExpectedCombinationKind = CardCombinationKind.None,
                ExpectedCombinationCards = "",
                ExpectedKickers = "A♥ Q♢ 10♥ 8♤ 6♣"
            },
            new DetectHandTestCase{
                Cards = "A♥ 4♢ 6♣ 8♤ 10♥ Q♢ A♢",
                ExpectedCombinationKind = CardCombinationKind.OnePair,
                ExpectedCombinationCards = "A♥ A♢",
                ExpectedKickers = "Q♢ 10♥ 8♤"
            },
            new DetectHandTestCase{
                Cards = "A♥ 6♢ 6♣ 8♤ 10♥ Q♢ A♢",
                ExpectedCombinationKind = CardCombinationKind.TwoPairs,
                ExpectedCombinationCards = "A♥ A♢ 6♢ 6♣",
                ExpectedKickers = "Q♢"
            },
            new DetectHandTestCase{
                Cards = "A♥ 6♢ A♣ 8♤ 10♥ Q♢ A♢",
                ExpectedCombinationKind = CardCombinationKind.Set,
                ExpectedCombinationCards = "A♥ A♢ A♣",
                ExpectedKickers = "Q♢ 10♥"
            },
            new DetectHandTestCase{
                NameSuffix = "StartingWithAce",
                Cards = "A♥ 2♢ 3♣ 4♤ 5♥ Q♢ Q♣",
                ExpectedCombinationKind = CardCombinationKind.Straight,
                ExpectedCombinationCards = "A♥ 2♢ 3♣ 4♤ 5♥",
                ExpectedKickers = ""
            },
            new DetectHandTestCase{
                Cards = "2♢ 3♣ 4♤ 5♥ 6♥ Q♢ Q♣",
                ExpectedCombinationKind = CardCombinationKind.Straight,
                ExpectedCombinationCards = "2♢ 3♣ 4♤ 5♥ 6♥",
                ExpectedKickers = ""
            },
            new DetectHandTestCase{
                NameSuffix = "EndingWithAce",
                Cards = "10♢ J♣ K♤ Q♥ A♥ 2♢ 2♣",
                ExpectedCombinationKind = CardCombinationKind.Straight,
                ExpectedCombinationCards = "10♢ J♣ K♤ Q♥ A♥",
                ExpectedKickers = ""
            },
            new DetectHandTestCase{
                Cards = "2♢ 3♢ 4♢ 6♢ 7♢ Q♢ Q♣",
                ExpectedCombinationKind = CardCombinationKind.Flush,
                ExpectedCombinationCards = "3♢ 4♢ 6♢ 7♢ Q♢",
                ExpectedKickers = ""
            },
            new DetectHandTestCase{
                Cards = "2♢ 2♣ J♢ J♥ J♣ Q♢ Q♣",
                ExpectedCombinationKind = CardCombinationKind.FullHouse,
                ExpectedCombinationCards = "J♢ J♥ J♣ Q♢ Q♣",
                ExpectedKickers = ""
            },
            new DetectHandTestCase{
                Cards = "2♢ 2♣ 2♤ 2♥ J♣ A♢ Q♣",
                ExpectedCombinationKind = CardCombinationKind.FourOfKind,
                ExpectedCombinationCards = "2♢ 2♣ 2♤ 2♥",
                ExpectedKickers = "A♢"
            },
            new DetectHandTestCase{
                NameSuffix = "StartingWithAce",
                Cards = "3♥ A♥ 2♥ 4♥ 5♥ Q♢ Q♣",
                ExpectedCombinationKind = CardCombinationKind.StraightFlush,
                ExpectedCombinationCards = "A♥ 2♥ 3♥ 4♥ 5♥",
                ExpectedKickers = ""
            },
            new DetectHandTestCase{
                Cards = "2♥ 4♥ 5♥ 3♥ 6♥ Q♢ Q♣",
                ExpectedCombinationKind = CardCombinationKind.StraightFlush,
                ExpectedCombinationCards = "2♥ 3♥ 4♥ 5♥ 6♥",
                ExpectedKickers = ""
            },
            new DetectHandTestCase{
                NameSuffix = "EndingWithAce",
                Cards = "10♥ J♥ Q♥ A♥ K♥ 2♢ 2♣",
                ExpectedCombinationKind = CardCombinationKind.StraightFlush,
                ExpectedCombinationCards = "10♥ J♥ K♥ Q♥ A♥",
                ExpectedKickers = ""
            },
        }).Select(ToNUnitCase);

        [Test]
        [TestCaseSource(nameof(DetectHandTestCases))]
        public void PokerHandDetectorShouldDetectCombination(DetectHandTestCase testCase)
        {
            var detector = new PokerHandDetector();

            var hand = detector.DetectTopPokerHand(CardParser.ParseCards(testCase.Cards).ToHashSet());
            Assert.AreEqual(
                expected: testCase.ExpectedCombinationKind,
                actual: hand.CardCombination.Kind);

            var cardCombnation = hand.CardCombination as CardCombination;
            CollectionAssert.AreEquivalent(
                expected: CardParser.ParseCards(testCase.ExpectedCombinationCards),
                actual: cardCombnation.Cards);

            var expectedKickers = CardParser.ParseCards(testCase.ExpectedKickers).ToList();
            var actualKickers = hand.Kickers
                .OrderByDescending(k => k.Value)
                .ThenByDescending(k => k.Suit) // added to define ordering in test as it matters
                .ToList();

            CollectionAssert.AreEqual(
                expected: expectedKickers,
                actual: actualKickers
            );
        }

        private static TestCaseData ToNUnitCase(DetectHandTestCase handTestCase)
        {
            string name = "PokerHandDetectorShouldDetect_"
                + handTestCase.ExpectedCombinationKind.ToString()
                + (handTestCase.NameSuffix ?? "");

            return new TestCaseData(handTestCase)
                .SetName(name);
        }

        public class DetectHandTestCase
        {
            public string Cards { get; set; }

            public string NameSuffix { get; set; }

            public CardCombinationKind ExpectedCombinationKind { get; set; }

            public string ExpectedCombinationCards { get; set; }

            public string ExpectedKickers { get; set; }
        }
    }
}