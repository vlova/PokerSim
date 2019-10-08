﻿using CsvHelper.Configuration.Attributes;
using PokerSim.Cards;
using PokerSim.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    RelativeError = options.DesiredRelativeError,
                    Balance = result.Balance / result.Runs
                };
            }
        }

        public class SimulationOptions
        {
            public double DesiredRelativeError { get; set; }

            public ConfidenceLevel ConfidenceLevel { get; set; }

            public int PlayerCount { get; set; }
        }

        public class SimulationResult
        {
            [Index(0)]
            [CsvHelper.Configuration.Attributes.TypeConverter(typeof(CardCsvTypeConverter))]
            public Card Card1 { get; set; }

            [Index(1)]
            [CsvHelper.Configuration.Attributes.TypeConverter(typeof(CardCsvTypeConverter))]
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
            public double RelativeError { get; set; }

            [Index(8)]
            public ConfidenceLevel ConfidenceLevel { get; set; }

            [Index(9)]
            public int PlayerCount { get; set; }

            [Index(9)]
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

        private static IntermediateSimulationResult ComputeChances(SimulationOptions options, IList<Card> cards)
        {
            var runner = new BenchmarkRunner();
            var result = runner.Run(new BenchmarkOptions<IntermediateSimulationResult>
            {
                BatchSize = 16,
                Seed = new IntermediateSimulationResult { Draw = 0, Runs = 0, Won = 0, WonSingle = 0, Balance = 0 },
                RunOnce = () =>
                {
                    var playerIndex = 0;
                    var session = new PokerSession(amountOfPlayers: options.PlayerCount);
                    session.CheatPlayerCards(session.Players[playerIndex], cards);
                    session.Simulate();

                    var won = session.Winners.Contains(session.Players[playerIndex]);
                    var wonSingle = won && session.Winners.Count == 1;
                    var baseBid = (decimal)1;
                    var balance = -baseBid + (
                        won
                            ? session.Players.Count * baseBid / session.Winners.Count
                            : 0
                        );

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
                GetQuantifiedValues = a => new[] {
                        (double)a.Won / a.Runs,
                        (double)a.WonSingle / a.Runs,
                        //(double)a.Balance / a.Runs,
                        //(double)a.draw / a.runs,
                        (double)(a.Runs - a.Won) / a.Runs
                    },
                ConfidenceLevel = options.ConfidenceLevel,
                DesiredRelativeError = options.DesiredRelativeError
            });
            return result;
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