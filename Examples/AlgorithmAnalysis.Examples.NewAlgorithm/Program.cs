using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace AlgorithmAnalysis.Examples.NewAlgorithm
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Module started.");
                ProcessArgs(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred:{Environment.NewLine}{ex}");
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine("Module closed.");
            }
        }

        private static void ProcessArgs(IReadOnlyList<string> args)
        {
            // args: <algorithm_type> <start_value> <end_value> <launches_number> <step> <output_filename_pattern>
            // Ignore <algorithm_type> because our module implements only one algorithm.

            int startValue = int.Parse(args[1]);
            int endValue = int.Parse(args[2]);
            int launchesNumber = int.Parse(args[3]);
            int step = int.Parse(args[4]);
            string outputFilenamePattern = args[5];

            // Iterate through segment
            for (int inputDataSize = startValue; inputDataSize <= endValue; inputDataSize += step)
            {
                Console.WriteLine($"Size: {inputDataSize.ToString()}");
                // Launch specified number of times algorithm and save operations numbers.
                var results = new List<int>();
                for (int launch = 1; launch <= launchesNumber; ++launch)
                {
                    int[] array = GenerateArray(inputDataSize);
                    int operationsNumber = BubbleSort(array);
                    results.Add(operationsNumber);

                    Console.WriteLine($"Execution: {launch.ToString()}; Operations number: {operationsNumber.ToString()}");
                }

                // Create filename and save all results on current iteration.
                string filename = $"{outputFilenamePattern}{inputDataSize.ToString()}.txt";
                SaveResults(results, filename);
            }
        }

        private static int[] GenerateArray(int arrayLength)
        {
            var random = new Random();
            return Enumerable.Range(1, arrayLength)
                .Select(i => random.Next())
                .ToArray();
        }

        private static int BubbleSort(int[] array)
        {
            int operationNumber = 0;
            for (int j = 0; j <= array.Length - 2; ++j)
            {
                for (int i = 0; i <= array.Length - 2; ++i)
                {
                    if (array[i] > array[i + 1])
                    {
                        int temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;

                        // Here we increment operations number.
                        ++operationNumber;
                    }
                }
            }

            return operationNumber;
        }

        private static void SaveResults(IReadOnlyList<int> results, string filename)
        {
            var stringBuilder = new StringBuilder(results.Count).AppendLine("Operations number");

            foreach (int result in results)
            {
                stringBuilder.AppendLine(result.ToString());
            }

            File.AppendAllText(filename, stringBuilder.ToString());
        }
    }
}
