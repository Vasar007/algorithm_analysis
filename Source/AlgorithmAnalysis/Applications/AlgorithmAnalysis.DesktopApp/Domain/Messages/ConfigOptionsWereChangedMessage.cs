using Prism.Events;

namespace AlgorithmAnalysis.DesktopApp.Domain.Messages
{
    internal sealed class ConfigOptionsWereChangedMessage : PubSubEvent
    {
        public ConfigOptionsWereChangedMessage()
        {
        }
    }
}
