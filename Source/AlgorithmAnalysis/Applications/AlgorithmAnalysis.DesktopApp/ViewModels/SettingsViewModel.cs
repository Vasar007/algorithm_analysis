using System;
using System.Windows.Input;
using Acolyte.Assertions;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.DesktopApp.Models;
using AlgorithmAnalysis.DesktopApp.Domain.MaterialDesign;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class SettingsViewModel : BindableBase
    {
        // TODO: allow to save into config all setting values.

        private static readonly ILogger _logger =
            LoggerFactory.CreateLoggerFor<SettingsViewModel>();

        public SettingsModel Settings { get; }

        public ICommand ToggleBaseCommand { get; }


        public SettingsViewModel()
        {
            Settings = new SettingsModel();

            ToggleBaseCommand = new DelegateCommand<bool?>(ApplyTheme);

            ApplyThemeFromSettings();
        }

        private void ApplyThemeFromSettings()
        {
            ThemeWrapper newTheme = Settings.Appearence.CurrentTheme;
            ApplyBase(newTheme);
        }

        private void ApplyTheme(bool? isDark)
        {
            if (!isDark.HasValue)
            {
                throw new ArgumentException("Boolean flag should be specified.", nameof(isDark));
            }

            ThemeWrapper newTheme = Settings.Appearence.GetTheme(isDark.Value);
            ApplyBase(newTheme);
        }

        private void ApplyBase(ThemeWrapper newTheme)
        {
            newTheme.ThrowIfNull(nameof(newTheme));

            _logger.Info($"Changing application theme. New theme: '{newTheme.ThemeName}'.");

            ModifyTheme(theme => theme.SetBaseTheme(newTheme.Value));
        }

        private static void ModifyTheme(Action<ITheme>? modificationAction)
        {
            _logger.Info("Modifying application theme.");

            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}
