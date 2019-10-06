using NUnit.Framework;
using PokerSim;
using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class PokerHandComparisonTests
    {
        public static IEnumerable<TestCaseData> CompareHandsTestCases = (new[] {
            new CompareHandsTestCase{
                NameSuffix = "HighestKicker",
                CommonCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                AliceCards = "J♢ A♢",
                BobCards = "Q♥ K♢",
                ExpectedTopHandPlayer = TopHandPlayer.Alice
            },
            new CompareHandsTestCase{
                NameSuffix = "BothOfEqualKickers",
                CommonCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                AliceCards = "J♢ A♢",
                BobCards = "Q♥ A♥",
                ExpectedTopHandPlayer = TopHandPlayer.Both
            },
            new CompareHandsTestCase{
                NameSuffix = "PairVersusNothing",
                CommonCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                AliceCards = "2♢ Q♢",
                BobCards = "3♥ 9♢",
                ExpectedTopHandPlayer = TopHandPlayer.Alice
            },
            new CompareHandsTestCase{
                NameSuffix = "HighestPair",
                CommonCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                AliceCards = "2♢ Q♢",
                BobCards = "6♥ 9♢",
                ExpectedTopHandPlayer = TopHandPlayer.Bob
            },
            new CompareHandsTestCase{
                NameSuffix = "HighestKickerOnPair",
                CommonCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                AliceCards = "2♢ Q♢",
                BobCards = "2♣ 9♢",
                ExpectedTopHandPlayer = TopHandPlayer.Alice
            },
            new CompareHandsTestCase{
                NameSuffix = "HighestStraight",
                CommonCards = "A♥ 3♣ 4♤ 5♥ Q♣",
                AliceCards = "2♢ Q♢",
                BobCards = "6♥ 7♢",
                ExpectedTopHandPlayer = TopHandPlayer.Bob
            },
            new CompareHandsTestCase{
                NameSuffix = "HighestStraightEndingWithAce",
                CommonCards = "10♥ J♥ Q♥ K♥ 5♢",
                AliceCards = "A♥ 6♢",
                BobCards = "9♥ 7♢",
                ExpectedTopHandPlayer = TopHandPlayer.Alice
            },
        }).Select(ToNUnitCase);

        [Test]
        [TestCaseSource(nameof(CompareHandsTestCases))]
        public void PokerHandsComparisons(CompareHandsTestCase testCase)
        {
            var detector = new PokerHandDetector();
            var commonCards = CardParser.ParseCards(testCase.CommonCards);
            var aliceCards = CardParser.ParseCards(testCase.AliceCards);
            var bobCards = CardParser.ParseCards(testCase.BobCards);
            var aliceTopHand = detector.DetectTopPokerHand(aliceCards.Concat(commonCards).ToHashSet());
            var bobTopHand = detector.DetectTopPokerHand(bobCards.Concat(commonCards).ToHashSet());
            var winner = DetectWinner(aliceTopHand, bobTopHand);
            Assert.AreEqual(expected: testCase.ExpectedTopHandPlayer, actual: winner);
        }

        private static TopHandPlayer DetectWinner(PokerHand aliceTopHand, PokerHand bobTopHand)
        {
            var comparisonResult = aliceTopHand.CompareTo(bobTopHand);
            TopHandPlayer winner;
            if (comparisonResult < 0)
            {
                winner = TopHandPlayer.Bob;
            }
            else if (comparisonResult > 0)
            {
                winner = TopHandPlayer.Alice;
            }
            else
            {
                winner = TopHandPlayer.Both;
            }

            return winner;
        }

        private static TestCaseData ToNUnitCase(CompareHandsTestCase handTestCase)
        {
            string name = "InComparisonOfHandsShouldWin"
                + "_" + (handTestCase.NameSuffix ?? "");

            return new TestCaseData(handTestCase)
                .SetName(name);
        }

        public class CompareHandsTestCase
        {
            public string NameSuffix { get; set; }

            public string CommonCards { get; set; }

            public string AliceCards { get; set; }

            public string BobCards { get; set; }

            public TopHandPlayer ExpectedTopHandPlayer { get; set; }

        }

        public enum TopHandPlayer
        {
            Alice,
            Bob,
            Both
        }
    }
}