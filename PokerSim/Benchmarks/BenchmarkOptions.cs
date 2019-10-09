using System;
using System.Collections.Generic;

namespace PokerSim
{
    class BenchmarkOptions<T>
    {
        public int BatchSize { get; set; }

        public T Seed { get; set; }

        public Func<T> RunOnce { get; set; }

        public Func<T, T, T> Combine { get; set; }

        public List<QuantifiedValueOptions<T>> QuantifiedValues { get; set; }
    }
}
