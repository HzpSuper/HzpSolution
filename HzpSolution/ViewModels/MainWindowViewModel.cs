using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace HzpSolution.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IContainerExtension _container;
        private IRegionNavigationJournal _journa ;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public DelegateCommand NavigateGoback { get; private set; }

        public DelegateCommand NavigateGoForward { get; private set; }

        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public MainWindowViewModel(IRegionManager regionManager, IContainerExtension container)
        {
            _regionManager = regionManager;
            _container = container;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            NavigateGoback  = new DelegateCommand(GoBack);
            NavigateGoForward = new DelegateCommand(GoForward);
        }
        #pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate(Constants.MainRegion, navigatePath,arg=>_journa = arg.Context.NavigationService.Journal);
            }
             
        }

        private void GoBack()
        {
            _journa.GoBack();
        }

        private void GoForward()
        {
            _journa.GoForward();
        }
    }
}
