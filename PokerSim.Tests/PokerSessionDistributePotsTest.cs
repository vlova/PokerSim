using NUnit.Framework;
using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Tests
{
    class PokerSessionDistributePotsTest
    {
        public static IEnumerable<TestCaseData> TestCases = (new[] {
            new DistributePotTestCase
            {
                NameSuffix = "WinnerShouldReceiveEverything",
                CommunityCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                Players = new []{
                    new PlayerTestData{
                        Cards = "J♢ A♢",
                        ExpectedPot = 3,
                        Folds = false,
                    },
                    new PlayerTestData{
                        Cards = "Q♥ K♢",
                        ExpectedPot = 0,
                        Folds = false,
                    },
                    new PlayerTestData{
                        Cards = "3♥ 9♢",
                        ExpectedPot = 0,
                        Folds = false,
                    }
                },
                PlayerPot = 1,
            },
            new DistributePotTestCase
            {
                NameSuffix = "WinnersShouldSplitPot",
                CommunityCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                Players = new []{
                    new PlayerTestData{
                        Cards = "J♢ A♢",
                        ExpectedPot = 1.5M,
                        Folds = false,
                    },
                    new PlayerTestData{
                        Cards = "Q♥ A♥",
                        ExpectedPot = 1.5M,
                        Folds = false,
                    },
                    new PlayerTestData{
                        Cards =  "3♥ 9♢",
                        ExpectedPot = 0,
                        Folds = false,
                    }
                },
                PlayerPot = 1,
            },
            new DistributePotTestCase
            {
                NameSuffix = "WinnerShouldReceiveEverythingEvenIfSomeoneFoldes",
                CommunityCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                Players = new []{
                    new PlayerTestData{
                        Cards = "J♢ A♢",
                        ExpectedPot = 2M,
                        Folds = false,
                    },
                    new PlayerTestData{
                        Cards = "Q♥ A♥",
                        ExpectedPot = 1M,
                        Folds = true,
                    },
                    new PlayerTestData{
                        Cards =  "3♥ 9♢",
                        ExpectedPot = 0,
                        Folds = false,
                    }
                },
                PlayerPot = 1,
            },
            new DistributePotTestCase
            {
                NameSuffix = "WhenFirstFoldsShouldSplitPot",
                CommunityCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                Players = new []{
                    new PlayerTestData{
                        Cards = "J♢ A♢",
                        ExpectedPot = 1,
                        Folds = true,
                    },
                    new PlayerTestData{
                        Cards = "Q♥ A♥",
                        ExpectedPot = 1,
                        Folds = false,
                    }
                },
                PlayerPot = 1,
            },
            new DistributePotTestCase
            {
                NameSuffix = "WhenSecondFoldsShouldSplitPot",
                CommunityCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                Players = new []{
                    new PlayerTestData{
                        Cards = "J♢ A♢",
                        ExpectedPot = 1,
                        Folds = false,
                    },
                    new PlayerTestData{
                        Cards = "Q♥ A♥",
                        ExpectedPot = 1,
                        Folds = true,
                    }
                },
                PlayerPot = 1,
            },
            new DistributePotTestCase
            {
                NameSuffix = "WhenBothFoldsShouldSplitPot",
                CommunityCards = "2♥ 4♣ 6♤ 8♥ 10♣",
                Players = new []{
                    new PlayerTestData{
                        Cards = "J♢ A♢",
                        ExpectedPot = 1,
                        Folds = true,
                    },
                    new PlayerTestData{
                        Cards = "Q♥ A♥",
                        ExpectedPot = 1,
                        Folds = true,
                    },
                    new PlayerTestData{
                        Cards = "J♥ K♥",
                        ExpectedPot = 1,
                        Folds = false,
                    }
                },
                PlayerPot = 1,
            }
        }).Select(ToNUnitCase);

        [TestCaseSource(nameof(TestCases))]
        public static void PokerSessionShouldDistributePotsCorrectly(DistributePotTestCase testCase)
        {
            var players = testCase.Players.Select(p => new Player
            {
                PlayerPot = testCase.PlayerPot,
                Cards = CardParser.ParseCards(p.Cards).ToList(),
                Strategy = p.Folds
                        ? (IPlayerStrategy)new FoldAlwaysStategy()
                        : new AllInStrategy()
            }).ToList();

            var session = new PokerSession(players);
            session.CheatCommunityCards(CardParser.ParseCards(testCase.CommunityCards).ToList());
            session.Simulate();

            CollectionAssert.AreEqual(
                expected: testCase.Players.Select(p => p.ExpectedPot).ToList(),
                actual: players.Select(p => p.PlayerPot)
                );
        }

        public class DistributePotTestCase
        {
            public string NameSuffix { get; set; }

            public string CommunityCards { get; set; }

            public PlayerTestData[] Players { get; set; }

            public int PlayerPot { get; set; }
        }

        public class PlayerTestData
        {
            public string Cards { get; set; }

            public bool Folds { get; set; }

            public decimal ExpectedPot { get; set; }
        }

        private static TestCaseData ToNUnitCase(DistributePotTestCase handTestCase)
        {
            string name = "PokerSessionShouldDistributePotsCorrectly"
                + "_" + (handTestCase.NameSuffix ?? "");

            return new TestCaseData(handTestCase)
                .SetName(name);
        }
    }
}
