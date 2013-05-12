using Prism.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Module1.Service
{
    /// <summary>
    /// Fakowa implementacja usługi obsługi danych użytkowników.
    /// </summary>
    public class MockUserService : IUserService
    {
        public IEnumerable<Entities.Users.User> GetUsers()
        {
            List<User> users = new List<User>();
            users.Add(new User() { Name = "Daniel" });
            users.Add(new User() { Name = "Tomek" });
            users.Add(new User() { Name = "Ola" });
            users.Add(new User() { Name = "Kubek" });

            return users;
        }


        public void Save(User user)
        {
            return;
        }
    }
}
