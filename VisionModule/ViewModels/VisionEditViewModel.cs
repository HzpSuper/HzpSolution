using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisionModule.ViewModels
{
    public class VisionEditViewModel : BindableBase
    {
        private string _title = "视觉编辑界面";
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

        public VisionEditViewModel()
        {
            Message = "视觉编辑界面 from your Prism Module";
        }
    }


}
