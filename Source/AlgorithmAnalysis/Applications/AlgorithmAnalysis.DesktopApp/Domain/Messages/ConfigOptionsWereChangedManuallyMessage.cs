using Prism.Events;

namespace AlgorithmAnalysis.DesktopApp.Domain.Messages
{
    internal sealed class ConfigOptionsWereChangedManuallyMessage : PubSubEvent
    {
        public ConfigOptionsWereChangedManuallyMessage()
        {
        }
    }
}
