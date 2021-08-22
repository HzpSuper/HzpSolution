using HzpSolution.MessageBoxExtend;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace HzpSolution.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await  MessageBoxE.ShowAsync("HHHH",MessageBoxButtonE.OKCancel,MessageBoxImageE.Error);

        }


        #region 主窗体常规按钮
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DemoItemsSearchBox.Focus();
        }

        private void MinWindow_Click(object sender, RoutedEventArgs e)
        {
           this.WindowState = WindowState.Minimized;
        }

        private bool _isWindowMax = true;
        private void MaxWindow_Click(object sender, RoutedEventArgs e)
        {
            if(_isWindowMax)
            {
                this.WindowState = WindowState.Normal;
                this.MaxWindowPackion.Kind = PackIconKind.BorderAllVariant;
                this.MaxWindowToolTip.Content = "最大化";
                _isWindowMax = false;

            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.MaxWindowPackion.Kind = PackIconKind.ImageFilterNone;
                this.MaxWindowToolTip.Content = "向下还原";
                _isWindowMax = true;
            }        
        }
        #endregion



    }
}
