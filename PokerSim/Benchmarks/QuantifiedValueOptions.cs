using System;

namespace PokerSim
{
    class QuantifiedValueOptions<T>
    {
        public Func<T, double> GetQuantifiedValue { get; set; }

        public ConfidenceLevel ConfidenceLevel { get; set; }

        public double? DesiredRelativeError { get; set; }

        public double? DesiredAbsoluteError { get; set; }
    }
}
