using PokerSim.Cards;
using PokerSim.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class PokerSession
    {
        public Queue<Card> Deck { get; set; }

        public List<Card> CommunityCards { get; set; }

        public List<Player> Players { get; set; }

        public List<Player> Winners { get; set; }

        public PokerSession(int amountOfPlayers)
        {
            this.Deck = new Queue<Card>(GetDeck().Shuffle());
            this.Players = Enumerable.Range(0, amountOfPlayers).Select(i => new Player()).ToList();
            this.CommunityCards = new List<Card>();
        }

        public void CheatPlayerCards(Player player, IEnumerable<Card> cards)
        {
            var deck = this.Deck.ToList();
            deck.RemoveAll(cards.Contains);
            player.Cards.AddRange(cards);
            this.Deck = new Queue<Card>(deck);
        }

        public void CheatDeckCards(List<Card> cards)
        {
            var deck = this.Deck.ToList();
            deck.RemoveAll(cards.Contains);
            CommunityCards.AddRange(cards);
            this.Deck = new Queue<Card>(deck);
        }

        public void Simulate()
        {
            this.DistributeCards();
            this.Flop();
            this.Turn();
            this.Turn();
            this.Showdown();
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

        void Flop()
        {
            this.CommunityCards.Add(this.Deck.Dequeue());
            this.CommunityCards.Add(this.Deck.Dequeue());
            this.CommunityCards.Add(this.Deck.Dequeue());
        }

        void Turn()
        {
            this.CommunityCards.Add(this.Deck.Dequeue());
        }

        void Showdown()
        {
            var detector = new PokerHandDetector();
            List<Player> currentWinners = new List<Player>();
            PokerHand currentWinnerHand = null;
            foreach (var player in this.Players)
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
