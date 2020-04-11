using AlgorithmAnalysis.Models;

namespace AlgorithmAnalysis.Configuration
{
    public sealed class AppearanceOptions : IOptions
    {
        public ThemeKind Theme { get; set; } = ThemeKind.Light;


        public AppearanceOptions()
        {
        }
    }
}
