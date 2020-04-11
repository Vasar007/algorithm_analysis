using Prism.Events;
using AlgorithmAnalysis.DesktopApp.Models;

namespace AlgorithmAnalysis.DesktopApp.Domain.Messages
{
    internal sealed class ResetSettingsMessage : PubSubEvent
    {
        public ResetSettingsMessage()
        {
        }
    }
}
