namespace AlgorithmAnalysis.Common.Files
{
    public sealed class PathCreationOptions
    {
        public bool AppendAppFolder { get; set; } = false;

        public bool ShouldResolvePath { get; set; } = false;


        public PathCreationOptions()
        {
        }
    }
}
