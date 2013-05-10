using Microsoft.Practices.Prism.Commands;
using Prism.Entities.Users;
using Prism.Infrastucture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Module1.ViewModels
{
    public interface IManageUserViewModel : IViewModel
    {
        User User { get; set; }
        DelegateCommand SaveUserCommand { get; }
        DelegateCommand CancelCommand { get; }
    }
}
