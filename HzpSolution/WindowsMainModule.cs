using HzpSolution.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace HzpSolution
{
    public class WindowsMainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //var regionManager = containerProvider.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion(Constants.MessageRegion ,typeof(MessageView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PaletteView>();
            containerRegistry.RegisterForNavigation<MessageLogSetView>();
        }
    }


}
