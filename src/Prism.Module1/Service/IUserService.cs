using Prism.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Module1.Service
{
    /// <summary>
    /// interfejsc dla usług obsługi danych użytkowników.
    /// </summary>
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        void Save(User user);
    }
}
