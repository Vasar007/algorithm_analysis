using System;
using System.IO;
using Acolyte.Assertions;
using FileHelpers;

namespace AlgorithmAnalysis.DomainLogic.Files
{
    internal sealed class LocalFileWorker
    {
        public LocalFileWorker()
        {
        }

        public DataObject<OutputFileData> ReadDataFile(string dataFilename, ParametersPack args)
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
            var engine = new FileHelperAsyncEngine<OutputFileData>();
            return DataObject.Create(engine, dataFilename);
        }
    }
}
