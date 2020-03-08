using FileHelpers;

namespace AlgorithmAnalysis.Common.Files
{
    // TODO: refactor FieldOrderAttribute and allow to work with propertis and immutable models.
    /// <summary>
    /// Represents record which could be used by FileHelper reader.
    /// </summary>
    /// <remarks>
    /// FileHelper doesn't support properties. That's why this data class contains public field.
    /// </remarks>
    [DelimitedRecord("|"), IgnoreEmptyLines(true), IgnoreFirst(1)]
    public sealed class OutputFileData
    {
        /// <summary>
        /// Output file contains single integer value which means operation number of algorithm
        /// launch.
        /// </summary>
        [FieldOrder(1)]
        public int operationNumber = default;


        /// <summary>
        /// Creates instance with default values.
        /// </summary>
        public OutputFileData()
        {
        }
    }
}
