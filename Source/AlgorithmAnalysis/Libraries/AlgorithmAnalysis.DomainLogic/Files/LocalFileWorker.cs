using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acolyte.Assertions;
using FileHelpers;

namespace AlgorithmAnalysis.DomainLogic.Files
{
    internal sealed class LocalFileWorker
    {
        public LocalFileWorker()
        {
        }

        public IReadOnlyList<int> ReadDataFile(string dataFilename, ParametersPack args)
        {
            dataFilename.ThrowIfNullOrWhiteSpace(nameof(dataFilename));
            args.ThrowIfNull(nameof(args));

            if (!File.Exists(dataFilename))
            {
                throw new ArgumentException(
                    $"File '{dataFilename}' does not exist.", nameof(dataFilename)
                );
            }

            // Output data file contains exactly "LaunchesNumber" values.
            var result = new List<int>(args.LaunchesNumber);

            using var engine = new FileHelperAsyncEngine<OutputFileData>();
            using (engine.BeginReadFile(dataFilename))
            {
                // The engine is IEnumerable.
                result.AddRange(engine.Select(data => data.operationNumber));
            }
            return result.ToList();
        }
    }
}
