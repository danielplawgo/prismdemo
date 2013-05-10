using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Prism.Module1.Views;
using Prism.Module1.Service;
using Prism.Module1.ViewModels;
using Prism.Infrastucture;

namespace Prism.Module1
{
    public class Module1 : BaseModule
    {
        public IUnityContainer Container { get; private set; }

        public IRegionManager RegionManager { get; private set; }

        public IEventAggregator EventAggregator { get; private set; }

        public Module1(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggreagator)
        {
            RegionManager = regionManager;
            Container = container;
            EventAggregator = eventAggreagator;
        }

        public override void Initialize()
        {
            Container.RegisterType<IUserService, MockUserService>();
            Container.RegisterType<IUsersListView, UsersListView>();
            Container.RegisterTypeForNavigation<IUsersListViewModel, UsersListViewModel>();
            Container.RegisterType<IManageUserView, ManageUserView>();
            Container.RegisterTypeForNavigation<IManageUserViewModel, ManageUserViewModel>();
            
            RegionManager.Regions[RegionNames.Main].RequestNavigate<IUsersListViewModel>();
        }
    }
}
