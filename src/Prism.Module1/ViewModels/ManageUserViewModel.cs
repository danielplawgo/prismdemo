using Microsoft.Practices.Prism.Events;
using Prism.Entities.Users;
using Prism.Module1.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Prism.Infrastucture;
using Prism.Module1.Views;
using Microsoft.Practices.Prism.Regions;
using Prism.Module1.Messages;

namespace Prism.Module1.ViewModels
{
    public class ManageUserViewModel : BaseViewModel, IManageUserViewModel
    {
        private IUserService _userService;
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        public ManageUserViewModel(IUserService userService, IEventAggregator eventAggregator, IRegionManager regionManager, IManageUserView view)
            : base(view)
        {
            _userService = userService;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var userKey = navigationContext.Parameters[Strings.User];

            if (string.IsNullOrEmpty(userKey))
            {
                return false;
            }
            return true;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);//very important line of code

            var userKey = navigationContext.Parameters[Strings.User];

            if (string.IsNullOrEmpty(userKey))
            {
                IsEditMode = false;
                User = new User();
            }
            else
            {
                IsEditMode = true;
                User = NavigationParameters.Get<User>(userKey);
            }
        }

        private User _user;
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    _user.PropertyChanged += UserPropertyChanged;
                    OnPropertyChanged(() => this.User);
                }
            }
        }

        void UserPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveUserCommand.RaiseCanExecuteChanged();
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get
            {
                return _isEditMode;
            }
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(() => this.IsEditMode);
                }
            }
        }

        private DelegateCommand _saveUserCommand;
        public DelegateCommand SaveUserCommand
        {
            get
            {
                if (_saveUserCommand == null)
                {
                    _saveUserCommand = new DelegateCommand(
                    () =>
                    {
                        _userService.Save(User);
                        if (IsEditMode == false)
                        {
                            _eventAggregator
                                .GetEvent<AddedUserEvent>()
                                .Publish(new AddedUserMessage() { User = User });
                            _regionManager.Regions[RegionNames.Main].RequestNavigate<IUsersListViewModel>();
                        }
                    },
                    () => User != null && User.IsValid);
                    
                }
                return _saveUserCommand;
            }
        }

        public DelegateCommand CancelCommand
        {
            get
            {
                return new DelegateCommand(
                    () => _regionManager.Regions[RegionNames.Main].RequestNavigate<IUsersListViewModel>()
                );
            }
        }
    }
}
