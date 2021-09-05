using Prism.Events;

namespace EventAggregator
{
    public class MessageSentEvent : PubSubEvent<(string messagecontent, MessageLevel messageLevel)>
    {

    }

    public enum MessageLevel
    {
        Information = 0,
        Warning = 1,
        Error = 2,
        Fatal = 3,
        Debug = 4
    }
}
