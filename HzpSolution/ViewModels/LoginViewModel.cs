using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HzpSolution.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private string? _userName;
        public string? UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private ObservableCollection<string> _users = new();
        public ObservableCollection<string> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private string? _password;
        public string? Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string? _newpassword;
        public string? Newpassword
        {
            get { return _newpassword; }
            set { SetProperty(ref _newpassword, value); }
        }

        private string? _confirmpassword;
        public string? Confirmpassword
        {
            get { return _confirmpassword; }
            set { SetProperty(ref _confirmpassword, value); }
        }

        private bool _rememberpassword;
        public  bool Rememberpassword
        {
            get { return _rememberpassword; }
            set { SetProperty(ref _rememberpassword, value); }
        }

        private bool _modifypassword;
        public bool Modifypassword
        {
            get { return _modifypassword; }
            set { SetProperty(ref _modifypassword, value); }
        }

        private bool _userNameChange = true;
        public bool UserNameChange
        {
            get { return _userNameChange; }
            set { SetProperty(ref _userNameChange, value); }
        }

        private bool _flip;
        public bool Flip
        {
            get { return _flip; }
            set { SetProperty(ref _flip, value); }
        }

        public DelegateCommand<object[]> LoginCommand => new(Login);

        public DelegateCommand<object> ChangePasswordCommand => new(ChangePassword);

        public LoginViewModel()
        {
            Users.Add("操作人员");
            Users.Add("调试人员");
            Users.Add("开发人员");
        }

        private void Login(object[] obj)
        {
            
            if (obj[0] is Window w && obj[1] is Snackbar sb)
            {
                if(string.IsNullOrEmpty(UserName))
                {
                    sb.MessageQueue?.Enqueue($"用户不可为空", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    return;
                }

                if (string.IsNullOrEmpty(Password))
                {
                    sb.MessageQueue?.Enqueue($"密码不可为空", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    return;
                }

                if (Password != "666")
                {
                    sb.MessageQueue?.Enqueue($"密码错误", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    return;
                }

                if (Modifypassword)
                {
                    UserNameChange = false;
                    Flip = true;
                    return;
                }
                w.DialogResult = true;
            }


        }

        private void ChangePassword(object obj)
        {
            if (obj is Snackbar sb)
            {
                if (string.IsNullOrEmpty(Newpassword))
                {
                    sb.MessageQueue?.Enqueue($"新密码不可为空", null, null, null, false, true,TimeSpan.FromSeconds(2));
                    return;
                }

                if (Newpassword != Confirmpassword)
                {
                    sb.MessageQueue?.Enqueue($"两次密码输入不相同", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    return;
                }
                Flip = false;
                UserNameChange = true;
            }
        }


    }
}
