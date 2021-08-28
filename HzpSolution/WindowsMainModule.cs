using HzpSolution.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace HzpSolution
{
    public class WindowsMainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Palette>();
        }
    }


}
