using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class FoldBadCardsOrAllInStrategy : IPlayerStrategy
    {
        public PlayerAction GetPlayerAction(PlayerPokerState state)
        {
            if (!IsGoodStartingCards(state.PlayerCards))
            {
                return new FoldAction();
            }


            return new AllInAction();
        }

        private bool IsGoodStartingCards(IReadOnlyList<Card> playerCards)
        {
            if (playerCards[0].Value == playerCards[1].Value)
            {
                return true;
            }

            var sumOfValues = (int)playerCards[0].Value + (int)playerCards[1].Value;
            if (playerCards[0].Suit == playerCards[1].Suit)
            {
                var isGood = sumOfValues > 13;
                return isGood;
            }
            else
            {
                var isGood = sumOfValues > 18;
                return isGood;
            }
        }
    }
}
