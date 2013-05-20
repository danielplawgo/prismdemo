using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Prism.ServiceContracts;

namespace Prism.Infrastucture.WcfService
{
    public class WcfCoreFactory : IWcfCoreFactory
    {
        private string _address = "net.tcp://localhost:8089";
        
        private IUserContracts _userContracts;
        
        public IUserContracts GetCore(bool force = false)
        {
            if (_userContracts == null || force)
            {
                _userContracts = CreateChannel(_address);
            }
            return _userContracts;
        }

        private void OnFaulted(object sender, EventArgs eventArgs)
        {
            _userContracts = CreateChannel(_address);
        }

        private IUserContracts CreateChannel(string address)
        {
            Binding binding = GetBinding();
            ChannelFactory<IUserContracts> channelFactory = new ChannelFactory<IUserContracts>(binding, new EndpointAddress(address));
            var channel = channelFactory.CreateChannel();
            ((ICommunicationObject)channel).Faulted += OnFaulted;
            return channel;
        }

        private Binding GetBinding()
        {
            NetTcpBinding binding = new NetTcpBinding();
            return binding;
        }
    }
}
