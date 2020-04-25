﻿using System.Windows;

namespace AlgorithmAnalysis.DesktopApp.Domain.Commands
{
    internal static class ApplicationCloseCommand
    {
        public static bool CanExecute()
        {
            return !(Application.Current is null) && !(Application.Current.MainWindow is null);
        }

        public static void Execute()
        {
            Application.Current.MainWindow.Close();

            // If no need to handle closing event, use this:
            // Application.Current.Shutdown();
        }
    }
}
