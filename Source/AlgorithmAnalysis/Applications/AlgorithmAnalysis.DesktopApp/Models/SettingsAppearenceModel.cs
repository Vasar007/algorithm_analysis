using Prism.Mvvm;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.MaterialDesign;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAppearenceModel : BindableBase, IChangeable, ISaveable
    {
        // Initializes through Reset method in ctor.
        public ThemeWrapper CurrentTheme { get; private set; } = default!;

        private bool _isDark;
        public bool IsDark
        {
            get => _isDark;
            set
            {
                CurrentTheme = value ? ThemeWrapper.Dark : ThemeWrapper.Light;
                CurrentTheme.ApplyBaseTheme();
                SetProperty(ref _isDark, value);
            }
        }


        public SettingsAppearenceModel()
        {
            Reset();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            AppearanceOptions appearenceOptions = ConfigOptions.Appearence;

            CurrentTheme = ThemeWrapper.Create(appearenceOptions.Theme);
            IsDark = CurrentTheme.IsDark;
        }

        public void Validate()
        {
            // Nothing to validate.
        }

        #endregion

        #region ISaveable Implementation

        public void SaveToConfigFile()
        {
            Validate();

            AppearanceOptions appearenceOptions = ConfigOptions.Appearence;

            appearenceOptions.Theme = CurrentTheme.Kind;

            ConfigOptions.SetOptions(appearenceOptions);
        }

        #endregion
    }
}
