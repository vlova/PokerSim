using PokerSim.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{

    public abstract class CardCombination : IComparable<CardCombination>
    {
        public IReadOnlyList<Card> Cards { get; }

        public CardCombination(IEnumerable<Card> cards)
        {
            this.Cards = cards.ToList();
        }

        public abstract CardCombinationKind Kind { get; }

        public abstract int CompareTo(CardCombination other);
    }

    abstract class CardCombination<T> : CardCombination, IComparable<CardCombination> where T : CardCombination<T>
    {
        public CardCombination(IEnumerable<Card> cards) : base(cards) { }

        public override int CompareTo(CardCombination other)
        {
            if (this.Kind < other.Kind)
            {
                return -1;
            }

            if (this.Kind > other.Kind)
            {
                return 1;
            }

            if (other is T)
            {
                return this.CompareCombination(other as T);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected abstract int CompareCombination(T other);
    }
}
