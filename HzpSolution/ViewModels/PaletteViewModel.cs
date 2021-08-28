using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzpSolution.ViewModels
{
    public class PaletteViewModel : BindableBase
    {
        private string _title = "调色板";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public PaletteViewModel()
        {
            _message = "调色板 from your Prism Module";
        }
    }

}
