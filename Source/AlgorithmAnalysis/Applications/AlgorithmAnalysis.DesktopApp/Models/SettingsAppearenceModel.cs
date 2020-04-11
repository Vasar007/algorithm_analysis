using Prism.Mvvm;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.MaterialDesign;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAppearenceModel : BindableBase, IChangeableModel
    {
        // Initializes through Reset method in ctor.
        public ThemeWrapper CurrentTheme { get; private set; } = default!;


        public SettingsAppearenceModel()
        {
            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            CurrentTheme = ThemeWrapper.CreateFromConfig();
        }

        public void Validate()
        {
            // Nothing to validate.
        }

        #endregion

        public ThemeWrapper GetTheme(bool isDark)
        {
            CurrentTheme = isDark ? ThemeWrapper.Dark : ThemeWrapper.Light;
            return CurrentTheme;
        }
    }
}
