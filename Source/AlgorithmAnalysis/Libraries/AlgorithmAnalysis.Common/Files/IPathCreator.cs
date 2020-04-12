namespace AlgorithmAnalysis.Common.Files
{
    public interface IPathCreator
    {
        string CreateSpecificPath(string specificValue, string? additionalPath,
            PathCreationOptions? options);

        string CreateSpecificPath(string specificValue, PathCreationOptions? options);

        string CreateSpecificPath(string specificValue);
    }
}
