﻿using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Prism.Infrastucture
{
    /// <summary>
    /// Klasa bazowa dla modułów.
    /// </summary>
    public abstract class BaseModule : IModule
    {
        public IUnityContainer Container { get; private set; }

        public IRegionManager RegionManager { get; private set; }

        public IEventAggregator EventAggregator { get; private set; }

        /// <summary>
        /// Domyślny region w shellu - właściwość jest po to, aby łatwiej się pracowało z tym regionem
        /// </summary>
        protected IRegion MainRegion { get; set; }

        public BaseModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggreagator)
        {
            RegionManager = regionManager;
            Container = container;
            EventAggregator = eventAggreagator;
        }

        public virtual void Initialize()
        {
            MainRegion = RegionManager.Regions[RegionNames.Main];

            ConfigureContainer();
            ConfigureEventAggregator();
        }
        protected abstract void ConfigureContainer();
        protected abstract void ConfigureEventAggregator();
    }
}
