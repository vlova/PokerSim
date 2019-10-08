using System;

namespace PokerSim
{
    class BenchmarkOptions<T>
    {
        public int BatchSize { get; set; }

        public T Seed { get; set; }

        public Func<T> RunOnce { get; set; }

        public Func<T, T, T> Combine { get; set; }

        public Func<T, double[]> GetQuantifiedValues { get; set; }

        public ConfidenceLevel ConfidenceLevel { get; set; }

        public double DesiredRelativeError { get; set; }
    }
}
