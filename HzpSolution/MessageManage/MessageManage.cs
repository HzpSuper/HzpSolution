using Config.Net;
using EventAggregator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HzpSolution
{
    public class MessageManage
    {
        private readonly ObservableCollection<MessageData> _messageDatas;

        private readonly ObservableCollection<MessageShow> _messageDatasShow;

        private readonly BlockingCollection<MessageShow> _bcmessageShow = new();

        private readonly Dictionary<MessageLevel, ConcurrentQueue<MessageShow>> _dictmessageDatasShow = new();

        private readonly MessageLogSettings _ml = MessageLogSettings.Instance;

        private MessageLevel? _currentMessageLevel;

        private int _currentshowmessagecount = 100;

        private int _currentshowtype = 0;

        public MessageManage(ObservableCollection<MessageData> messageDatas, ObservableCollection<MessageShow> messageDatasShow)
        {
            _messageDatas = messageDatas;
            _messageDatasShow = messageDatasShow;

            foreach (int ml in Enum.GetValues(typeof(MessageLevel)))
            {
                _dictmessageDatasShow.Add((MessageLevel)ml, new());
            }

            foreach (KeyValuePair<MessageLevel, (string name, bool isselected)> kv in _ml.GetMessageLevels())
            {
                _messageDatas.Add(new() { Name = kv.Value.name, Numeric = null,Visible = kv.Value.isselected ? Visibility.Visible : Visibility.Collapsed,Messagelevel = kv.Key });
            }

            //if(_messageDatas.Count == 0)
            //{
            //    _currentMessageLevel = MessageLevel.Information;
            //    MessageData md = _messageDatas.First(x => x.Messagelevel == MessageLevel.Information);
            //    md.IsSelected = true;
            //    md.Visible = Visibility.Visible;
            //}
            //else
            //{
                _currentMessageLevel = _messageDatas.Where(x => x.Visible == Visibility.Visible).FirstOrDefault(y => y.IsSelected = true)?.Messagelevel;
           // }
            _currentshowmessagecount = _ml.Imessagelogsettings.ShowMessageCount;
            _currentshowtype = _ml.Imessagelogsettings.ShowType;
            _ml.Imessagelogsettings.PropertyChanged += MessageLogPropertyChanged;
            AsyncMessageDisplay();
        }

        public void MessageSwitchover(MessageLevel? messageLevel)
        {
            if (_currentMessageLevel == messageLevel || messageLevel == null)
            {
                return;
            }
            _currentMessageLevel = (MessageLevel)messageLevel;
            _messageDatas.Where(x => x.Messagelevel != _currentMessageLevel).ToList().ForEach(x => x.IsSelected = false);
            MessageData d = _messageDatas.First(x => x.Messagelevel == _currentMessageLevel);
            d.IsSelected = true;
            d.Numeric = null;
            _messageDatasShow.Clear();

            MessageShow[] ms = _dictmessageDatasShow[(MessageLevel)_currentMessageLevel].ToArray();
            if (_currentshowmessagecount < ms.Length)
            {
                Range phrase = (ms.Length - _currentshowmessagecount)..ms.Length;
                ms = ms[phrase];
            }
            foreach (MessageShow m in ms)
            {
                _bcmessageShow.Add(m);
            }
        }

        private async void AsyncMessageDisplay()
        {
            while (true)
            {
                MessageShow data = await Task.Run(() =>
                {
                    return _bcmessageShow.Take();
                });
                int count = _messageDatasShow.Count;
                if (data.Messagelevel == _currentMessageLevel)
                {
                    _messageDatasShow.Insert(0, new() { MessageTime = data.MessageTime, MessageContext = data.MessageContext });
                    int removecount = count - _currentshowmessagecount;
                    if (removecount > 0)
                    {
                        
                        for (int i = 1; i < removecount + 1; i++)
                        {
                            _messageDatasShow.RemoveAt(count - i);
                        }
                    }
                }
                else
                {
                    MessageData md = _messageDatas.First(x => x.Messagelevel == data.Messagelevel);
                    if (md.Numeric == null)
                    {
                        md.Numeric = 1;
                    }
                    else if (md.Numeric < _currentshowmessagecount)
                    {
                        md.Numeric++;
                    }
                    else
                    {
                        md.Numeric = _currentshowmessagecount;
                    }
                }
            }
        }

        public void MessageReceived(string messagecontent, MessageLevel messageLevel)
        {
            DateTime now = DateTime.Now;
            _dictmessageDatasShow[messageLevel].Enqueue(new() { MessageTime = now, MessageContext = messagecontent, Messagelevel = messageLevel });
            if (_dictmessageDatasShow[messageLevel].Count > _ml.Imessagelogsettings.MaxMessageCount)
            {
                _ = _dictmessageDatasShow[messageLevel].TryDequeue(out _);
            }
            _bcmessageShow.Add(new() { MessageTime = now, MessageContext = messagecontent, Messagelevel = messageLevel });
        }

        private void MessageLogPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowMessageCount" && _ml.Imessagelogsettings.ShowMessageCount != _currentshowmessagecount)
            {
                _currentshowmessagecount = _ml.Imessagelogsettings.ShowMessageCount;
                foreach(MessageData md in _messageDatas)
                {
                    if (md.Numeric > _currentshowmessagecount)
                    {
                        md.Numeric = _currentshowmessagecount;
                    }
                }
            }

            if (e.PropertyName == "ShowType" && _ml.Imessagelogsettings.ShowType != _currentshowtype)
            {
                _currentshowtype = _ml.Imessagelogsettings.ShowType;
                Dictionary<MessageLevel, bool>? dictshowtype = _ml.GetMessageLevels().Where(x => x.Value.isshow).ToDictionary(key => key.Key, value => value.Value.isshow);
                if (dictshowtype.Count == 0)
                {
                    _currentMessageLevel = null;
                    foreach (MessageData md in _messageDatas)
                    {
                        md.Visible = Visibility.Collapsed;
                    }
                }
                else
                {
                    List<MessageLevel> listml = dictshowtype.Keys.ToList();
                    foreach (MessageData md in _messageDatas)
                    {
                        md.Visible = listml.Contains(md.Messagelevel) ? Visibility.Visible : Visibility.Collapsed;
                    }
                    if (_currentMessageLevel == null || !listml.Contains((MessageLevel)_currentMessageLevel))
                    {
                        MessageSwitchover(listml.First());
                    }
                }

            }
        }

    }

    public class MessageShow
    {
       public DateTime MessageTime { get; set; }

       public string? MessageContext { get; set; }

       public MessageLevel Messagelevel { get; set; } = MessageLevel.Information;
    }
}



