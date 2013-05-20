using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Entities.Interfaces;
using Prism.Entities.Users;
using Prism.ServiceContracts;
using Prism.ServiceDataAccess;

namespace Prism.WCFService
{
    public class UserService : IUserContracts
    {
        private IUsersRepository _usersRepository;

        public UserService()
        {
            _usersRepository = new UsersRepository();
        }

        public IEnumerable<User> GetUsers(int index, int count)
        {
            return _usersRepository.GetUsers(index, count);
        }

        public int GetUsersCount()
        {
            return _usersRepository.GetUsersCount();
        }
    }
}
