using Microsoft.Practices.Prism.Commands;
using Prism.Entities.Users;
using Prism.Infrastucture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Prism.Module1.ViewModels
{
    public interface IUsersListViewModel : IViewModel
    {
        bool IsBusy { get; set; }
        ObservableCollection<User> Users { get; set; }
        User SelectedUser { get; set; }
        DelegateCommand RefreshUsersListCommand { get;  }
        DelegateCommand AddUserCommand { get; }
        DelegateCommand<User> EditUserCommand { get; }
        DelegateCommand EditSelectedUserInDialogBoxCommand { get; }
        void Load();
        void LoadAsync();
        DelegateCommand CancelLoadDataCommand { get; }
    }
}
