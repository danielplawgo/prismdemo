using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Prism.Entities.Users;
using Prism.Infrastucture.Messages;

namespace Prism.Module1.Messages
{
    public class AddedUserMessage : BaseMessage
    {
        public User User { get; set; }
    }

    public class AddedUserEvent : CompositePresentationEvent<AddedUserMessage>{}
}
