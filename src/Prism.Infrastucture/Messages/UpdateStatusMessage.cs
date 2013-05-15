using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;

namespace Prism.Infrastucture.Messages
{
    public class UpdateStatusMessage : BaseMessage
    {
        public string Value { get; set; }
    }

    public class UpdateStatusEvent : CompositePresentationEvent<UpdateStatusMessage>{}
}
