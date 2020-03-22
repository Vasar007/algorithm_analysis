using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using AlgorithmAnalysis.DesktopApp.Views;
using AlgorithmAnalysis.Logging;
using AlgorithmAnalysis.Common;
using AlgorithmAnalysis.Configuration;

namespace AlgorithmAnalysis.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private static readonly ILogger _logger = LoggerFactory.CreateLoggerFor<App>();

        private static readonly string TraceLogFilename = LogHelper.CreateLogFilename("trace");


        public App()
        {
            // Set current culture for app globally.
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.InvariantCulture.Name)
                )
            );

            string logFilePath = LogHelper.GetLogFilePath(ConfigOptions.Logger, TraceLogFilename);
            TraceHelper.SetTraceListener(logFilePath, "TextLogTraceListener");

            _logger.PrintHeader("Desktop client application started.");
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // TODO: register common domain and models classes here.
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // TODO: split desktop project on separate modules.
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _logger.PrintFooter("Desktop client application stopped.");
        }
    }
}
