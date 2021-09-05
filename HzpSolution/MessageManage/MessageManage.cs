using EventAggregator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzpSolution
{
    public class MessageManage
    {
        private readonly ObservableCollection<MessageData> _messageDatas;

        private readonly ObservableCollection<MessageShow> _messageDatasShow;

        private BlockingCollection<MessageShow> bcmessageShow = new();

        private MessageLevel? _currentMessageLevel;

        private int _currentMaxMessageCount;

        public MessageManage(ObservableCollection<MessageData> messageDatas, ObservableCollection<MessageShow> messageDatasShow)
        {
            _messageDatas = messageDatas;
            _messageDatasShow = messageDatasShow;

            Task.Factory.StartNew(()=>MessageDisplay());

            Task.Factory.StartNew(() => bcmessageShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈" }));
            Task.Factory.StartNew(() => bcmessageShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈1" }));
            Task.Factory.StartNew(() => bcmessageShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈2" }));
            Task.Factory.StartNew(() => bcmessageShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈3" }));
            Task.Factory.StartNew(() => bcmessageShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈4" }));

        }

        public void MessageSwitchover(MessageLevel? messageLevel)
        {
            if (_currentMessageLevel == messageLevel)
            {
                return;
            }
            _currentMessageLevel = messageLevel;
            _messageDatas.Where(x => x.Messagelevel != _currentMessageLevel).ToList().ForEach(x => x.IsSelected = false);
            var d = _messageDatas.Where(x => x.Messagelevel == _currentMessageLevel).First(y => y.IsSelected = true);

            _currentMaxMessageCount = d.MaxMessageCount;
        }

        private void MessageDisplay()
        {
            while (!bcmessageShow.IsCompleted)
            {
                MessageShow data = bcmessageShow.Take();
                _messageDatasShow.Add(new() { MessageTime = data.MessageTime, MessageContext = data.MessageContext });
            }
        }


    }
}
