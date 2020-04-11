using System;
using Acolyte.Assertions;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.Models;
using MaterialDesignThemes.Wpf;

namespace AlgorithmAnalysis.DesktopApp.Domain.MaterialDesign
{
    internal sealed class ThemeWrapper
    {
        private static readonly ILogger _logger = LoggerFactory.CreateLoggerFor<ThemeWrapper>();

        public static readonly ThemeWrapper Light = Create(ThemeKind.Light);

        public static readonly ThemeWrapper Dark = Create(ThemeKind.Dark);

        public IBaseTheme Value { get; }

        public string ThemeName { get; }

        public ThemeKind Kind { get; }

        public bool IsLight => Kind == ThemeKind.Light;

        public bool IsDark => Kind == ThemeKind.Dark;


        public ThemeWrapper(
            IBaseTheme value,
            string themeName,
            ThemeKind kind)
        {
            Value = value.ThrowIfNull(nameof(value));
            ThemeName = themeName.ThrowIfNull(nameof(themeName));
            Kind = kind.ThrowIfEnumValueIsUndefined(nameof(kind));
        }

        public static ThemeWrapper Create(ThemeKind themeKind)
        {
            themeKind.ThrowIfEnumValueIsUndefined(nameof(themeKind));

            return GetThemeByKind(themeKind);
        }

        public void ApplyBaseTheme()
        {
            _logger.Info($"Changing application theme. New theme: '{ThemeName}'.");

            ModifyTheme(theme => theme.SetBaseTheme(Value));
        }

        private static ThemeWrapper GetThemeByKind(ThemeKind themeKind)
        {
            return themeKind switch
            {
                ThemeKind.Light => new ThemeWrapper(
                                       Theme.Light, nameof(Theme.Light), ThemeKind.Light
                                   ),

                ThemeKind.Dark => new ThemeWrapper(
                                      Theme.Dark, nameof(Theme.Dark), ThemeKind.Dark
                                  ),

                _ => throw new ArgumentOutOfRangeException(
                         nameof(themeKind), themeKind,
                         $"Unknown theme kind: '{themeKind.ToString()}'."
                     )
            };
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
