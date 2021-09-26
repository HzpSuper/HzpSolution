using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HzpSolution.ViewModels
{
    public class MessageLogSetViewModel : BindableBase
    {
        private string _title = "消息日志";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private int _maxMessageCount;
        public int MaxMessageCount
        {
            get => _maxMessageCount;
            set => SetProperty(ref _maxMessageCount, value);
        }

        private int _minMessageCount;
        public int MinMessageCount
        {
            get => _minMessageCount;
            set => SetProperty(ref _minMessageCount, value);
        }


        private int? _showMessageCount;
        public  int? ShowMessageCount
        {
            get => _showMessageCount;
            set => SetProperty(ref _showMessageCount, value);
        }

        private ObservableCollection<bool> _selectedtype = new();
        public ObservableCollection<bool> Selectedtype
        {
            get { return _selectedtype; }
            set { SetProperty(ref _selectedtype, value); }
        }

        private ObservableCollection<string?> _name = new();
        public ObservableCollection<string?> Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string? _savePath;
        public  string? SavePath
        {
            get => _savePath;
            set => SetProperty(ref _savePath, value);
        }

        private int? _saveDays;
        public int? SaveDays
        {
            get => _saveDays;
            set => SetProperty(ref _saveDays, value);
        }


        public DelegateCommand UnloadedCommand => new(Unloaded);

        public DelegateCommand UpdateLogSavePathCommand => new(UpdateLogSavePath);

        public DelegateCommand ResetLogSavePathCommand => new(ResetLogSavePath);

        private readonly MessageLogSettings _ml = MessageLogSettings.Instance;

        public MessageLogSetViewModel()
        {
            MaxMessageCount = _ml.Imessagelogsettings.MaxMessageCount;
            MinMessageCount = _ml.Imessagelogsettings.MinMessageCount;
            ShowMessageCount = _ml.Imessagelogsettings.ShowMessageCount;
            _ml.GetMessageLevels().Values.ToList().ForEach(x => { Selectedtype.Add(x.isshow); Name.Add($" {x.name} "); });

            SaveDays = _ml.Imessagelogsettings.SaveDays;
            SavePath = System.IO.Directory.Exists(_ml.Imessagelogsettings.SavePath)
                ? _ml.Imessagelogsettings.SavePath
                : $@"{AppContext.BaseDirectory}Logs";
        }

        private void Unloaded()
        {
            if (ShowMessageCount != null && int.TryParse(ShowMessageCount.ToString(), out int showmessagecount))
            {
                _ml.Imessagelogsettings.ShowMessageCount = showmessagecount;
            }
            _ml.SetMessageLevels(Selectedtype);


            if (SaveDays != null && int.TryParse(SaveDays.ToString(), out int savedays))
            {
                _ml.Imessagelogsettings.SaveDays = savedays;
            }

            _ml.Imessagelogsettings.SavePath = System.IO.Directory.Exists(SavePath)
               ? SavePath
               : $@"{AppContext.BaseDirectory}Logs";
        }

        private void UpdateLogSavePath()
        {
            CommonOpenFileDialog dialog = new();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SavePath = dialog.FileName;
            }
        }

        private void ResetLogSavePath()
        {
            SavePath = $@"{AppContext.BaseDirectory}Logs";
        }


    }
}
