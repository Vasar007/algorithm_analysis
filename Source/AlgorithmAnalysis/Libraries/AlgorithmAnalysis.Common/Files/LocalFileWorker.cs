using System;
using System.IO;
using Acolyte.Assertions;
using FileHelpers;

namespace AlgorithmAnalysis.Common.Files
{
    public sealed class LocalFileWorker
    {
        public LocalFileWorker()
        {
        }

        public DataObject<OutputFileData> ReadDataFile(string dataFilename)
        {
            dataFilename.ThrowIfNullOrWhiteSpace(nameof(dataFilename));

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
