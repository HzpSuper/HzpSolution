using EventAggregator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace MotionModule.ViewModels
{
    public class MotionMainViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;

        private string _title = "运动控制主界面";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _message = "Message to Send";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public DelegateCommand SendMessageCommand { get; private set; }

        public MotionMainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            SendMessageCommand = new DelegateCommand(SendMessage);
            Message = "运动控制主 from your Prism Module";
        }

        private void SendMessage()
        {
            _ea.GetEvent<MessageSentEvent>().Publish((Message,MessageLevel.Information));
        }


    }
}
