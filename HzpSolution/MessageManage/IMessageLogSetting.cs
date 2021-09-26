using Config.Net;
using EventAggregator;
using HzpSolution.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzpSolution
{
    public interface IMessageLogSettings : INotifyPropertyChanged
    {
        [DefaultValue(200)]
        public int MaxMessageCount { get; set; }

        [DefaultValue(5)]
        public int MinMessageCount { get; set; }

        [DefaultValue(50)]
        public int ShowMessageCount { get; set; }

        [DefaultValue(1)]
        public int ShowType { get; set; }

        public string? SavePath { get; set; }
       
        [DefaultValue(31)]
        public int SaveDays { get; set; }

    }

    public sealed class MessageLogSettings
    {
        private static readonly Lazy<MessageLogSettings> lazy = new(() => new MessageLogSettings());

        public static MessageLogSettings Instance => lazy.Value;

        private MessageLogSettings() { Imessagelogsettings = new ConfigurationBuilder<IMessageLogSettings>().UseJsonFile(Constants.Configs + "MessageLog.json").Build(); }

        public IMessageLogSettings Imessagelogsettings { get; set; }

        public Dictionary<MessageLevel, (string name, bool isshow)> GetMessageLevels()
        {
            MessageLevel stype = (MessageLevel)Imessagelogsettings.ShowType;
            Dictionary<MessageLevel, (string name, bool isshow)> dict_messageLevels = new();
            foreach (int i in Enum.GetValues(typeof(MessageLevel)))
            {
                MessageLevel ml = (MessageLevel)i;
                if (stype.HasFlag(ml))
                {
                    dict_messageLevels.Add(ml, (keyValuePairs[ml], true));
                }
                else
                {
                    dict_messageLevels.Add(ml, (keyValuePairs[ml], false));
                }
            }
            return dict_messageLevels;
        }

        public void SetMessageLevels(IEnumerable<bool>  enumerator)
        {
            int result = 0;
            int num = 0;
            foreach (bool v in enumerator)
            {
                if (v)
                {
                    result += (int)Math.Pow(2, num);
                }
                num++;
            }
            Imessagelogsettings.ShowType = result;
        }

        private readonly Dictionary<MessageLevel, string> keyValuePairs = new()
        {
            { MessageLevel.Verbose,"冗余" },
            { MessageLevel.Information,"消息"},
            { MessageLevel.Warning, "警告" },
            { MessageLevel.Error, "错误" },
            { MessageLevel.Fatal, "致命" },
            { MessageLevel.Debug, "调试" }
        };
    }
}