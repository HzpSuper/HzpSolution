using AvalonDock;
using Config.Net;
using HzpSolution.Views;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Serilog;
using Serilog.Events;
using System;
using System.Threading;
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
        protected override Window? CreateShell()
        {
            _ = new Mutex(false, "solutionAppMutex", out bool mutexCreated);
            if (!mutexCreated)
            {
                Current.Shutdown();
                return null;
            }

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

            IniLog();

            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            if (Container.Resolve<LoginView>().ShowDialog() == false)
            {
                Current.Shutdown();
            }
            else
            {
                base.InitializeShell(shell);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            _ = moduleCatalog.AddModule<WindowsMainModule>();
        }


        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(DockingManager), Container.Resolve<DockingManagerRegionAdapter>());
        }

        

        private static void IniLog()
        {
            MessageLogSettings _ml = MessageLogSettings.Instance;
            string?  savepath = System.IO.Directory.Exists(_ml.Imessagelogsettings.SavePath)
                ? _ml.Imessagelogsettings.SavePath
                : $@"{AppContext.BaseDirectory}Logs";
            int savedays = _ml.Imessagelogsettings.SaveDays;
            string LogFilePath(string LogEvent)
            {
                return $@"{savepath}\{LogEvent}\log.log";
            }

            string SerilogOutputTemplate = "{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 50);

            Log.Logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .MinimumLevel.Debug() // 所有Sink的最小记录级别
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Verbose).WriteTo.Async(a => a.File(LogFilePath("Verbose"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: savedays)))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.Async(a => a.File(LogFilePath("Debug"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: savedays)))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.Async(a => a.File(LogFilePath("Information"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: savedays)))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.Async(a => a.File(LogFilePath("Warning"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: savedays)))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.Async(a => a.File(LogFilePath("Error"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: savedays)))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.Async(a => a.File(LogFilePath("Fatal"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: savedays)))
           .CreateLogger();
        }
    }
}
