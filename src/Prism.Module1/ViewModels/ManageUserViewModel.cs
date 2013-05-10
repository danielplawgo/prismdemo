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

        public ManageUserViewModel(IUserService userService, IEventAggregator eventAggregator, IManageUserView view)
            : base(view)
        {
            _userService = userService;
            _eventAggregator = eventAggregator;

            User = new User();
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
                    if (_user != null)
                    {
                        _user.PropertyChanged -= UserPropertyChanged;
                    }

                    _user = value;
                    if (_user == null)
                    {
                        _user = new User();
                        IsEditMode = false;
                    }
                    else
                    {
                        IsEditMode = true;
                    }
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
                        _eventAggregator
                                .GetEvent<SavedUserEvent>()
                                .Publish(new SavedUserMessage()
                                {
                                    User = User,
                                    IsNewUser = !IsEditMode
                                });
                    },
                    () => User != null && User.IsValid);

                }
                return _saveUserCommand;
            }
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand(
                     () => _eventAggregator
                         .GetEvent<ShowUserListViewEvent>()
                         .Publish(new ShowUserListViewMessage())
                 );
                }
                return _cancelCommand;
            }
        }
    }
}
