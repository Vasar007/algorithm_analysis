using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgorithmAnalysis.DesktopApp.Domain.Commands
{
    internal interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();

        bool CanExecute();
    }
}
