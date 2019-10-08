using PokerSim.Simulations;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PokerSim
{

    class Program
    {
        static void Main(string[] args)
        {
            ReMakeRecords(2);
            ReMakeRecords(3);
            ReMakeRecords(4);
            ReMakeRecords(5);
            ReMakeRecords(6);
            ReMakeRecords(8);
            ReMakeRecords(9);
        }

        private static void ReMakeRecords(int playerCount)
        {
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"all-in-{playerCount}players.csv");
            Console.WriteLine(csvFilePath);

            using (var fileStream = new FileStream(csvFilePath, FileMode.Create))
            {
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    using (var csvWriter = new CsvHelper.CsvWriter(writer))
                    {
                        csvWriter.WriteHeader<AllInSimulation.SimulationResult>();
                        csvWriter.Flush();
                        writer.WriteLine();
                        writer.Flush();

                        var allInSimulation = new AllInSimulation();
                        var simulationResults = allInSimulation.Run(new AllInSimulation.SimulationOptions
                        {
                            ConfidenceLevel = ConfidenceLevel.L95,
                            DesiredRelativeError = 0.02,
                            PlayerCount = playerCount
                        });

                        var watch = Stopwatch.StartNew();
                        foreach (var result in simulationResults)
                        {
                            csvWriter.WriteRecord(result);
                            csvWriter.Flush();
                            writer.WriteLine();
                            writer.Flush();

                            Console.WriteLine($"Processed {result.Card1}, {result.Card2}. Elapsed totally {watch.Elapsed}, Time: {DateTime.Now}");
                        }
                    }
                }
            }
        }
    }
}
