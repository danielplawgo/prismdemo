using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Entities.Users;

namespace Prism.Entities.Interfaces
{
    interface IUsersRepository
    {
        IEnumerable<User> GetUsers(int index, int count);
    }
}
