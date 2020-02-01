﻿using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgorithmAnalysis.DesktopApp.Domain.Commands
{
    internal interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);

        bool CanExecute(T parameter);
    }
}
