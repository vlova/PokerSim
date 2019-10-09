using PokerSim.Cards;
using PokerSim.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    public class PokerSession
    {
        public decimal CommunityPot { get; private set; }

        public Queue<Card> Deck { get; private set; }

        public List<Card> CommunityCards { get; private set; }

        public List<Player> Players { get; private set; }

        public List<Player> Winners { get; private set; }

        public PokerSession(List<Player> players)
        {
            this.Deck = new Queue<Card>(GetDeck().Shuffle());
            this.Players = players;
            this.CommunityCards = new List<Card>();
            this.CommunityPot = 0;
        }

        public void CheatPlayerCards(Player player, IEnumerable<Card> cards)
        {
            var deck = this.Deck.ToList();
            deck.RemoveAll(cards.Contains);
            player.Cards.AddRange(cards);
            this.Deck = new Queue<Card>(deck);
        }

        public void CheatCommunityCards(List<Card> cards)
        {
            var deck = this.Deck.ToList();
            deck.RemoveAll(cards.Contains);
            CommunityCards.AddRange(cards);
            this.Deck = new Queue<Card>(deck);
        }

        public void Simulate()
        {
            this.MakePlayersActive();
            this.DistributeCards();
            this.Trade();
            this.Flop();
            this.Turn();
            this.Turn();
            this.Showdown();
            this.DistributePot();
        }

        void MakePlayersActive()
        {
            foreach (var player in this.Players)
            {
                player.IsActive = true;
            }
        }

        void DistributeCards()
        {
            foreach (var player in this.Players)
            {
                while (player.Cards.Count < 2)
                {
                    player.Cards.Add(this.Deck.Dequeue());
                }
            }
        }

        void Trade()
        {
            while (true)
            {
                var seenCommunityPot = this.CommunityPot;
                foreach (var player in this.Players)
                {
                    if (!player.IsActive)
                    {
                        continue;
                    }

                    var action = player.Strategy.GetPlayerAction(new PlayerPokerState
                    {
                        PlayerCards = player.Cards,
                        CommunityCards = this.CommunityCards,
                        CommunityPot = this.CommunityPot,
                    });

                    HandlePlayerAction(player, action);
                }

                if (seenCommunityPot == this.CommunityPot)
                {
                    break;
                }
            }

            if (this.CommunityPot == 0)
            {
                throw new InvalidOperationException("Fuck, where are the bets?");
            }
        }

        void HandlePlayerAction(Player player, PlayerAction action)
        {
            if (action is NoneAction)
            {
                return;
            }

            if (action is FoldAction)
            {
                player.IsActive = false;
                return;
            }

            if (action is AllInAction)
            {
                var playerPot = player.PlayerPot;
                player.PlayerPot -= playerPot;
                this.CommunityPot += playerPot;
                return;
            }

            throw new NotImplementedException("Fuck OCP");
        }

        void Flop()
        {
            if (this.CommunityCards.Count >= 3)
            {
                return;
            }

            this.CommunityCards.Add(this.Deck.Dequeue());
            this.CommunityCards.Add(this.Deck.Dequeue());
            this.CommunityCards.Add(this.Deck.Dequeue());
        }

        void Turn()
        {
            if (this.CommunityCards.Count >= 5)
            {
                return;
            }

            this.CommunityCards.Add(this.Deck.Dequeue());
        }

        void Showdown()
        {
            var detector = new PokerHandDetector();
            List<Player> currentWinners = new List<Player>();
            PokerHand currentWinnerHand = null;
            foreach (var player in this.Players.Where(p => p.IsActive))
            {
                var cards = this.CommunityCards.Concat(player.Cards).ToHashSet();
                var playerTopHand = detector.DetectTopPokerHand(cards);
                if (currentWinnerHand == null || currentWinnerHand.CompareTo(playerTopHand) <= 0)
                {
                    if (currentWinnerHand != null && currentWinnerHand.CompareTo(playerTopHand) == 0)
                    {
                        currentWinners.Add(player);
                    }
                    else
                    {
                        currentWinners = new List<Player>() { player };
                    }
                    currentWinnerHand = playerTopHand;
                }
            }

            this.Winners = currentWinners;
        }

        private void DistributePot()
        {
            var winnerAmount = this.Winners.Count;
            var wonResult = this.CommunityPot / winnerAmount;
            foreach (var winner in this.Winners)
            {
                winner.PlayerPot += wonResult;
            }
        }

        static IEnumerable<Card> GetDeck()
        {
            foreach (var suit in EnumUtil.GetValues<Suit>())
            {
                foreach (var value in EnumUtil.GetValues<CardValue>())
                {
                    yield return new Card(value, suit);
                }
            }
        }
    }

}
