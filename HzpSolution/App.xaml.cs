﻿using AvalonDock;
using HzpSolution.Views;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using System.Windows.Media;
using Zametek.Wpf.Core;

namespace HzpSolution
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            #region 设置主题颜色
            PaletteHelper paletteHelper = new();
            ITheme theme = paletteHelper.GetTheme();

            Color colorprimary = Colors.MediumPurple;
            theme.PrimaryLight = new ColorPair(colorprimary.Lighten());
            theme.PrimaryMid = new ColorPair(colorprimary);
            theme.PrimaryDark = new ColorPair(colorprimary.Darken());

            Color colorsecondary = Colors.YellowGreen;
            theme.SecondaryLight = new ColorPair(colorsecondary.Lighten());
            theme.SecondaryMid = new ColorPair(colorsecondary);
            theme.SecondaryDark = new ColorPair(colorsecondary.Darken());

            bool isDarkTheme = false; 
            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);

            #endregion

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }


        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(DockingManager), Container.Resolve<DockingManagerRegionAdapter>());  
        }

    }
}
