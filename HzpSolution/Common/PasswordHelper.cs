using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HzpSolution.Common
{
    public class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
          DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new PropertyMetadata(new PropertyChangedCallback(OnPropertyChanged)));

        public static string? GetPassword(DependencyObject obj)
        {
            return (string?)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string? value)
        {
            obj.SetValue(PasswordProperty, value);
        }


        public static readonly DependencyProperty AttchProperty =
          DependencyProperty.RegisterAttached("Attch", typeof(string), typeof(PasswordHelper), new PropertyMetadata(new PropertyChangedCallback(OnAttchChanged)));

        public static string? GetAttch(DependencyObject obj)
        {
            return (string?)obj.GetValue(AttchProperty);
        }

        public static void SetAttch(DependencyObject obj, string? value)
        {
            obj.SetValue(AttchProperty, value);
        }


        private static bool _isUpdating = false;

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pd)
            {
                pd.PasswordChanged -= Pb_PasswordChanged;
                if (!_isUpdating)
                {
                    pd.Password = e.NewValue.ToString();
                }
                pd.PasswordChanged += Pb_PasswordChanged;
            }
           
        }

        private static void OnAttchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pd)
            {
                pd.PasswordChanged += Pb_PasswordChanged;
            }
        }

        private static void Pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pd)
            {
                _isUpdating = true;
                SetPassword(pd,pd.Password);
                _isUpdating = false;
            }
        }


    }
}
