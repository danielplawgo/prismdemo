using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Entities.Interfaces;
using Prism.Entities.Users;

namespace Prism.ServiceDataAccess
{
    /// <summary>
    /// Przykładowa implementacji klasy zwracającej dane.
    /// Potrzebna jest ona do testów.
    /// W normalnym Pharmacy będzie to usługa wcfowa.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private List<User> _users;

        public UsersRepository()
        {
            _users = new List<User>();
            _users.Add(new User() { Name = "Daniel" });
            _users.Add(new User() { Name = "Tomek" });
            _users.Add(new User() { Name = "Ola" });
            _users.Add(new User() { Name = "Kubek" });

        }

        public IEnumerable<User> GetUsers(int index, int count)
        {
            return _users.Skip(index).Take(count);
        }


        public int GetUsersCount()
        {
            return _users.Count;
        }
    }
}
