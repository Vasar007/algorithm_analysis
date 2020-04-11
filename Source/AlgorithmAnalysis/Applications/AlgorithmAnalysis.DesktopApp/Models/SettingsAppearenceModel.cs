using Prism.Mvvm;
using AlgorithmAnalysis.Configuration;
using AlgorithmAnalysis.DesktopApp.Domain;
using AlgorithmAnalysis.DesktopApp.Domain.MaterialDesign;

namespace AlgorithmAnalysis.DesktopApp.Models
{
    internal sealed class SettingsAppearenceModel : BindableBase, IChangeableModel
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
            Validate();
        }

        #region IChangeableModel Implementation

        public void Reset()
        {
            var appearenceOptions = ConfigOptions.Appearence;

            CurrentTheme = ThemeWrapper.Create(appearenceOptions.Theme);
            IsDark = CurrentTheme.IsDark;
        }

        public void Validate()
        {
            // Nothing to validate.
        }

        #endregion

        public void Set()
        {

        }
    }
}
