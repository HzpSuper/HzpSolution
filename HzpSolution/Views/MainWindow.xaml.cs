using HzpSolution.MessageBoxExtend;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media;
using Zametek.Wpf.Core;
using Serilog;

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

            #region 初始化时最大化窗体
            this.Left = 0.0;
            this.Top = 0.0;
            this.Height = SystemParameters.MaximizedPrimaryScreenHeight-10;
            this.Width = SystemParameters.MaximizedPrimaryScreenWidth-10;
            #endregion
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
            Log.CloseAndFlush();
            this.Close();
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
                this.Left = SystemParameters.MaximizedPrimaryScreenWidth / 2 - this.MinWidth  / 2;
                this.Top = SystemParameters.MaximizedPrimaryScreenHeight / 2 - this.MinHeight / 2;
                this.Height = this.MinHeight;
                this.Width  = this.MinWidth;
                this.MaxWindowPackion.Kind = PackIconKind.BorderAllVariant;
                this.MaxWindowToolTip.Content = "最大化";
                _isWindowMax = false;

            }
            else
            {
                this.Left = 0.0;
                this.Top = 0.0;
                this.Height = SystemParameters.MaximizedPrimaryScreenHeight-10;
                this.Width = SystemParameters.MaximizedPrimaryScreenWidth-10;
                this.MaxWindowPackion.Kind = PackIconKind.ImageFilterNone;
                this.MaxWindowToolTip.Content = "向下还原";
                _isWindowMax = true;
            }        
        }
        #endregion


        private void OpenMessageModul_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.IsChecked ?? false)
            {
                //MainGrid.RowDefinitions[4].Height = new System.Windows.GridLength(1, GridUnitType.Star);

                MainGrid.RowDefinitions[4].Height = new System.Windows.GridLength(100);
            }
            else
            {
                MainGrid.RowDefinitions[4].Height = new System.Windows.GridLength(0);
            }
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar? toolBar = sender as ToolBar;
            if (toolBar?.Template.FindName("OverflowButton", toolBar) is ToggleButton overflowGrid)
            {
                overflowGrid.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            }
        }


    }
}
