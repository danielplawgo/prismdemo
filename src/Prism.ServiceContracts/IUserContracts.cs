using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Prism.Entities.Users;

namespace Prism.ServiceContracts
{
    [ServiceContract]
    public interface IUserContracts
    {
        [OperationContract]
        IEnumerable<User> GetUsers(int index, int count);
        [OperationContract]
        int GetUsersCount(); 
    }
}
