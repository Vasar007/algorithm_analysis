using Prism.Events;

namespace AlgorithmAnalysis.DesktopApp.Domain.Messages
{
    internal sealed class ConfigOptionsWereChangedThroughSettingsMessage : PubSubEvent
    {
        public ConfigOptionsWereChangedThroughSettingsMessage()
        {
        }
    }
}
