using EventAggregator;
using HzpSolution.Common;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using System.Windows;

namespace HzpSolution
{
    public  class MessageData: ViewModelBase
    {
        private bool _isSelected;
        public  bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private Visibility _visible;
        public Visibility Visible
        {
            get => _visible;
            set => SetProperty(ref _visible, value);
        }

        private string? _name;
        public  string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int? _numeric;
        public  int? Numeric
        {
            get => _numeric;
            set => SetProperty(ref _numeric, value);
        }

        private MessageLevel _messagelevel;
        public  MessageLevel Messagelevel
        {
            get => _messagelevel;
            set => SetProperty(ref _messagelevel, value);
        }

    }   
}