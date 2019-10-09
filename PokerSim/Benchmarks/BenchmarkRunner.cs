using MathNet.Numerics.Statistics;
using PokerSim.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class BenchmarkRunner
    {
        public T Run<T>(BenchmarkOptions<T> options)
        {
            var batchAmounts = 32;
            var batchSize = options.BatchSize;

            List<T> batchRunResults = new List<T> { };
            while (true)
            {
                // TODO: maybe it's better to increase batchAmount * 2/3 at each iteration - this can help parallelization
                // TODO: maybe it's better to increase batchSize at each iteration + merge previous batches - this can reduce overall memory usage
                batchRunResults.AddRange(
                    ParallelEnumerable.Range(0, batchAmounts)
                    .Select(_ => RunOnce(options, batchSize)));

                if (!IsGoodEnough(options, batchRunResults))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine($"Total runs: {batchRunResults.Count * batchSize}");
            return batchRunResults.Aggregate(options.Combine);
        }

        private static T RunOnce<T>(BenchmarkOptions<T> options, int runs)
        {
            return ParallelEnumerable.Range(0, runs)
                .Select(_ => options.RunOnce())
                .Aggregate(options.Seed, options.Combine);
        }

        static Dictionary<ConfidenceLevel, double> ConfidenceToFactor = new Dictionary<ConfidenceLevel, double> {
            { ConfidenceLevel.L95, 1.96},
            { ConfidenceLevel.L99, 2.576}
        };

        private static bool IsGoodEnough<T>(BenchmarkOptions<T> options, List<T> batchRunResults)
        {
            return options.QuantifiedValues.All(valueOptions => IsGoodEnough(valueOptions, batchRunResults));
        }

        private static bool IsGoodEnough<T>(QuantifiedValueOptions<T> valueOptions, List<T> batchRunResults)
        {
            var stats = batchRunResults.Select(runResult => valueOptions.GetQuantifiedValue(runResult)).ToList();

            var isNiceRelativeError
                = !valueOptions.DesiredRelativeError.HasValue
                || RelativeError(valueOptions, stats) <= valueOptions.DesiredRelativeError;

            var isNiceAbsoluteError
                = !valueOptions.DesiredAbsoluteError.HasValue
                || AbsoluteError(valueOptions, stats) <= valueOptions.DesiredAbsoluteError;

            return isNiceRelativeError && isNiceAbsoluteError;
        }

        private static double RelativeError<T>(QuantifiedValueOptions<T> options, List<double> stats)
        {
            var (mean, deviation) = Statistics.MeanStandardDeviation(stats);
            var sem = deviation / Math.Sqrt(stats.Count);
            var marginOfErrorInPercents = sem * ConfidenceToFactor[options.ConfidenceLevel] / mean;
            return marginOfErrorInPercents;
        }

        private static double AbsoluteError<T>(QuantifiedValueOptions<T> options, List<double> stats)
        {
            var deviation = Statistics.StandardDeviation(stats);
            var sem = deviation / Math.Sqrt(stats.Count);
            var marginOfError = sem * ConfidenceToFactor[options.ConfidenceLevel];
            return marginOfError;
        }
    }
}
