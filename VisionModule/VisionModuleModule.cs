using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using VisionModule.Views;

namespace VisionModule
{
    public class VisionModuleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<VisionMain>();
            containerRegistry.RegisterForNavigation<VisionEdit>();
        }
    }
}