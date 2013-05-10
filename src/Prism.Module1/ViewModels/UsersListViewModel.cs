using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Prism.Entities.Users;
using Prism.Module1.Service;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Prism.Infrastucture;
using Prism.Module1.Views;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism;
using Prism.Module1.Messages;


namespace Prism.Module1.ViewModels
{
    public class UsersListViewModel : BaseViewModel, IUsersListViewModel
    {
        private IUserService _userService;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        public UsersListViewModel(IUserService userService, IEventAggregator eventAggregator, IRegionManager regionManager, IUsersListView view)
            : base(view)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;

            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            _eventAggregator.GetEvent<AddedUserEvent>().Subscribe(ProcessAddedUserMessage);
        }

        private void ProcessAddedUserMessage(AddedUserMessage message)
        {
            Users.Add(message.User);
        }

        public void Load()
        {
            var data = _userService.GetUsers();
            if (data == null)
            {
                Users = new ObservableCollection<User>();
            }
            else
            {
                Users = new ObservableCollection<User>(data);
            }
        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get
            {
                if (_users == null)
                {
                    Load();
                }
                return _users;
            }
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged(() => this.Users);
                }
            }
        }

        public DelegateCommand RefreshUsersListCommand
        {
            get
            {
                return new DelegateCommand(
                    () =>
                    {
                        Load();
                    });
            }
        }

        public DelegateCommand AddUserCommand
        {
            get
            {
                return new DelegateCommand(
                    () =>
                    {
                        _regionManager.Regions[RegionNames.Main].RequestNavigate<IManageUserViewModel>();
                    });
            }
        }


        public DelegateCommand<User> EditUserCommand
        {
            get {
                return new DelegateCommand<User>(
                        user =>
                        {
                            UriQuery query = new UriQuery();
                            var key = NavigationParameters.Add<User>(user);
                            query.Add(Strings.User, key);

                            _regionManager.Regions[RegionNames.Main].RequestNavigate<IManageUserViewModel>(query);
                        }
                    );
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            OnPropertyChanged(() => this.Users);
        }
    }
}
