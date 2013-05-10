using Prism.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Module1.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        void Save(User user);
    }
}
