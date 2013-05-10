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

        public UsersListViewModel(IUserService userService, IEventAggregator eventAggregator, IUsersListView view)
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

            _eventAggregator.GetEvent<SavedUserEvent>().Subscribe(ProcessSavedUserMessage);
        }

        public void ProcessSavedUserMessage(SavedUserMessage message)
        {
            if (message.IsNewUser)
            {
                Users.Add(message.User);
            }
        }

        public void Load()
        {
            var data = _userService.GetUsers();
            Users = data == null ? new ObservableCollection<User>() : new ObservableCollection<User>(data);
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

        private DelegateCommand _refreshUsersListCommand;
        public DelegateCommand RefreshUsersListCommand
        {
            get
            {
                if (_refreshUsersListCommand == null)
                {
                    _refreshUsersListCommand = new DelegateCommand(Load);
                }
                return _refreshUsersListCommand;
            }
        }

        private DelegateCommand _addUserCommand;
        public DelegateCommand AddUserCommand
        {
            get
            {
                if (_addUserCommand == null)
                {
                    _addUserCommand = new DelegateCommand(
                    () => _eventAggregator
                        .GetEvent<ShowManageUserEvent>()
                        .Publish(new ShowManageUserViewMessage()));
                }
                return _addUserCommand;
            }
        }

        private DelegateCommand<User> _editUserCommand;
        public DelegateCommand<User> EditUserCommand
        {
            get
            {
                if (_editUserCommand == null)
                {
                    _editUserCommand = new DelegateCommand<User>(
                        user => _eventAggregator
                            .GetEvent<ShowManageUserEvent>()
                            .Publish(new ShowManageUserViewMessage()
                            {
                                User = user
                            })
                    );
                }
                return _editUserCommand;
            }
        }
    }
}
