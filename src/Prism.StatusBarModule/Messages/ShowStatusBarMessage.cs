using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Infrastucture.Messages;
using Microsoft.Practices.Prism.Events;

namespace Prism.StatusBarModule.Messages
{
    public class ShowStatusBarMessage : BaseMessage
    {
    }

    public  class ShowStatusBarEvent : CompositePresentationEvent<ShowStatusBarMessage>{}
}
