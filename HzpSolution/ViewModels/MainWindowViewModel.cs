using EventAggregator;
using HzpSolution.Common;
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

        public DelegateCommand<object> NavigateCommand => new(Navigate);

        public DelegateCommand NavigateGobackCommand => new(NavigateGoBack);

        public DelegateCommand NavigateGoForwardCommand => new(NavigateGoForward);

        private bool _isOpenMenu;
        public bool IsOpenMenu
        {
            get => _isOpenMenu;
            set => SetProperty(ref _isOpenMenu, value);
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
            new MenuManage().IniMenuNodes(Constants.Configs + "MenuManage.json")?.ForEach(x => _menuTreeNodes.Add(x));
            _menuItemsView = CollectionViewSource.GetDefaultView(MenuTreeNodes);
            _menuItemsView.Filter = MenuItemsFilter;
        }

        private void Navigate(object menutreenode)
        {
            if (menutreenode is MenuTreeNode m)
            {
                if (m.ViewName != null)
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

        private bool MenuItemsFilter(object obj)
        {
            if (obj is not MenuTreeNode menutreenode)
            {
                return true;
            }
            else
            {
                bool result = false;
                if (string.IsNullOrWhiteSpace(SearchKeyword) || (!string.IsNullOrWhiteSpace(SearchKeyword) && menutreenode.MenuName.Contains(SearchKeyword)))
                {
                    menutreenode.Visible = System.Windows.Visibility.Visible;
                    result = true;
                }

                if (menutreenode.ChildMenuNodes !=null)
                {
                    foreach (MenuTreeNode node in menutreenode.ChildMenuNodes)
                    {
                        result = MenuItemsChildrenFilter(node) || result;
                    }
                }
                return result;
            }
        }

        private bool MenuItemsChildrenFilter(MenuTreeNode  node)
        {
            bool result = false;
            if (node.ChildMenuNodes != null)
            {
                foreach (MenuTreeNode item in node.ChildMenuNodes)
                {
                    result = MenuItemsChildrenFilter(item) || result;
                }
            }

            if(string.IsNullOrEmpty(SearchKeyword))
            {
                result = true;
                node.Visible = System.Windows.Visibility.Visible;
            }
            else
            {
                if (result || node.MenuName.Contains(SearchKeyword))
                {
                    result = true;
                    node.Visible = System.Windows.Visibility.Visible;
                }
                else
                {
                    node.Visible = System.Windows.Visibility.Collapsed;
                }
            }
            return result;
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
            set { SetProperty(ref _messageDatasShow, value);}
        }

        public DelegateCommand<MessageLevel?> MessageSwitchoverCommand => new(MessageSwitchover);

        private MessageManage? messageManage;

        private void IniMessageManage()
        {
            messageManage = new(MessageDatas, MessageDatasShow);
            _ = _ea.GetEvent<MessageSentEvent>().Subscribe(MessageReceived, ThreadOption.PublisherThread, false);
        }

        private void MessageSwitchover(MessageLevel? messageLevel)
        {
            messageManage?.MessageSwitchover(messageLevel);
        }

        private void MessageReceived((string messagecontent, MessageLevel messageLevel) message)
        {
            messageManage?.MessageReceived(message.messagecontent, message.messageLevel);
        }
        #endregion  
    }

}
