using CsvHelper.Configuration.Attributes;
using PokerSim.Cards;
using PokerSim.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Simulations
{
    class AllInSimulation
    {
        public IEnumerable<SimulationResult> Run(SimulationOptions options)
        {
            var cardPairs = GeneratePairs(GenerateCards());

            foreach (var cards in cardPairs)
            {
                IntermediateSimulationResult result = ComputeChances(options, cards.ToList());

                var wonRate = (double)result.Won / result.Runs;
                var wonSingleRate = (double)result.WonSingle / result.Runs;
                var drawRate = (double)result.Draw / result.Runs;
                var loseRate = (double)(result.Runs - result.Won) / result.Runs;
                yield return new SimulationResult
                {
                    Card1 = cards[0],
                    Card2 = cards[1],
                    WonRate = wonRate,
                    WonSingleRate = wonSingleRate,
                    DrawRate = drawRate,
                    LoseRate = loseRate,
                    Runs = result.Runs,
                    ConfidenceLevel = options.ConfidenceLevel,
                    PlayerCount = options.PlayerCount,
                    WinLoseRatesRelativeError = options.WinLoseRatesDesiredRelativeError,
                    Balance = result.Balance / result.Runs
                };
            }
        }

        public class SimulationOptions
        {
            public double WinLoseRatesDesiredRelativeError { get; set; }

            public ConfidenceLevel ConfidenceLevel { get; set; }

            public int PlayerCount { get; set; }

            public double BalanceDesiredAbsoluteError { get; set; }
        }

        public class SimulationResult
        {
            [Index(0)]
            [TypeConverter(typeof(CardCsvTypeConverter))]
            public Card Card1 { get; set; }

            [Index(1)]
            [TypeConverter(typeof(CardCsvTypeConverter))]
            public Card Card2 { get; set; }

            [Index(2)]
            public double WonRate { get; set; }

            [Index(3)]
            public double WonSingleRate { get; set; }

            [Index(4)]
            public double DrawRate { get; set; }

            [Index(5)]
            public double LoseRate { get; set; }

            [Index(6)]
            public int Runs { get; set; }

            [Index(7)]
            public double WinLoseRatesRelativeError { get; set; }

            [Index(8)]
            public double BalanceAbsoluteError { get; set; }

            [Index(9)]
            public ConfidenceLevel ConfidenceLevel { get; set; }

            [Index(10)]
            public int PlayerCount { get; set; }

            [Index(11)]
            public decimal Balance { get; set; }
        }

        public class IntermediateSimulationResult
        {
            public int Won { get; set; }

            public int WonSingle { get; set; }

            public decimal Balance { get; set; }

            public int Draw { get; set; }

            public int Runs { get; set; }
        }

        private IntermediateSimulationResult ComputeChances(SimulationOptions options, IList<Card> cards)
        {
            var runner = new BenchmarkRunner();
            var result = runner.Run(new BenchmarkOptions<IntermediateSimulationResult>
            {
                BatchSize = 16,
                Seed = new IntermediateSimulationResult { Draw = 0, Runs = 0, Won = 0, WonSingle = 0, Balance = 0 },
                RunOnce = () =>
                {
                    var startingPot = 1;
                    var playerIndex = 0;
                    var players = MakePlayers(options, startingPot);
                    var session = new PokerSession(players);
                    session.CheatPlayerCards(session.Players[playerIndex], cards);
                    session.Simulate();

                    var won = session.Winners.Contains(session.Players[playerIndex]);
                    var wonSingle = won && session.Winners.Count == 1;
                    var balance = -startingPot + session.Players[0].PlayerPot;

                    return new IntermediateSimulationResult
                    {
                        Draw = (session.Winners.Count == session.Players.Count) ? 1 : 0,
                        WonSingle = (won && session.Winners.Count == 1) ? 1 : 0,
                        Won = won ? 1 : 0,
                        Balance = balance,
                        Runs = 1
                    };
                },
                Combine = (a, b) =>
                {
                    return new IntermediateSimulationResult
                    {
                        Won = (a.Won + b.Won),
                        WonSingle = (a.WonSingle + b.WonSingle),
                        Draw = (a.Draw + b.Draw),
                        Balance = (a.Balance + b.Balance),
                        Runs = (a.Runs + b.Runs)
                    };
                },
                QuantifiedValues = new List<QuantifiedValueOptions<IntermediateSimulationResult>> {
                    new QuantifiedValueOptions<IntermediateSimulationResult> {
                        GetQuantifiedValue = a => (double)a.Won / a.Runs,
                        ConfidenceLevel = options.ConfidenceLevel,
                        DesiredRelativeError = options.WinLoseRatesDesiredRelativeError
                    },
                    new QuantifiedValueOptions<IntermediateSimulationResult> {
                        GetQuantifiedValue = a => (double)a.WonSingle / a.Runs,
                        ConfidenceLevel = options.ConfidenceLevel,
                        DesiredRelativeError = options.WinLoseRatesDesiredRelativeError
                    },
                    new QuantifiedValueOptions<IntermediateSimulationResult> {
                        GetQuantifiedValue = a => (double)(a.Runs - a.Won) / a.Runs,
                        ConfidenceLevel = options.ConfidenceLevel,
                        DesiredRelativeError = options.WinLoseRatesDesiredRelativeError
                    },
                    new QuantifiedValueOptions<IntermediateSimulationResult>{
                        GetQuantifiedValue = a => (double)a.Balance / a.Runs,
                        ConfidenceLevel = options.ConfidenceLevel,
                        DesiredAbsoluteError = options.BalanceDesiredAbsoluteError
                    }
                }
            });
            return result;
        }

        protected virtual List<Player> MakePlayers(SimulationOptions options, int startingPot)
        {
            return Enumerable
                    .Range(0, options.PlayerCount)
                    .Select(i => new Player()
                    {
                        PlayerPot = startingPot,
                        Strategy = new AllInStrategy()
                    }).ToList();
        }

        private static IEnumerable<IList<Card>> GeneratePairs(IEnumerable<Card> cards)
        {
            return cards
                .SelectMany((card1, index1) => cards
                    .Where((card2, index2) => index2 > index1)
                    .Select(card2 => new[] { card1, card2 }));
        }

        private static IEnumerable<Card> GenerateCards()
        {
            var suits = new[] { Suit.Clovers, Suit.Hearts };
            var cardValues = EnumUtil.GetValues<CardValue>().Reverse();
            var cards = suits.SelectMany(suit => cardValues.Select(cardValue => new Card(cardValue, suit)));
            return cards;
        }
    }
}
