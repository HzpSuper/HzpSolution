using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace HzpSolution.MessageBoxExtend
{
    public class MessageBoxDialogModel : ViewModelBase
    {
       #pragma warning disable CS8618
        public MessageBoxDialogModel(string? message,MessageBoxButtonE messageBoxButton = MessageBoxButtonE.OK,MessageBoxImageE messageBoxImage= MessageBoxImageE.Information)
        {
           switch(messageBoxButton)
           {
                case MessageBoxButtonE.OK:
                    ListButton = new(){ new(){ Content = "确认", CommandParameter = MessageBoxResultE.Yes ,IsDefault =true}};
                    break;
                case MessageBoxButtonE.OKCancel:
                    ListButton = new(){ new(){ Content = "确认", CommandParameter = MessageBoxResultE.Yes ,IsDefault = true }, new(){ Content = "取消",CommandParameter = MessageBoxResultE.Cancel,IsCancel = true } };
                    break;
                case MessageBoxButtonE.YesNoCancel:
                    ListButton = new() { new(){ Content = "是", CommandParameter = MessageBoxResultE.Yes },  new(){ Content = "否", CommandParameter = MessageBoxResultE.No }, new() { Content = "取消", CommandParameter = MessageBoxResultE.Cancel ,IsCancel = true} };
                    break;
            }

            switch(messageBoxImage)
            {
                case MessageBoxImageE.Information:
                    _icon = "InformationOutline";
                    _title = "提示";
                    break;
                case MessageBoxImageE.Warning:
                    _icon = "AlertCircleOutline";
                    _title = "警告";
                    break;
                case MessageBoxImageE.Error:
                    _icon = "CloseCircleOutline";
                    _title = "错误";
                    break;
            }
            _message = message;
        }
       #pragma warning restore CS8618

        private string? _title;
        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string? _message;
        public string? Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string? _icon;
        public string? Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public ObservableCollection<Button> ListButton { get; }
    }
}
