using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionModule.ViewModels
{
    public class VisionMainViewModel : BindableBase
    {
        private string _title = "视觉主界面";
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

        public VisionMainViewModel()
        {
            Message = "视觉主界面 from your Prism Module";
        }
    }
}
