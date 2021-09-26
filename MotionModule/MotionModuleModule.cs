﻿using MotionModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MotionModule
{
    public class MotionModuleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            string aa = "";
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MotionMain>();
        }
    }
}