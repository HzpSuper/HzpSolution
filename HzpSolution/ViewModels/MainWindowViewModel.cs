using EventAggregator;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HzpSolution.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private readonly IEventAggregator _ea;

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            _regionManager = regionManager;

            _ea = ea;

            IniNavigate();

            IniMessageManage();
        }

        #region 导航部分
        private IRegionNavigationJournal? _journa;

        public DelegateCommand<object>? NavigateCommand { get; private set; }

        public DelegateCommand? NavigateGobackCommand { get; private set; }

        public DelegateCommand? NavigateGoForwardCommand { get; private set; }

        private bool _isOpenMenu;
        public  bool IsOpenMenu
        {
            get { return _isOpenMenu; }
            set { SetProperty(ref _isOpenMenu, value); }
        }

        private string? _searchKeyword;
        public  string? SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                if (SetProperty(ref _searchKeyword, value))
                {
                    _menuItemsView?.Refresh();
                }
            }
        }

        private ICollectionView? _menuItemsView;
        private ObservableCollection<MenuTreeNode> _menuTreeNodes = new();
        public  ObservableCollection<MenuTreeNode> MenuTreeNodes
        {
            get { return _menuTreeNodes; }
            set { SetProperty(ref _menuTreeNodes, value); }
        }

        private void IniNavigate()
        {
            NavigateCommand = new DelegateCommand<object>(Navigate);
            NavigateGobackCommand = new DelegateCommand(NavigateGoBack);
            NavigateGoForwardCommand = new DelegateCommand(NavigateGoForward);

            MenuManage mm = new();
            List<MenuTreeNode> nodes = new()
            {
                new() { MenuName = "视图" },
                new() { ParentMenuName = "视图", MenuName = "视觉显示", ViewName = "VisionMain" },
                new() { ParentMenuName = "视图", MenuName = "视觉模板", ViewName = "VisionEdit" },
                new() { ParentMenuName = "视图", MenuName = "运动控制", ViewName = "MotionMain"},
                new() { MenuName = "设置", Icon = "CogOutline" },
                new() { ParentMenuName = "设置", MenuName = "主题样式", Icon = "PaletteOutline" },
                new() { ParentMenuName = "设置", MenuName = "日志存储", Icon = "MessageTextOutline" },
                new() { ParentMenuName = "设置", MenuName = "消息显示", Icon = "MessageProcessingOutline" }


                , new() { ParentMenuName = "设置", MenuName = "主题样式", Icon = "PaletteOutline" },
                new() { ParentMenuName = "设置", MenuName = "日志存储", Icon = "MessageTextOutline" },
                new() { ParentMenuName = "设置", MenuName = "消息显示", Icon = "MessageProcessingOutline" },
                new() { ParentMenuName = "设置", MenuName = "主题样式", Icon = "PaletteOutline" },
                new() { ParentMenuName = "设置", MenuName = "日志存储", Icon = "MessageTextOutline" },
                new() { ParentMenuName = "设置", MenuName = "消息显示", Icon = "MessageProcessingOutline" }
            };
            mm.IniMenuNodes(nodes)?.ForEach(x => _menuTreeNodes.Add(x));

            _menuItemsView = CollectionViewSource.GetDefaultView(MenuTreeNodes);
            _menuItemsView.Filter = DemoItemsFilter;
        }

        private void Navigate(object menutreenode)
        {
            if(menutreenode is MenuTreeNode m)
            {
                if(m.ViewName != null)
                {
                    _regionManager.RequestNavigate(Constants.MainRegion, m.ViewName, arg => _journa = arg.Context.NavigationService.Journal);
                    IsOpenMenu = false;
                }
            }
        }

        private void NavigateGoBack()
        {
            _journa?.GoBack();
        }

        private void NavigateGoForward()
        {
            _journa?.GoForward();
        }

        private bool DemoItemsFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(_searchKeyword))
            {
                return true;
            }

            return obj is MenuTreeNode item && !string.IsNullOrWhiteSpace(item.MenuName)
                   && (item.MenuName.Contains(_searchKeyword) || item.ChildMenuNodes.All(x=>x.MenuName == _searchKeyword));
        }
        #endregion



        #region 消息处理部分
        private ObservableCollection<MessageData> _messageDatas = new();
        public ObservableCollection<MessageData> MessageDatas
        {
            get { return _messageDatas; }
            set { SetProperty(ref _messageDatas, value); }
        }

        private ObservableCollection<MessageShow> _messageDatasShow = new();
        public ObservableCollection<MessageShow> MessageDatasShow
        {
            get { return _messageDatasShow; }
            set { SetProperty(ref _messageDatasShow, value); }
        }

        public DelegateCommand<MessageLevel?>? MessageSwitchoverCommand { get; private set; }

        private MessageManage? messageManage;

        private void IniMessageManage()
        {
            messageManage = new(_messageDatas, _messageDatasShow);
            _ea.GetEvent<MessageSentEvent>().Subscribe(MessageReceived, ThreadOption.PublisherThread, false);
            MessageSwitchoverCommand = new DelegateCommand<MessageLevel?>(MessageSwitchover);

            _messageDatas.Add(new() { Name = "消息", Numeric = 8, IsSelected = false, Messagelevel = MessageLevel.Information });
            _messageDatas.Add(new() { Name = "错误", Numeric = null, IsSelected = true, Messagelevel = MessageLevel.Error });
            _messageDatas.Add(new() { Name = "警告", Numeric = 5, IsSelected = false, Messagelevel = MessageLevel.Warning });

            //_messageDatasShow.Add(new(){ MessageTime = DateTime.Now,MessageContext = "哈哈哈" });
            //_messageDatasShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈1" });
            //_messageDatasShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈2" });
            //_messageDatasShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈3" });
            //_messageDatasShow.Add(new() { MessageTime = DateTime.Now, MessageContext = "哈哈哈4" });
        }

        private void MessageSwitchover(MessageLevel? messageLevel)
        {
            messageManage?.MessageSwitchover(messageLevel);
        }

        private void MessageReceived((string messagecontent, MessageLevel messageLevel) message)
        {
            _messageDatasShow.Add(new() { MessageTime = DateTime.Now, MessageContext = message.messagecontent });
        }
        #endregion

    }

}
