namespace AlgorithmAnalysis.Common.Files
{
    public interface IPathCreator
    {
        string CreateSpecificPath(string specificValue, string? path, bool appendAdppFolder);

        string CreateSpecificPath(string specificValue, bool appendAdppFolder);
    }
}
