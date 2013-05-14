using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;

namespace Prism.Infrastucture.Messages
{
    public class UpdateStatusBarMessage : BaseMessage
    {
        public string Value { get; set; }
    }

    public class UpdateStatusBarEvent : CompositePresentationEvent<UpdateStatusBarMessage>{}
}
