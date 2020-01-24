using AlgorithmAnalysis.DesktopApp.Domain;
using Prism.Mvvm;

namespace AlgorithmAnalysis.DesktopApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        public string Title { get; }


        public MainWindowViewModel()
        {
            Title = DesktopOptions.Title;
        }
    }
}
