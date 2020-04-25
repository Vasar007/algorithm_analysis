namespace AlgorithmAnalysis.Common.Files
{
    public sealed class PathResolutionOptions
    {
        public bool UnifyDirectorySeparatorChars { get; set; } = true;

        public bool UsePlatformIndependentDirectorySeparatorChar { get; set; } = true;

        public bool ReturnRelativePath { get; set; } = false;


        public PathResolutionOptions()
        {
        }
    }
}
