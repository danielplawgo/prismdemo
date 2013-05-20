using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Prism.ServiceContracts;
using Prism.WCFService;

namespace Prism.WCFConsolHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof (UserService)))
            {
                host.AddServiceEndpoint(typeof (IUserContracts), new NetTcpBinding(), "net.tcp://localhost:8089");
                host.Open();

                Console.ReadLine();
            }
        }
    }
}
