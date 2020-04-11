using Prism.Events;
using AlgorithmAnalysis.DesktopApp.Models;

namespace AlgorithmAnalysis.DesktopApp.Domain.Messages
{
    internal sealed class SaveSettingsMessage : PubSubEvent<SettingsModel>
    {
        public SaveSettingsMessage()
        {
        }
    }
}
