using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Prism.Entities;
using Prism.Module1.Messages;
using Prism.Module1.Views;
using Prism.Module1.Service;
using Prism.Module1.ViewModels;
using Prism.Infrastucture;

namespace Prism.Module1
{
    public class Module1 : BaseModule
    {
        public Module1(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggreagator)
            :base(regionManager, container, eventAggreagator)
        {}

        public override void Initialize()
        {
            base.Initialize();
            EventAggregator.GetEvent<ShowUserListViewEvent>().Publish(new ShowUserListViewMessage());
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType<IUserService, MockUserService>();
            Container.RegisterType<IUsersListView, UsersListView>();
            Container.RegisterType<IUsersListViewModel, UsersListViewModel>();
            Container.RegisterType<IManageUserView, ManageUserView>();
            Container.RegisterType<IManageUserViewModel, ManageUserViewModel>();
        }

        protected override void ConfigureEventAggregator()
        {
            EventAggregator.GetEvent<ShowUserListViewEvent>().Subscribe(ProcessShowUserListViewMessage, ThreadOption.UIThread, true);
            EventAggregator.GetEvent<ShowManageUserEvent>().Subscribe(ProcessShowManageUserMessage, ThreadOption.UIThread, true);
            EventAggregator.GetEvent<SavedUserEvent>().Subscribe(ProcessSavedUserEvent, ThreadOption.UIThread, true);
        }

        private void ProcessSavedUserEvent(SavedUserMessage obj)
        {
            EventAggregator.GetEvent<ShowUserListViewEvent>().Publish(new ShowUserListViewMessage());
        }

        private IManageUserViewModel _manageUserViewModel;
        private void ProcessShowManageUserMessage(ShowManageUserViewMessage message)
        {
            if (_manageUserViewModel == null)
            {
                _manageUserViewModel = Container.Resolve<IManageUserViewModel>();
                MainRegion.Add(_manageUserViewModel.View);
            }

            _manageUserViewModel.User = message.User;
            MainRegion.Activate(_manageUserViewModel.View);
        }

        private IUsersListViewModel _usersListViewModel;
        private void ProcessShowUserListViewMessage(ShowUserListViewMessage message)
        {
            if (_usersListViewModel == null)
            {
                _usersListViewModel = Container.Resolve<IUsersListViewModel>();
                MainRegion.Add(_usersListViewModel.View);
            }

            if (message.Reload)
            {
                _usersListViewModel.Load();
            }
            MainRegion.Activate(_usersListViewModel.View);
        }
    }
}
