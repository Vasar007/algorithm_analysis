﻿namespace AlgorithmAnalysis.Common.Files
{
    public interface IPathResolver
    {
        string Resolve(string unresolvedPath, PathResolutionOptions? options);

        string Resolve(string unresolvedPath);
    }
}
