using Prism.Events;
using System;

namespace EventAggregator
{
    public class MessageSentEvent : PubSubEvent<(string messagecontent, MessageLevel messageLevel)>
    {

    }

    [Flags]
    public enum MessageLevel
    {
        Verbose = 1 << 0,
        Information = 1 << 1,
        Warning = 1 << 2,
        Error = 1 << 3,
        Fatal = 1 << 4,
        Debug = 1 << 5
    }
}
