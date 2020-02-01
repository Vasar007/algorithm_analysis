using System.ComponentModel;

namespace AlgorithmAnalysis.DomainLogic
{
    public enum AlgorithmType
    {
        [Description("Pallottino's algorithm")]
        PallottinoAlgorithm = 0,

        [Description("Insertion sort")]
        InsertionSort = 1
    }
}
