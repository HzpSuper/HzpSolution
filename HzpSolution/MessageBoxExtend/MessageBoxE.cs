using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace HzpSolution.MessageBoxExtend
{
    public static class MessageBoxE
    {
        public static async  Task<MessageBoxResultE?> ShowAsync(string? message, MessageBoxButtonE messageBoxButton = MessageBoxButtonE.OK, MessageBoxImageE messageBoxImage = MessageBoxImageE.Information,string dependcontainer = "RootDialog")
        {
            var messageBoxDialog = new MessageBoxDialog()
            {
                DataContext = new MessageBoxDialogModel(message, messageBoxButton, messageBoxImage)
            };
            return await DialogHost.Show(messageBoxDialog, dependcontainer) as MessageBoxResultE?;
        }
    }

    public enum MessageBoxButtonE
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 2
    }

    public enum MessageBoxImageE
    {
        Information = 0,
        Warning = 1,
        Error = 2
    }

    public enum MessageBoxResultE
    {
        Yes = 0,
        No = 1,
        Cancel = 2
    }

}
